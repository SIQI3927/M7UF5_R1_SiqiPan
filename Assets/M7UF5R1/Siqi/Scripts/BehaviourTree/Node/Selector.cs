using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(string name = "Node", int priority = 0) : base(name, priority)
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
                case NodeState.SUCCESS:
                    {
                        Reset();
                        return NodeState.SUCCESS;
                    }
                default:
                    currentChild++;
                    return NodeState.RUNNING;
            }
        }
        Reset();
        return NodeState.FAILURE;
    }
}
