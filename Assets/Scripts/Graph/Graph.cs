using UnityEngine;

public class Graph : MonoBehaviour
{
    // The array holds all vertices of the graph
    public GameObject[] vertices;

    // Visualizes the graph by drawing arrows that represent the directed edges connecting the vertices
    private void OnDrawGizmos()
    {
        foreach (var vertex in vertices)
        {
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
                DrawArrow.ForGizmo(currentPos, adjacentVertexDir, Color.green);
            }
        }
    }
    
}
