using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < SaveSettings.players.Length; i++) // Make sure everything gets initialized
        {
            SaveSettings.players[i] = "NPC";
        }
    }

    public void StartTheGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
