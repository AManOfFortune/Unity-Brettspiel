using UnityEngine;

public class SlipperyField : Actionfield
{
    public override void PerformAction(Stone Piece)
    {
        Debug.Log("Hello, this is a Debug-field");
    }

    public override void ChangeVisuals(GameObject Node)
    {
        transform.localScale = new Vector3(1f, 1.5f, 1f);
    }
}
