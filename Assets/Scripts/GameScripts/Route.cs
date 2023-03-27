using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    public List<Transform> ChildNodeList = new();

    // Start is called before the first frame update
    void Start()
    {
        FillNodes();
    }


    //private void OnDrawGizmos() // Visualize node list by drawing lines between them
    //{
    //    Gizmos.color = Color.green;
    //    FillNodes();

    //    for (int i = 1; i < ChildNodeList.Count; i++)
    //    {
    //        var currentPosition = ChildNodeList[i].position;

    //        var previousNodePosition = ChildNodeList[i - 1].position;

    //        Gizmos.DrawLine(previousNodePosition, currentPosition);
    //    }
    //}

    private void FillNodes()
    {
        ChildNodeList.Clear();

        var childNodes = GetComponentsInChildren<Transform>();

        foreach(var child in childNodes)
        {
            if(child != transform) // Makes sure that route game object is not part of child node list
            {
                ChildNodeList.Add(child);
            }
        }
    }
}
