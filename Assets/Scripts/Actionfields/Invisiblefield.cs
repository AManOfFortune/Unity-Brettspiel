using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisiblefield : Actionfield
{
    public override void performAction(Stone Piece)
    {
        Debug.Log("This is an invisible field");
    }

    public override void changeVisuals(GameObject Node)
    {
        Node.transform.localScale = new Vector3(0.5f, 1, 0.5f);
        Node.transform.tag = "InvisibleField";
    }
}
