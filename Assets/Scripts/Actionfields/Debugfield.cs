using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugfield : Actionfield
{
    public override void performAction(Stone Piece)
    {
        Debug.Log("Hello, this is a Debug-field");
    }
}
