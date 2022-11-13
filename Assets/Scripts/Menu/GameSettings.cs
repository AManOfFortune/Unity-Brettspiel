using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public int settingsForPlayerNumber;

    public void SetPlayerTypeHuman(bool on)
    {
        if (on) SaveSettings.players[settingsForPlayerNumber] = "Human";
    }

    public void SetPlayerTypeNPC(bool on)
    {
        if (on) SaveSettings.players[settingsForPlayerNumber] = "NPC";
    }
}

public static class SaveSettings
{
    //  0    1      2     3 
    // Red Green Yellow Blue
    public static string[] players = new string[4];

    public static List<string> winners = new();
}