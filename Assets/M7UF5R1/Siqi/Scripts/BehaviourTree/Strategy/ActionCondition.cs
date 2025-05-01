using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCondition : IStrategy
{
    private Action doSomething;

    public ActionCondition(Action doSomething)
    {
        this.doSomething = doSomething;
    }

    public Node.NodeState Process()
    {
        doSomething();
        return Node.NodeState.SUCCESS;
    }
}
