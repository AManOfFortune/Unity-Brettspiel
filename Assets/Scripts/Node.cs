using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isTaken;
    public List<Actionfield> OnEndActions = new List<Actionfield>();
    public List<Actionfield> StickyActions = new List<Actionfield>();
    public List<Actionfield> SlipperyActions = new List<Actionfield>();
    public Stone stone;

    private void Start()
    {
        UpdateNode();
    }

    private void UpdateNode()
    {
        this.gameObject.tag = (OnEndActions.Count > 0) ? "Actionfield" : "Defaultfield";

        foreach (IActionfield Action in OnEndActions)
        {
            Action.changeVisuals(this.gameObject);
        }

        if (SlipperyActions.Count > 0) this.gameObject.tag = "Slipperyfield";

        foreach (IActionfield Action in SlipperyActions)
        {
            Action.changeVisuals(this.gameObject);
        }

        if (StickyActions.Count > 0) this.gameObject.tag = "Stickyfield";

        foreach (IActionfield Action in StickyActions)
        {
            Action.changeVisuals(this.gameObject);
        }
    }

    public void performOnEndActions(Stone Piece)
    {
        Debug.Log("Performing " + OnEndActions.Count + " Actions");
        
        foreach(IActionfield Action in OnEndActions)
        {
            Action.performAction(Piece);
        }
    }

    public void performStickyActions(Stone Piece)
    {
        Debug.Log("Performing " + StickyActions.Count + " Actions");

        foreach (IActionfield Action in StickyActions)
        {
            Action.performAction(Piece);
        }
    }

    public void performSlipperyActions(Stone Piece)
    {
        Debug.Log("Performing " + SlipperyActions.Count + " Actions");

        foreach (IActionfield Action in SlipperyActions)
        {
            Action.performAction(Piece);
        }
    }
}
