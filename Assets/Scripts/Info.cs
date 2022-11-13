using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Info : MonoBehaviour
{
    public static Info Instance;

    public TMP_Text infoText;

    private void Awake()
    {
        Instance = this;
        infoText.text = "";
    }

    public void ShowMessage(string text)
    {
        infoText.text = text;
    }
}
