using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolStrategy : IStrategy
{
    [SerializeField]
    private Transform entity;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private List<Transform> patrolPoints = new();
    private Condition detectCondition;
    [SerializeField]
    private float patrolSpeed;
    private int currentPoint = 0;
    private bool isPathCalculated;

    public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed,Condition detectCondition)
    {
        this.entity = entity;
        this.agent = agent;
        this.patrolPoints = patrolPoints;
        this.patrolSpeed = patrolSpeed;
        this.detectCondition = detectCondition;
    }

    public Node.NodeState Process()
    {
        if (detectCondition.Process() == Node.NodeState.SUCCESS)
        {
            agent.isStopped = true;
            return Node.NodeState.FAILURE;
        }
        else
        {
            agent.isStopped = false; // Reactivar movimiento
        }

        if (patrolPoints.Count == 0) return Node.NodeState.FAILURE; // Evitar errores

        if (currentPoint >= patrolPoints.Count) return Node.NodeState.SUCCESS;

        var target = patrolPoints[currentPoint];

        if (!agent.pathPending && agent.remainingDistance <= 2f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Count; // Reinicio de patrulla
            agent.SetDestination(patrolPoints[currentPoint].position);
        }

        agent.speed = patrolSpeed;

        return Node.NodeState.RUNNING;
        //var target = patrolPoints[currentPoint];
        //agent.SetDestination(target.position);

        //// Solo establecer un nuevo destino si no se está calculando el camino
        //if (!agent.pathPending && !isPathCalculated)
        //{
        //    agent.speed = patrolSpeed;
        //    agent.SetDestination(target.position);
        //    isPathCalculated = true;  // Marcamos que hemos asignado un destino
        //}

        //// Revisar si ha llegado al destino
        //if (!agent.pathPending && agent.remainingDistance <= 2f && agent.velocity.sqrMagnitude == 0)
        //{
        //    currentPoint++;
        //    isPathCalculated = false; // Permitir asignar el siguiente destino
        //}

        //return Node.NodeState.RUNNING;
    }

    public void Reset()
    {
        currentPoint = 0;
    }
}
