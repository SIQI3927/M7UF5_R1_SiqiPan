using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStrategy
{
    Node.NodeState Process();
    void Reset()
    {
        //
    }
}
