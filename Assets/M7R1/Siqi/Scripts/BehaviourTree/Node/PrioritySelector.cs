using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrioritySelector : Node
{
    private List<Node> sortedChildren;
    private List<Node> SortedChildren => sortedChildren ??= SortChildren();

    public virtual List<Node> SortChildren() => children.OrderByDescending(child => child.priority).ToList();

    public PrioritySelector(string name) : base(name)
    { }

    public override void Reset()
    {
        base.Reset();
        sortedChildren = null;
    }

    public override NodeState Process()
    {
        foreach (var child in SortedChildren)
        {
            switch (child.Process())
            {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
                case NodeState.SUCCESS:
                    return NodeState.SUCCESS;
                default:
                    continue;
            }
        }
        return NodeState.FAILURE;
    }
}
