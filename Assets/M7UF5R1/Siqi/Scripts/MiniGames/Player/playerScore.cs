using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class playerScore : NetworkBehaviour
{
    [SerializeField]
    public TMP_Text player;

    [SerializeField]
    public TMP_Text score;
    // Variable de red para almacenar el puntaje del jugador
    public ulong clientId;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public void AddName(string name)
    {
        player.text = name;
        UpdateNameUI(name);
    }
    // Mtodo para agregar puntos (solo el servidor puede modificar NetworkVariables)
    public void AddScore(float points)
    {
        score.text += points;
        UpdateScoreUI(points);
    }

    // Mtodo para actualizar la UI del jugador
    private void UpdateScoreUI(float points)
    {
        score.text = points.ToString();
    }
    private void UpdateNameUI(string name)
    {
        player.text = name;
    }
}

