using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMoveField : Actionfield
{
    public override void performAction(Stone Piece)
    {
        //Piece.MovePiece(-1);
        Debug.Log("Yeah I don't work (anymore) but its fine, once Raoul goes crazy this would've needed to be changed anyway");
    }

    public override void changeVisuals(GameObject Node)
    {
        Debug.Log("Hello, this is a ExtraMove-field");
    }
}
