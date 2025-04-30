using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string name = "Node", int priority = 0) : base(name, priority)
    {

    }

    public override NodeState Process()
    {
        if (currentChild < children.Count)
        {
            switch(children[currentChild].Process())
            {
                case NodeState.RUNNING:
                    {
                        return NodeState.RUNNING;
                    }
                case NodeState.FAILURE:
                    {
                        Reset();
                        return NodeState.FAILURE;
                    }
                default:
                    {
                        currentChild++;
                        return currentChild == children.Count ? NodeState.SUCCESS : NodeState.RUNNING;
                    }
            }
        }
        Reset();
        return NodeState.SUCCESS;
    }
}
