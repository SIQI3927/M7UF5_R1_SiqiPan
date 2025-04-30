using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseStrategy : IStrategy
{
    [SerializeField]
    private Transform entity;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Func<GameObject> target;
    [SerializeField]
    private Condition chaseCondition;

    public ChaseStrategy(Transform entity, NavMeshAgent agent, Func<GameObject> target, Condition chaseCondition)
    {
        this.entity = entity;
        this.agent = agent;
        this.target = target;
        this.chaseCondition = chaseCondition;
    }

    public Node.NodeState Process()
    {
        if (chaseCondition.Process() == Node.NodeState.SUCCESS)
        {
            GameObject target = this.target();
            if (target == null)
                return Node.NodeState.FAILURE;
            //agent.speed = speedChase;
            //agent.SetDestination(target.transform.position);
            
            Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - entity.position);
            lookRotation.x = 0;
            lookRotation.z = 0;
            entity.rotation = Quaternion.Slerp(entity.rotation, lookRotation, Time.deltaTime * 5f);

            return Node.NodeState.RUNNING;
        }
        return Node.NodeState.FAILURE;
    }
}
