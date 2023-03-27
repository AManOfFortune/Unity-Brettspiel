using UnityEngine;

public class Debugfield : Actionfield
{
    public override void PerformAction(Stone Piece)
    {
        Debug.Log("Hello, this is a Debug-field");
    }

    public override void ChangeVisuals(GameObject Node)
    {
        Debug.Log("Debug-field doesnt have any Visuals");
    }
}
