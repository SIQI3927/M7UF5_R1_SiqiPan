using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class KahootMiniGame : MiniGame
{
    [SerializeField]
    private KahootData_SO KahootData;

    private int currentQuestion;
    private NetworkVariable<int> numberOfRounds = new(-1);
    private NetworkVariable<int> networkCurrentQuestion = new(-1);
    private NetworkVariable<int> correctAnswer = new(-1);
    private NetworkVariable<double> roundEndTime = new(-1f);

    [SerializeField]
    private TMP_Text timer;
    [SerializeField]
    private TMP_Text questionTitle;
    [SerializeField]
    private List<TMP_Text> posibleAnswers;

    [SerializeField]
    private float points;

    private Coroutine timerCoroutine;
    private Coroutine endRoundCoroutine;
    private bool alreadyAnswer = false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            numberOfRounds.Value = KahootData.GetNumberOfRounds();
            networkRoundDuration.Value = gameManager.gameDuration / numberOfRounds.Value;
            networkCurrentQuestion.OnValueChanged += (oldValue, newValue) =>
            {
                if (newValue == 0)
                    StartGameClientRpc();
                if (newValue >= 0) // Evitar actualizar la UI si el valor es -1
                {
                    correctAnswer.Value = KahootData.GetCorrectAnswer(newValue);
                    UpdateQuestionUIClientRpc(newValue);
                    StartNewRound();
                    gameManager.ShowNewScoreClientRpc();
                }
            };
            GameStarted.OnValueChanged += (oldValue, newValue) =>
            {
                if (newValue)
                {
                    StartGameServerRpc();
                }
                else
                {
                    EndGameServerRpc();
                }
            };
        }

        // En todos los clientes, actualiza el temporizador en Update()
        roundEndTime.OnValueChanged += (oldValue, newValue) =>
        {
            if (timerCoroutine != null) StopCoroutine(timerCoroutine);
            timerCoroutine = StartCoroutine(UpdateTimerUI());
        };
    }

    private void StartNewRound()
    {
        alreadyAnswer = false;
        if (IsServer)
        {
            roundEndTime.Value = NetworkManager.ServerTime.Time + networkRoundDuration.Value;
            if (endRoundCoroutine != null) StopCoroutine(endRoundCoroutine);
            endRoundCoroutine = StartCoroutine(CheckEndOfRound());
        }
    }

    public override void StartGame()
    {
        StartGameServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        networkCurrentQuestion.Value = 0;
    }

    [ClientRpc]
    public void StartGameClientRpc()
    {
        currentQuestion = networkCurrentQuestion.Value;
        StartGame();
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndGameServerRpc()
    {
        networkCurrentQuestion.Value = -1;
        EndGameClientRpc();
    }

    [ClientRpc]
    public void EndGameClientRpc()
    {
        EndGame();
    }

    private IEnumerator CheckEndOfRound()
    {
        while (NetworkManager.ServerTime.Time < roundEndTime.Value)
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (networkCurrentQuestion.Value < KahootData.GetNumberOfRounds() - 1)
        {
            networkCurrentQuestion.Value += 1;
        }
        else
        {
            GameStarted.Value = false;
        }
    }

    private IEnumerator UpdateTimerUI()
    {
        while (roundEndTime.Value > 0)
        {
            double remainingTime = roundEndTime.Value - NetworkManager.ServerTime.Time;
            timer.text = Mathf.Max(0, (float)remainingTime).ToString("F0");

            if (remainingTime <= 0) yield break;

            yield return null;
        }
    }
    [ClientRpc]
    public void UpdateQuestionUIClientRpc(int newValue)
    {
        currentQuestion = newValue;
        questionTitle.text = KahootData.GetQuestionTitle(currentQuestion);
        for (int i = 0; i < posibleAnswers.Count; i++)
        {
            posibleAnswers[i].text = KahootData.GetAnswer(currentQuestion, i);
        }
    }

    public void CheckAnswer(int index)
    {
        CheckAnswerServerRpc(index, NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckAnswerServerRpc(int indexAnswer, ulong clientID)
    {
        if (KahootData.GetCorrectAnswer(currentQuestion) == indexAnswer && !alreadyAnswer)
        {
            print(currentQuestion);
            gameManager.addToScore(points, clientID);
            alreadyAnswer = true;
        }
    }
}

