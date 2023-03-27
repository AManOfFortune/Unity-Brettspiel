using System;
using System.Collections;
using System.Collections.Generic;
using GameScripts.StateMachine;
using GameScripts.StateMachine.State;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(IStateMachine))]
public class GameManager : MonoBehaviour
{
    [NonSerialized] public static GameManager Instance;
    
    public static IStateMachine StateMachine {get; private set; }

    public static IState Waiting { get; private set; }
    public static IState RollingDice { get; private set; }
    public static IState SwitchingPlayers { get; private set; }

    public float MovementSpeed = 5f;
    public float MovementArcHeight = 0.5f;

    public Route CommonRoute;
    public GameObject NodeSelector;
    [SerializeField] private Dice GameDice;
    [SerializeField] private CameraController CameraController;

    [SerializeField] private List<Player> Players = new();
    private int ActivePlayer; // Index of currently playing player in player list

    private void Awake()
    {
        // Fills player list
        Players = new(FindObjectsOfType<Player>());

        Instance = this;
        
        // Get the state machine component
        StateMachine = GetComponent<IStateMachine>();
        // Get the state components
        Waiting = GetComponentInChildren<Waiting>();
        RollingDice = GetComponentInChildren<RollingDice>();
        SwitchingPlayers = GetComponentInChildren<SwitchingPlayers>();
        // Initialize the state machine with state Waiting
        StateMachine.InitState(Waiting);
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {   
        InitializePlayers(); // Initializes the players

        InitializeRoutes(); // Initializes the common route

        UIManager.Instance.RollDiceButtonVisible(false); // Makes sure the roll dice button is hidden

        ActivePlayer = /*UnityEngine.Random.Range(0, Players.Count)*/3; // Randomize which player starts

        UIManager.Instance.ShowMessage(Players[ActivePlayer].Name + " starts first!");

        StateMachine.ChangeState(RollingDice);
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
            if(GetActivePlayer().Type == Player.PlayerTypes.HUMAN) CameraController.MoveToDiceArena(); // Zoom camera to dice arena if player is human

            GameDice.RollDice();
        }));
    }

    public void SwitchPlayer(int delayBySeconds = 0)
    {
        if (StateMachine.IsLocked) return;

        StateMachine.Lock();

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

            StateMachine.ChangeState(RollingDice);
        }));

        StateMachine.Unlock();
    }

    // Once physical dice roll is done, this function gets called with the results
    public void ReportDiceRollResults(int diceRollResult)
    {
        if (GetActivePlayer().Type == Player.PlayerTypes.HUMAN) CameraController.ResetPosition(); // Move zoomed camera back if player is human

        UIManager.Instance.ShowMessage(GetActivePlayer().Name + " has rolled " + diceRollResult);

        // Moves after a 2 second delay to give the camera time to move to its original position
        StartCoroutine(DelayBySeconds(2, () =>
        {
            // Move with the result and the condition for leaving the base is called
            GetActivePlayer().ChooseMove(diceRollResult, diceRollResult > 0);
        }));
    }

    private void InitializePlayers()
    {
        foreach(Player player in Players)
        {
            player.Type = GameSettings.PlayerNamesAndTypes.ContainsKey(player.Name) ? GameSettings.PlayerNamesAndTypes[player.Name] : Player.PlayerTypes.NPC; // Sets the player type to what was set in main menu, or to default NPC
            player.SpawnStones(); // Spawns all player stones on their respective base
            player.InitializeGoalRoute(); // Creates adjacency list for goal route

            if (player.Name == "Yellow") player.Type = Player.PlayerTypes.HUMAN;
        }
    }

    private void InitializeRoutes()
    {
        // Loop common route entries
        for (int i = 0; i < CommonRoute.ChildNodeList.Count; i++)
        {
            var node = CommonRoute.ChildNodeList[i].GetComponent<Node>();

            var nextNode = 
            i != CommonRoute.ChildNodeList.Count - 1 ? // If the loop reaches the last entry, the last node gets joined to the first one so we create a loop
            CommonRoute.ChildNodeList[i + 1].GetComponent<Node>() : 
            CommonRoute.ChildNodeList[0].GetComponent<Node>();

            node.AdjacentNodes.Add(nextNode);
        }

        /// SCUFFED SELECTOR CODE HERE
        /// FUCKING REMOVE THAT SHIT ASAP

        var allNodes = FindObjectsOfType<Node>();
        foreach (var node in allNodes)
        {
            var selector = Instantiate(NodeSelector, node.transform);
            node.selector = selector;
            node.ActivateSelector(false);
        }

        /// END OF SCUFFED SELECTOR CODE

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
            StateMachine.ChangeState(Waiting);
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
