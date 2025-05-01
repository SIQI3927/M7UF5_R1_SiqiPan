using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : IStrategy
{
    private Func<bool> _condition;

    public Condition(Func<bool> _condition)
    {
        this._condition = _condition;
    }

    public Node.NodeState Process() => _condition() ? Node.NodeState.SUCCESS : Node.NodeState.FAILURE;
}
