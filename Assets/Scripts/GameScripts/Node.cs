using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Actionfield> Actions = new List<Actionfield>();

    public List<Node> AdjacentNodes;

    [NonSerialized] public bool isTaken;
    [NonSerialized] public Stone stone;
    [NonSerialized] public GameObject selector;

    private void Start()
    {
        UpdateNode();
    }

    private void OnDrawGizmos()
    {
        // Store the position of the current vertex
        var currentPos = this.transform.position;

        foreach (var adjacentVertex in AdjacentNodes)
        {
            // Store the direction of the adjacent vertex
            var adjacentVertexDir = adjacentVertex.gameObject.transform.position;
            // Draw an arrow from the current vertex in the direction of the adjacent vertex
            DrawArrow.ForGizmo(currentPos, adjacentVertexDir - currentPos, Color.green);

        }
    }

    private void UpdateNode()
    {
        foreach (var Action in Actions)
        {
            Action.ChangeVisuals(this.gameObject);
        }

        //switch (gameObject.tag)
        //{
        //    case "Slipperyfield":
        //        transform.localScale = new Vector3(1f, 1.5f, 1f);
        //        break;
        //    case "Stickyfield":
        //        break;
        //    case "Actionfield":
        //        break;
        //    case "Defaultfield":
        //        break;
        //    default: gameObject.tag = "Defaultfield"; break;
        //}
    }

    public void ActivateSelector(bool state)
    {
        selector.gameObject.SetActive(state);
    }

    public void performActions(Stone Piece)
    {
        Debug.Log("Performing " + Actions.Count + " Actions");

        foreach (var Action in Actions)
        {
            Action.PerformAction(Piece);
        }
    }
}
