using UnityEngine;

public class ExtraMoveField : Actionfield
{
    public override void PerformAction(Stone Piece)
    {
        //Piece.MovePiece(-1);
        Debug.Log("Yeah I don't work (anymore) but its fine, once Raoul goes crazy this would've needed to be changed anyway");
    }

    public override void ChangeVisuals(GameObject Node)
    {
        Debug.Log("Hello, this is a ExtraMove-field");
    }
}
