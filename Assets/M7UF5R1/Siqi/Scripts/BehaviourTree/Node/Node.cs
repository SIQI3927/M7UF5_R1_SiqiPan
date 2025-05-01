using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public string name;
    public int priority;
    public List<Node> children = new();

    public int currentChild;

    public Node(string name = "Node", int priority = 0)
    {
        this.name = name;
        this.priority = priority;
    }

    public void AddChild(Node newChild)
    {
        children.Add(newChild);
    }

    public virtual NodeState Process()
    {
        return children[currentChild].Process();
    }

    public virtual void Reset()
    {
        currentChild = 0;
        foreach (Node child in children)
        {
            child.Reset();
        }
    }
}
