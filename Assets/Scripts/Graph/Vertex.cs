using UnityEngine;

public class Vertex : Node
{
    public GameObject[] adjacentVertices;

    private void OnDrawGizmos()
    {
        // Store the position of the current vertex
        var currentPos = this.transform.position;
             
        foreach (var adjacentVertex in adjacentVertices)
        {
            // Store the direction of the adjacent vertex
            var adjacentVertexDir = adjacentVertex.transform.position;
            // Draw an arrow from the current vertex in the direction of the adjacent vertex
            DrawArrow.ForGizmo(currentPos, adjacentVertexDir - currentPos, Color.green);
            
        }
    }
}