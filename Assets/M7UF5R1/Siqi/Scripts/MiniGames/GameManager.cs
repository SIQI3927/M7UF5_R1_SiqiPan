using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class GameManager : NetworkBehaviour
{
    private List<GameObject> playerScores = new();
    public enum GameState { None, PreGame, OnGame, PostGame };

    [Header("Game Visuals")]
    public GameObject playerNamePlaceHolder;

    public GameObject playerScorePlaceHolder;

    public Transform scoreBoard;

    public TMP_Text gameName;

    [Header("Game properties")]
    public MiniGame miniGame;

    [SerializeField]
    private Transform gameArena;

    [SerializeField]
    private Transform postGameArena;

    [Header("Player properties")]
    [SerializeField]
    private TMP_Text playerNameSource;

    private TeleportationProvider teleportationProvider;


    #region NetworkVariables

    private NetworkVariable<GameState> networkGameState = new(GameState.None, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkList<ulong> networkGameQueue;
    private NetworkList<float> networkPlayerScore;
    private NetworkVariable<double> networkTimeToPrepare = new(double.MaxValue, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    #endregion

    [SerializeField]
    private int minNumberOfPLayers = 2;
    [SerializeField]
    private int maxNumberOfPlayers = 4;

    [SerializeField]
    private double timeToPrepare = 10;

    [SerializeField]
    public double gameDuration = 100;

    [SerializeField]
    public int postGameDuration = 0;

    private Coroutine pregameTimer;
    private Coroutine onGameTimer;
    private Coroutine postGameTimer;


    private void Awake()
    {
        networkGameQueue = new NetworkList<ulong>();
        networkPlayerScore = new NetworkList<float>();
    }

    public NetworkList<float> m_networkPlayerScore
    {
        get { return networkPlayerScore; }
    }

    public void addToScore(float newScore, ulong clientId)
    {
        int index = 0;
        while (networkGameQueue[index] != clientId)
        {
            index++;
        }
        networkPlayerScore[index] += newScore;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            //gameDuration = gameDuration + NetworkManager.ServerTime.Time;
            networkTimeToPrepare.OnValueChanged += (oldValue, newValue) =>
            {
                TimeClientRpc(newValue);
            };

            networkGameQueue.OnListChanged += (changeEvent) =>
            {
                if (networkGameState.Value == GameState.None && networkGameQueue.Count >= minNumberOfPLayers)
                {
                    networkGameState.Value = GameState.PreGame;
                }
            };

            networkGameState.OnValueChanged += (oldValue, newValue) =>
            {
                if (newValue == GameState.PreGame)
                    PreGameServerRpc();
                else if (newValue == GameState.OnGame)
                    OnGameServerRpc();
                else if (newValue == GameState.PostGame)
                    PostGameServerRpc();
            };
        }

        gameName.text = miniGame.gameName;
    }
    // Start is called before the first frame update
    void Start()
    {
        teleportationProvider = FindFirstObjectByType<TeleportationProvider>();
    }

    [ClientRpc]
    public void TimeClientRpc(double currentTime)
    {
        print($"Numero de jugadores en la queue # {networkGameQueue.Count}");
    }

    #region JOIN GAME
    public void JoinGame()
    {
        JoinClientToGameServerRpc(NetworkManager.Singleton.LocalClientId, playerNameSource.text);
    }

    [ServerRpc(RequireOwnership = false)]
    public void JoinClientToGameServerRpc(ulong clientId, string playerName)
    {
        if (networkGameQueue.Count < maxNumberOfPlayers && !networkGameQueue.Contains(clientId) && NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
        {
            networkGameQueue.Add(clientId);
            networkPlayerScore.Add(0);
            ShowNewScoreClientRpc(playerName, clientId);
        }
    }

    [ClientRpc]
    public void ShowNewScoreClientRpc(string playerName, ulong clientId)
    {
        GameObject playerScoreMiniGame = Instantiate(playerScorePlaceHolder, scoreBoard);
        playerScore newScore = playerScoreMiniGame.GetComponent<playerScore>();

        newScore.AddName(playerName);
        newScore.AddScore(0);
        newScore.clientId = clientId;
        playerScores.Add(playerScoreMiniGame);
    }

    [ServerRpc]
    public void TeleportPlayersServerRpc(Vector3 target)
    {
        List<Vector3> positions = new();
        List<ulong> playerIds = new();

        foreach (var player in networkGameQueue)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            positions.Add(target + randomOffset);
            playerIds.Add(player);
        }

        TeleportPlayerClientRpc(positions.ToArray(), playerIds.ToArray());
    }

    [ClientRpc]
    public void TeleportPlayerClientRpc(Vector3[] positions, ulong[] playerIds)
    {
        int index = System.Array.IndexOf(playerIds, NetworkManager.Singleton.LocalClientId);
        if (index >= 0)
        {
            TeleportToArea(positions[index], gameArena.rotation);
        }
    }
    void TeleportToArea(Vector3 position, Quaternion rotation)
    {
        TeleportRequest teleportRequest = new TeleportRequest
        {
            destinationPosition = position,
            destinationRotation = rotation,
            matchOrientation = MatchOrientation.TargetUpAndForward
        };

        teleportationProvider.QueueTeleportRequest(teleportRequest);
    }
    #endregion

    public IEnumerator timerPreGameCoroutine(double time)
    {
        while (networkTimeToPrepare.Value > 0)
        {
            networkTimeToPrepare.Value = time - NetworkManager.ServerTime.Time;
            print(Mathf.Max(0, (float)networkTimeToPrepare.Value).ToString("F0"));

            yield return null;
        }
        networkGameState.Value = GameState.OnGame;
    }
    [ServerRpc]
    public void PreGameServerRpc()
    {
        double auxTimeToPrepare = NetworkManager.ServerTime.Time + timeToPrepare;
        if (pregameTimer != null) StopCoroutine(pregameTimer);
        pregameTimer = StartCoroutine(timerPreGameCoroutine(auxTimeToPrepare));
    }
    [ServerRpc]
    public void OnGameServerRpc()
    {
        RequestStartMiniGameServerRpc();
        TeleportPlayersServerRpc(gameArena.position);
        OnGameDurationServerRpc();
    }

    public IEnumerator timerOnGameCoroutine(double time)
    {
        double remainingTime = double.MaxValue;
        while (remainingTime > 0)
        {
            remainingTime = time - NetworkManager.ServerTime.Time;
            yield return null;
        }
        networkGameState.Value = GameState.PostGame;
    }
    [ServerRpc]
    public void OnGameDurationServerRpc()
    {
        double auxGameDuration = gameDuration + NetworkManager.ServerTime.Time;
        if (onGameTimer != null) StopCoroutine(onGameTimer);
        onGameTimer = StartCoroutine(timerOnGameCoroutine(auxGameDuration));
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestStartMiniGameServerRpc()
    {
        miniGame.StartGame();
        for (int i = 0; i < networkGameQueue.Count; i++)
        {
            miniGame.playersOnGame.Add(networkGameQueue[i], networkPlayerScore[i]);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PostGameServerRpc()
    {
        print("!El juego ha terminado");
        TeleportPlayersServerRpc(postGameArena.position);
        ShowNewScoreClientRpc();
        PostGameDurationServerRpc();
    }

    [ClientRpc]
    public void ShowNewScoreClientRpc()
    {
        int i = 0;
        foreach (var player in playerScores)
        {
            player.GetComponent<playerScore>().AddScore(networkPlayerScore[i]);
            i++;
        }
    }

    public IEnumerator timerPostGameCoroutine(double time)
    {
        double remainingTime = double.MaxValue;
        while (remainingTime > 0)
        {
            remainingTime = time - NetworkManager.ServerTime.Time;
            yield return null;
        }
        networkGameState.Value = GameState.None;
        networkGameQueue.Clear();
        networkPlayerScore.Clear();
        ClearListClientRpc();
    }

    [ServerRpc]
    public void PostGameDurationServerRpc()
    {
        double auxPostGameTimer = postGameDuration + NetworkManager.ServerTime.Time;
        if (postGameTimer != null) StopCoroutine(postGameTimer);
        postGameTimer = StartCoroutine(timerPostGameCoroutine(auxPostGameTimer));
    }

    [ClientRpc(RequireOwnership = false)]
    public void ClearListClientRpc()
    {
        foreach (var item in playerScores)
            Destroy(item);
        playerScores.Clear();
    }
}

