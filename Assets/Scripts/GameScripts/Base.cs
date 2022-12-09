using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [NonSerialized] public List<Node> BaseNodes;

    void Awake()
    {
        BaseNodes = new (GetComponentsInChildren<Node>());
    }
}
