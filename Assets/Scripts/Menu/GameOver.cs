using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMP_Text first, second, third;

    // Start is called before the first frame update
    void Start()
    {
        first.text = "1st: " + GameSettings.WinnerNames[0];
        second.text = "2nd: " + GameSettings.WinnerNames[1];
        third.text = "3rd: " + GameSettings.WinnerNames[2];
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
