using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;

public class MiniGame : NetworkBehaviour
{
    [SerializeField]
    internal GameManager gameManager;

    [SerializeField]
    public string gameName;

    public Dictionary<ulong, float> playersOnGame = new();

    [SerializeField]
    public List<GameObject> objectsForTheGame;

    [SerializeField]
    public NetworkVariable<double> networkRoundDuration = new();

    [SerializeField]
    public NetworkVariable<bool> GameStarted = new(false);

    public virtual void StartGame()
    {
        ActivateGameObject(true);
    }

    public virtual void EndGame()
    {
        ActivateGameObject(false);
    }
    public void ActivateGameObject(bool activate)
    {
        foreach (var obj in objectsForTheGame)
        {
            obj.gameObject.SetActive(activate);
        }
    }
}

