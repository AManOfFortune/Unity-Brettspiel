using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actionfield : MonoBehaviour, IActionfield
{
    public virtual void performAction(Stone Piece)
    {
        Debug.Log("This is an empty Action field");
    }
}
