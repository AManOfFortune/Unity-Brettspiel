using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Route : MonoBehaviour
{
    public List<Transform> childNodeList = new();

    // Start is called before the first frame update
    void Start()
    {
        FillNodes();
    }

    private void OnDrawGizmos() // Visualize node list by drawing lines between them
    {
        Gizmos.color = Color.green;
        FillNodes();

        for (int i = 1; i < childNodeList.Count; i++)
        {
            var currentPosition = childNodeList[i].position;

            var previousNodePosition = childNodeList[i - 1].position;

            Gizmos.DrawLine(previousNodePosition, currentPosition);
        }
    }

    private void FillNodes()
    {
        childNodeList.Clear();

        var childNodes = GetComponentsInChildren<Transform>();

        foreach(var child in childNodes)
        {
            if(child != transform) // Makes sure that route game object is not part of child node list
            {
                childNodeList.Add(child);
            }
        }
    }

    public int GetIndexOfPosition(Transform nodeTransform)
    {
        return childNodeList.IndexOf(nodeTransform);
    }
}
