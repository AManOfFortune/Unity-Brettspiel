using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string Name;
    [SerializeField] private GameObject StonePrefab;

    public PlayerTypes Type = PlayerTypes.NPC; // Every player is an NPC by default

    [SerializeField] private Base PlayerBase;
    public Node StartNode; // Node Player starts on when leaving the base
    public Route GoalRoute; // Final Route where only this player can walk on

    [NonSerialized] public List<Node> FullRoute; // Full route from start to finish
    [NonSerialized] public int StepsToMove; // Amount of steps the player can move; When a stone moves, the stone itself decreases this value
    [NonSerialized] public bool HasWon = false; // If all stones are in base gets set to true

    private List<Stone> Stones = new(); // List of player stones

    public enum PlayerTypes
    {
        HUMAN,
        NPC,
        NO_PLAYER
    }

    // Spawns stones on base
    public void SpawnStones()
    {
        foreach (Node baseNode in PlayerBase.BaseNodes)
        {
            GameObject newStone = Instantiate(StonePrefab); // Instaniate new player stone

            newStone.transform.parent = transform; // Put player stone into player wrapper (the object this script is on)

            Stone newStoneScript = newStone.GetComponent<Stone>();

            newStoneScript.SetTransformTo(baseNode.transform); // Sets position of new stone to base position
            
            newStoneScript.BaseNode = baseNode;
            newStoneScript.Owner = this;

            Stones.Add(newStoneScript); // Adds new stone to list
        }
    }

    // Creates and fills full route List
    public void CreateFullRoute()
    {
        FullRoute = new();

        // Gets index of start node in common route
        int startNodeIndex = GameManager.Instance.CommonRoute.ChildNodeList.IndexOf(StartNode.transform);

        // Adds common route to full route, starts at starting postion
        for (int i = 0; i < GameManager.Instance.CommonRoute.ChildNodeList.Count; i++)
        {
            int tempPos = startNodeIndex + i;
            tempPos %= GameManager.Instance.CommonRoute.ChildNodeList.Count;

            FullRoute.Add(GameManager.Instance.CommonRoute.ChildNodeList[tempPos].GetComponent<Node>());
        }

        // Adds goal route at the end of full route
        for (int i = 0; i < GoalRoute.ChildNodeList.Count; i++)
        {
            FullRoute.Add(GoalRoute.ChildNodeList[i].GetComponent<Node>());
        }
    }

    public void Move(int steps, bool canLeaveBase)
    {
        // Saves steps we can move
        // Gets decreased, reset etc. in the Stone.Move() function
        StepsToMove = steps;

        // If player is human, show selectors
        // Clicking on a stone with an active selector then results in the movement function of that stone being triggered
        if(Type == PlayerTypes.HUMAN)
        {
            var moveableStones = GetAllMoveableStones(steps, canLeaveBase);

            // If no moveable stone is found, switch player
            if(moveableStones.Count == 0)
            {
                StateMachine.Instance.State = StateMachine.States.SWITCH_PLAYER;
                return;
            }

            foreach(Stone stone in moveableStones)
            {
                stone.ShowSelector(true);
            }
        }
        // If player is NPC, find "best" stone to move and move it
        else if (Type == PlayerTypes.NPC)
        {
            var stoneToMove = GetBestStoneToMove(steps, canLeaveBase);

            // If no moveable stone is found, switch player
            if (stoneToMove == null) 
            { 
                StateMachine.Instance.State = StateMachine.States.SWITCH_PLAYER;
                return;
            }

            stoneToMove.Move();
        }
    }

    public void CheckIfHasWon()
    {
        foreach (var nodeTransform in GoalRoute.ChildNodeList)
        {
            if (!nodeTransform.GetComponent<Node>().isTaken)
            {
                return;
            }
        }

        HasWon = true; // Save hasWon to make later access faster

        GameSettings.WinnerNames.Add(Name); // Save Winner to settings
    }

    // Deactivates all selectors
    public void DeactivateAllSelectors()
    {
        foreach(Stone stone in Stones)
        {
            stone.ShowSelector(false);
        }
    }

    // Returns a list of all stones a player can move, including stones in the base
    private List<Stone> GetAllMoveableStones(int steps, bool canLeaveBase)
    {
        List<Stone> moveableStones = new();

        if(canLeaveBase)
        {
            moveableStones.AddRange(GetMoveableStonesInBase());
        }

        moveableStones.AddRange(GetRegularMoveableStones(steps));

        return moveableStones;
    }

    // Returns a single stone that the "AI" deems best
    // If you want to influence NPC behaviour, this is the place to do it
    private Stone GetBestStoneToMove(int steps, bool canLeaveBase)
    {
        List<Stone> moveableStones = new();

        // If we are allowed to move a stone out of the base, that is what we try to do
        if(canLeaveBase)
        {
            moveableStones = GetMoveableStonesInBase();
        }
        
        // If we cannot leave the base, or no stone that can leave the base was found, look for stones that are already out
        if(!canLeaveBase || moveableStones.Count == 0)
        {
            foreach (Stone stone in Stones)
            {
                if (!stone.IsInBase())
                {
                    // If we find a stone that can kick another stone, basically return it as the best stone
                    if (stone.KickMovePossible(steps))
                    {
                        moveableStones.Clear();
                        moveableStones.Add(stone);
                        break;
                    }
                    // If a kick is not possible, check if a regular move is possible
                    else if (stone.RegularMovePossible(steps))
                    {
                        moveableStones.Add(stone);
                    }
                }
            }
        }

        // If no stone that can move at all was found, return null
        if(moveableStones.Count == 0) return null;

        // Otherwise return a random stone from the moveableStones list
        return moveableStones[UnityEngine.Random.Range(0, moveableStones.Count)];
    }

    private List<Stone> GetMoveableStonesInBase()
    {
        List<Stone> moveableStones = new();

        foreach (Stone stone in Stones)
        {
            // If a friendly stone is on the start node, no stones can be moved from the base so return empty list
            if (stone.CurrentPosition == StartNode)
            {
                moveableStones.Clear();
                break;
            }

            if (stone.IsInBase())
            {

            }
                moveableStones.Add(stone);
        }

        return moveableStones;
    }

    private List<Stone> GetRegularMoveableStones(int steps)
    {
        List<Stone> moveableStones = new();

        foreach(Stone stone in Stones)
        {
            // If stone is not in base, and stone can move (regular move possible or a kick move possible), add it to list
            if(!stone.IsInBase() && (stone.RegularMovePossible(steps) || stone.KickMovePossible(steps)))
            {
                moveableStones.Add(stone);
            }
        }

        return moveableStones;
    }
}
