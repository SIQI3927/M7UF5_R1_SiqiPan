using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    readonly IStrategy strategy;
    readonly IStrategy strategy2;

    public Leaf(string name, IStrategy strategy, int priority = 0) : base(name, priority)
    {
        this.strategy = strategy;
    }

    //public Leaf(string name, IStrategy strategy1, IStrategy strategy2, int priority = 0) : base(name, priority)
    //{
    //    this.strategy1 = strategy1;
    //    this.strategy2 = strategy2;
    //}

    public override NodeState Process()
    {
        return strategy.Process();
    }

    public override void Reset()
    {
        strategy.Reset();
    }
}
