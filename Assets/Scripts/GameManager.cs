using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [System.Serializable]
    public class Player
    {
        public string name;
        public List<Stone> stones;
        public bool hasTurn;

        public enum PlayerTypes
        {
            HUMAN,
            NPC,
            NO_PLAYER
        }

        public PlayerTypes type;

        public bool hasWon;
    }

    public List<Player> playerList = new();

    // Statemachine
    public enum States
    {
        WAITING,
        ROLL_DICE,
        SWITCH_PLAYER
    }

    public States state;

    public int activePlayer; // Index of active player in playerList
    private bool switchingPlayer;
    private bool turnPossible = true;

    // Human Inputs
    // Gameobject for button
    public GameObject rollDiceButton;
    [HideInInspector] public int rolledHumanDice;

    public Dice dice;

    private void Awake()
    {
        instance = this;

        for(int i = 0; i < playerList.Count; i++)
        {
            if (SaveSettings.players[i] == "Human") playerList[i].type = Player.PlayerTypes.HUMAN;
            else if (SaveSettings.players[i] == "NPC") playerList[i].type = Player.PlayerTypes.NPC;
        }
    }

    private void Start()
    {
        ActivateButton(false);

        activePlayer = Random.Range(0, playerList.Count);

        Info.Instance.ShowMessage(playerList[activePlayer].name + " starts first!");
    }

    private void Update()
    {
        if (playerList[activePlayer].type == Player.PlayerTypes.NPC)
        {
            switch (state)
            {
                case States.ROLL_DICE:
                    if (turnPossible)
                    {
                        StartCoroutine(RollDiceDelay());
                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    if (turnPossible)
                    {
                        StartCoroutine(SwitchPlayer());
                        state = States.WAITING;
                    }
                    break;
                case States.WAITING: // Idle, waits for next input
                    break;
            }
        }

        else if (playerList[activePlayer].type == Player.PlayerTypes.HUMAN)
        {
            switch (state)
            {
                case States.ROLL_DICE:
                    if (turnPossible)
                    {
                        // Deactivate highlights
                        ActivateButton(true);
                        state = States.WAITING;
                    }
                    break;
                case States.SWITCH_PLAYER:
                    if (turnPossible)
                    {
                        // Deactivate button
                        // Deactivate highlights
                        StartCoroutine(SwitchPlayer());
                        state = States.WAITING;
                    }
                    break;
                case States.WAITING: // Idle, waits for next input
                    break;
            }
        }
    }

    private void NPCDice()
    {
        dice.RollDice();
    }

    public void RollDice(int rolledDice) // Called from Dice
    {
        if (playerList[activePlayer].type == Player.PlayerTypes.NPC)
        {
            if (rolledDice == 6)
            {
                // check the start node
                CheckStartNode(rolledDice);
            }
            else
            {
                // check for move
                MoveAStone(rolledDice);
            }
        }

        else if (playerList[activePlayer].type == Player.PlayerTypes.HUMAN)
        {
            rolledHumanDice = rolledDice;
            HumanDiceRoll();
        }

        Info.Instance.ShowMessage(playerList[activePlayer].name + " has rolled " + rolledDice);
    }

    private IEnumerator RollDiceDelay()
    {
        yield return new WaitForSeconds(2);
        NPCDice();
    }

    private void CheckStartNode(int rolledDiceNumber)
    {
        // Is anyone on the start node?
        bool startNodeFull = false;

        //Loop through all the active players stones and check if one of his stones is on the start position
        foreach (var stone in playerList[activePlayer].stones)
        {
            if(stone.currentNode == stone.StartNode)
            {
                startNodeFull = true;
                break;
            }
        }

        if(startNodeFull) // Move a stone since start node is full
        {
            MoveAStone(rolledDiceNumber);
        }
        else // Move a stone out of the base if at least one is inside base
        {
            // Loop stones and check if one is inside base
            foreach (var stone in playerList[activePlayer].stones)
            {
                // If a stone is not out, leave the base
                if (!stone.ReturnIsOut())
                {
                    stone.LeaveBase();
                    state = States.WAITING;
                    return;
                }
            }

            MoveAStone(rolledDiceNumber);
        }
    }

    private void MoveAStone(int rolledDice)
    {
        List<Stone> moveableStones = new();
        List<Stone> moveKickStones = new();

        // Fill lists
        foreach(var stone in playerList[activePlayer].stones)
        {
            if(stone.ReturnIsOut())
            {
                // Check for possible kick
                if(stone.CheckPossibleKick(stone.stoneID, rolledDice))
                {
                    moveKickStones.Add(stone);
                    continue;
                }

                // Check for possible move
                if(stone.CheckPossibleMove(rolledDice))
                {
                    moveableStones.Add(stone);
                }
            }
        }

        // Perform kick if possible
        if(moveKickStones.Count > 0)
        {
            int randomIndex = Random.Range(0, moveKickStones.Count);

            moveKickStones[randomIndex].StartMove(rolledDice);
            state = States.WAITING;
            return;
        }

        // Perform move if possible
        if (moveableStones.Count > 0)
        {
            int randomIndex = Random.Range(0, moveableStones.Count);

            moveableStones[randomIndex].StartMove(rolledDice);
            state = States.WAITING;
            return;
        }

        // If no possible move, switch player
        state = States.SWITCH_PLAYER;
    }

    private IEnumerator SwitchPlayer()
    {
        if(switchingPlayer) { yield break; }

        switchingPlayer = true;

        yield return new WaitForSeconds(2);

        // Set next active player
        SetNextActivePlayer();

        switchingPlayer = false;
    }

    private void SetNextActivePlayer()
    {
        activePlayer++;
        activePlayer %= playerList.Count;

        // Makes sure that at least 2 players are still playing
        int available = 0;
        foreach(var player in playerList)
        {
            if (!player.hasWon) available++;
        }

        if (playerList[activePlayer].hasWon && available > 1)
        {
            SetNextActivePlayer();
            return;
        }
        else if (available < 2)
        {
            // Game over screen
            SceneManager.LoadScene("GameOver");
            state = States.WAITING;
            return;
        }

        Info.Instance.ShowMessage(playerList[activePlayer].name + "'s turn!");

        state = States.ROLL_DICE;
    }

    public void ReportTurnPossible(bool possible)
    {
        turnPossible = possible;
    }

    public void ReportWinning()
    {
        // TODO: Show some UI
        playerList[activePlayer].hasWon = true;
        // Save winner
        SaveSettings.winners.Add(playerList[activePlayer].name);
    }

    // ------------------------------------------------- Human Input ---------------------------------------------------
    #region human input

    private void ActivateButton(bool on)
    {
        rollDiceButton.SetActive(on);
    }

    public void DeactivateSelectors()
    {
        foreach(var player in playerList)
        {
            foreach(var stone in player.stones)
            {
                stone.SetSelector(false);
            }
        }
    }

    // Gets called when roll-dice button is clicked
    public void HumanRoll()
    {
        ActivateButton(false);
        dice.RollDice();
    }

    public void HumanDiceRoll()
    {
        // Moveable stone list
        var moveableStones = PossibleStones(rolledHumanDice);

        // Start node full check
        bool startNodeFull = false;

        //Loop through all the active players stones and check if one of his stones is on the start position
        foreach (var stone in playerList[activePlayer].stones)
        {
            if (stone.currentNode == stone.StartNode)
            {
                startNodeFull = true;
                break;
            }
        }

        // Number == 6 && start node is not full, add stones inside base to moveable stone list too
        if(rolledHumanDice == 6 && !startNodeFull)
        {
            foreach (var stone in playerList[activePlayer].stones)
            {
                if (!stone.ReturnIsOut())
                {
                    moveableStones.Add(stone);
                }
            }
        }

        // Activate all possible selectors
        foreach(var stone in moveableStones)
        {
            stone.SetSelector(true);
        }

        // If no moves are possible, switch player
        if(moveableStones.Count == 0)
        {
            state = States.SWITCH_PLAYER;
        }
    }

    private List<Stone> PossibleStones(int rolledDice)
    {
        List<Stone> tempList = new();

        foreach (var stone in playerList[activePlayer].stones)
        {
            if (stone.ReturnIsOut())
            {
                // Check for possible kick
                if (stone.CheckPossibleKick(stone.stoneID, rolledDice))
                {
                    tempList.Add(stone);
                    continue;
                }

                // Check for possible move
                if (stone.CheckPossibleMove(rolledDice))
                {
                    tempList.Add(stone);
                }
            }
        }

        return tempList;
    }

    #endregion
}
