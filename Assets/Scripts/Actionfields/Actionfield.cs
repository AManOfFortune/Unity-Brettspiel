using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actionfield : MonoBehaviour
{
    public virtual void PerformAction(Stone Piece)
    {
        Debug.Log("This is an empty Action field");
    }

    public virtual void ChangeVisuals(GameObject Node)
    {
        Debug.Log("This is an empty Action field");
    }
}
