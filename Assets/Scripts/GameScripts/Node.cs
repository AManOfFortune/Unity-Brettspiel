using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Actionfield> Actions = new List<Actionfield>();
    
    [NonSerialized] public bool isTaken;

    [NonSerialized] public Stone stone;

    private void Start()
    {
        UpdateNode();
    }

    private void UpdateNode()
    {
        switch (gameObject.tag)
        {
            case "Slipperyfield":
                transform.localScale = new Vector3(1f, 1.5f, 1f);
                break;
            case "Stickyfield":
                break;
            case "Actionfield":
                break;
            case "Defaultfield":
                break;
            default: gameObject.tag = "Defaultfield"; break;
        }
    }
    public void performActions(Stone Piece)
    {
        Debug.Log("Performing " + Actions.Count + " Actions");

        foreach (IActionfield Action in Actions)
        {
            Action.performAction(Piece);
        }
    }
}
