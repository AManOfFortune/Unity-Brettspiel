using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [NonSerialized] public static StateMachine Instance;
    public enum States
    {
        WAITING,
        ROLL_DICE,
        SWITCH_PLAYER
    }

    [NonSerialized] public States State = States.WAITING;

    // Something could either be someone moving or a player switching
    // Essentially a safeguard that prevents Update() from accidentally calling the same state function multiple times
    [NonSerialized] public bool SomethingIsHappening = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!SomethingIsHappening)
        {
            switch (State)
            {
                case States.ROLL_DICE:

                    if (GameManager.Instance.GetActivePlayer().Type == Player.PlayerTypes.NPC)
                    {
                        GameManager.Instance.RollDice(2); // Roll dice with a 2 second delay
                    }

                    else if (GameManager.Instance.GetActivePlayer().Type == Player.PlayerTypes.HUMAN)
                    {
                        UIManager.Instance.RollDiceButtonVisible(true); // Show Roll Dice Button
                    }

                    State = States.WAITING;
                    break;

                case States.SWITCH_PLAYER:
                    GameManager.Instance.SwitchPlayer(2); // Switch player after a 2 second delay
                    State = States.WAITING;
                    break;

                case States.WAITING: // Idle, waits for next input
                    break;
            }
        }
    }
}
