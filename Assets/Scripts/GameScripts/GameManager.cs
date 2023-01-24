using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [NonSerialized] public static GameManager Instance;

    public float MovementSpeed = 5f;
    public float MovementArcHeight = 0.5f;

    public Route CommonRoute;
    [SerializeField] private Dice GameDice;
    [SerializeField] private ActionCameraController CameraController;

    [SerializeField] private List<Player> Players;
    private int ActivePlayer; // Index of currently playing player in player list

    public GameObject DiceArena;

    private void Awake()
    {
        // Fills player list
        Players = new List<Player>(FindObjectsOfType<Player>());

        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {   
        InitializePlayers(); // Initializes the players

        UIManager.Instance.RollDiceButtonVisible(false); // Makes sure the roll dice button is hidden

        ActivePlayer = UnityEngine.Random.Range(0, Players.Count); // Randomize which player starts

        UIManager.Instance.ShowMessage(Players[ActivePlayer].Name + " starts first!");

        StateMachine.Instance.State = StateMachine.States.ROLL_DICE; // Change state to ROLL_DICE
    }

    public Player GetActivePlayer()
    {
        return Players[ActivePlayer];
    }

    // Starts the physical dice roll
    public void RollDice(int delayBySeconds = 0)
    {
        StartCoroutine(DelayBySeconds(delayBySeconds, () =>
        {
            //if(GetActivePlayer().Type == Player.PlayerTypes.HUMAN) CameraController.LookAt(new Vector3(-7,0,50)); // Zoom camera to dice arena if player is human
            CameraController.LookAt(GameDice.gameObject);
            GameDice.RollDice();
        }));
    }

    public void SwitchPlayer(int delayBySeconds = 0)
    {
        if (StateMachine.Instance.SomethingIsHappening) return;

        StateMachine.Instance.SomethingIsHappening = true;

        StartCoroutine(DelayBySeconds(delayBySeconds, () =>
        {
            ActivePlayer++;
            ActivePlayer %= Players.Count;

            // If game is over, return (function automatically switches scene)
            if (CheckIsGameover()) return;

            // If current active player has already won, go to the next player (call function again)
            if (GetActivePlayer().HasWon)
            {
                SwitchPlayer(0);
                return;
            }
            
            // If current player is playing and game is not over, show message and change state to ROLL_DICE
            UIManager.Instance.ShowMessage(GetActivePlayer().Name + "'s turn!");

            StateMachine.Instance.State = StateMachine.States.ROLL_DICE;
        }));

        StateMachine.Instance.SomethingIsHappening = false;
    }

    // Once physical dice roll is done, this function gets called with the results
    public void ReportDiceRollResults(int diceRollResult)
    {
        //if (GetActivePlayer().Type == Player.PlayerTypes.HUMAN) CameraController.ResetPosition(); // Move zoomed camera back if player is human

        UIManager.Instance.ShowMessage(GetActivePlayer().Name + " has rolled " + diceRollResult);

        // Moves after a 2 second delay to give the camera time to move to its original position
        StartCoroutine(DelayBySeconds(2, () =>
        {
            // Move with the result and the condition for leaving the base is called
            GetActivePlayer().Move(diceRollResult, diceRollResult > 0);
        }));
    }

    private void InitializePlayers()
    {
        foreach(Player player in Players)
        {
            player.Type = GameSettings.PlayerNamesAndTypes.ContainsKey(player.Name) ? GameSettings.PlayerNamesAndTypes[player.Name] : Player.PlayerTypes.NPC; // Sets the player type to what was set in main menu, or to default NPC
            player.SpawnStones(); // Spawns all player stones on their respective base
            player.CreateFullRoute(); // Creates the full route list
        }
    }

    private bool CheckIsGameover()
    {
        // Loop all players and count how many are still in the game
        int available = 0;
        foreach (var player in Players)
        {
            if (!player.HasWon) available++;
        }

        // If 1 or less players are still playing, load the game over screen
        if(available <= 1)
        {
            // Game over screen
            SceneManager.LoadScene("GameOver");
            StateMachine.Instance.State = StateMachine.States.WAITING;
            return true;
        }

        return false;
    }

    // Helper function to easily delay function execution outside of an IEnumerator
    private IEnumerator DelayBySeconds(int delay, Action doThisAfterDelay)
    {
        yield return new WaitForSeconds(delay);

        doThisAfterDelay();
    }
}
