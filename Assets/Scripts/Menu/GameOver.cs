using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMP_Text first, second, third;

    // Start is called before the first frame update
    void Start()
    {
        first.text = "1st: " + SaveSettings.winners[0];
        second.text = "2nd: " + SaveSettings.winners[1];
        third.text = "3rd: " + SaveSettings.winners[2];
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
