using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerStrategy : MonoBehaviour
{
    private GameObject tutorial;
    private Func<GameObject> getTarget;
    private Action<GameObject> setTarget;
    private Action<bool> playerDetected;
    private LayerMask playerLayerMask;

    public void Configure(Func<GameObject> getTarget, Action<GameObject> setTarget, Action<bool> playerDetected, float detectionRadius, LayerMask playerLayerMask, GameObject tutorial)
    {
        this.tutorial = tutorial;
        this.getTarget = getTarget;
        this.setTarget = setTarget;
        this.playerDetected = playerDetected;
        this.playerLayerMask = playerLayerMask;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayerMask) != 0)
        {
            setTarget(other.gameObject);
            playerDetected(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayerMask) != 0)
        {
            playerDetected(false);
            setTarget(null);
            tutorial.SetActive(false);
        }
    }
}
