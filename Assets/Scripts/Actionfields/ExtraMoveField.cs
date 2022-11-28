using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMoveField : Actionfield
{
    public override void performAction(Stone Piece)
    {
        Piece.MovePiece(-1);
    }

    public override void changeVisuals(GameObject Node)
    {
        Debug.Log("Hello, this is a ExtraMove-field");
    }
}
