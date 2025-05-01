using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : Node
{
    public BehaviourTree(string name = "Node") : base(name)
    {
    }

    public override NodeState Process()
    {
        while (currentChild < children.Count)
        {
            var status = children[currentChild].Process();
            if (status != NodeState.SUCCESS)
                return status;
            currentChild++;
        }
        return NodeState.SUCCESS;
    }
}
