using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject RollDiceButton;
    [SerializeField] private TMP_Text InfoBox;

    private void Awake()
    {
        Instance = this;
    }

    // Gets called by player clicking checkbox
    public void SetPlayerTypeHuman(string playerName)
    {
        GameSettings.PlayerNamesAndTypes[playerName] = Player.PlayerTypes.HUMAN;
    }

    // Same as above
    public void SetPlayerTypeNPC(string playerName)
    {
        GameSettings.PlayerNamesAndTypes[playerName] = Player.PlayerTypes.NPC;
    }

    // Gets called by player clicking start button
    public void LoadSceneCalled(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RollDiceButtonVisible(bool visible)
    {
        RollDiceButton.SetActive(visible);
    }

    public void ShowMessage(string text)
    {
        InfoBox.text = text;
    }
}
