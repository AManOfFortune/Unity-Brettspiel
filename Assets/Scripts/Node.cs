using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isTaken;
    public List<Actionfield> Actions = new List<Actionfield>();
    public Stone stone;


    private void Start()
    {
        this.gameObject.tag = (Actions.Count > 0) ? "Actionfield" : "Defaultfield";

        foreach (IActionfield Action in Actions)
        {
            Action.changeVisuals(this.gameObject);
        }
    }

    public void performActions(Stone Piece)
    {
        Debug.Log("Performing " + Actions.Count + " Actions");
        
        foreach(IActionfield Action in Actions)
        {
            Action.performAction(Piece);
        }
    }
}
