using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isTaken;

    public Stone stone;

    private void Start()
    {
        if(this.gameObject.tag == "InvisNode")
        {
            this.gameObject.transform.localScale = new Vector3(0.5f, 1, 0.5f);
        }
    }
}
