using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // The array holds all vertices of the graph
    public List<GameObject> vertices;

    // Visualizes the graph by drawing arrows that represent the directed edges connecting the vertices
    private void OnDrawGizmos()
    {
        for (var iterator = 0; iterator < vertices.Count; iterator++)
        {
            GameObject vertex = vertices[iterator];
            // Get the script of the vertex game object
            var script = vertex.GetComponent<Vertex>();
            // Store the position of the current vertex
            var currentPos = vertex.transform.position;
            
            // Iterate through the adjacency list of the current vertex
            foreach (var adjacentVertex in script.adjacentVertices)
            {
                // Store the direction of the adjacent vertex
                var adjacentVertexDir = adjacentVertex.transform.position;
                // Draw an arrow from the current vertex in the direction of the adjacent vertex
                DrawArrow.ForGizmo(currentPos, adjacentVertexDir - currentPos, Color.green);
                if (!adjacentVertex.GetComponent<Vertex>().AddedToList)
                {
                    vertices.Add(adjacentVertex);
                    adjacentVertex.GetComponent<Vertex>().AddedToList = true;
                }
            }
        }
    }
    
}
