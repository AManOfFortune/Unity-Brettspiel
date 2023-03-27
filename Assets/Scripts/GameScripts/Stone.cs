using System;
using System.Collections;
using System.Collections.Generic;
using GameScripts.StateMachine;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stone : MonoBehaviour
{
    [NonSerialized] public Node BaseNode;
    [NonSerialized] public Node CurrentPosition = null;

    [NonSerialized] public Player Owner; // Owner knows the routes & other player information

    private float cTime; // Used in movement function math

    private GameObject Selector;

    private void Awake()
    {
        // Gets the selector and makes it invisible by default
        Selector = transform.GetChild(0).transform.gameObject;
        ShowSelector(false);
    }

    private void OnMouseDown()
    {
        // If selector is active
        if (Selector.activeSelf)
        {
            // Move the stone
            Move();
            // Deactivate all selectors
            Owner.DeactivateAllSelectors();
        }
    }

    // Sets transform and rotation to target
    // Currently only used when spawning a stone
    public void SetTransformTo(Transform target)
    {
        gameObject.transform.position = target.position;
        gameObject.transform.rotation = target.rotation;
    }

    public bool IsInBase()
    {
        return CurrentPosition == null;
    }

    public void ShowSelector(bool show)
    {
        Selector.SetActive(show);
    }

    // Checks if a regular move is possible for this stone
    public bool RegularMovePossible(int stepsToMove)
    {
        // Get all the possible end positions for the current position
        var endPositions = GetValidEndPositions(stepsToMove, CurrentPosition);

        // If there are end positions (not null and more than 0 elements), loop them
        if (endPositions is { Count: > 0 })
        {
            foreach (var node in endPositions)
            {
                // If there is a non-taken node in the list, return true
                if (!node.isTaken) return true;
            }
        }

        return false;
    }

    // Checks if this stone can move and kick another stone out
    public bool KickMovePossible(int stepsToMove)
    {
        // Get all the possible end positions for the current position
        var endPositions = GetValidEndPositions(stepsToMove, CurrentPosition);

        // If there are end positions (not null and more than 0 elements), loop them
        if (endPositions is { Count: > 0 })
        {
            foreach (var node in endPositions)
            {
                // If there is a taken node in the list, return true
                // Note: We don't need to compare owners here, since the GetValidEndPositions function does not return end positions where stones with the same owner exist
                if (node.isTaken) return true;
            }
        }

        return false;
    }

    // Moves a stone, either regularly or out of the base if it is in the base
    // Make sure that this function is only called after making sure the move is possible/valid
    public void Move()
    {
        if(IsInBase())
        {
            StartCoroutine(LeaveBase(true));
        }
        else
        {
            StartCoroutine(MoveSteps(Owner.StepsToMove));
            Owner.StepsToMove = 0;
        }
    }

    // Enumerator for regular board movement
    // Takes care of "cleaning" node, kicking stones and checking for win too
    private IEnumerator MoveSteps(int steps)
    {
        if (GameManager.StateMachine.IsLocked) yield break;

        GameManager.StateMachine.Lock();

        // Clean current node (-> Old position)
        CurrentPosition.stone = null;
        CurrentPosition.isTaken = false;

        // Move stone visually (-> Change the node transform and save new node position as our current one)
        while (steps > 0)
        {
            var nextNodeList = DetermineNextNode(steps, CurrentPosition);

            if (nextNodeList.Count > 1)
            {
                UIManager.Instance.ShowMessage("Choose Path with Q and E, confirm with SPACE");

                int chosenNodeIndex = 0;

                nextNodeList[chosenNodeIndex].ActivateSelector(true);

                while (!Input.GetKeyDown(KeyCode.Space))
                {
                    // Cycle left through the adjacent nodes
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        // Hide selector before changing node
                        nextNodeList[chosenNodeIndex].ActivateSelector(false);

                        chosenNodeIndex = Modulo(chosenNodeIndex - 1, nextNodeList.Count);

                        // Show the new selector
                        nextNodeList[chosenNodeIndex].ActivateSelector(true);
                    }

                    // Cycle right through the adjacent nodes
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Hide selector before changing node
                        nextNodeList[chosenNodeIndex].ActivateSelector(false);

                        chosenNodeIndex = Modulo(chosenNodeIndex + 1, nextNodeList.Count);

                        // Show the new selector
                        nextNodeList[chosenNodeIndex].ActivateSelector(true);
                    }

                    yield return null;
                }

                // Hide the selector of the chosen node
                nextNodeList[chosenNodeIndex].ActivateSelector(false);
                CurrentPosition = nextNodeList[chosenNodeIndex];
            }
            else
            {
                CurrentPosition = nextNodeList[0];
            }

            var nextPos = CurrentPosition.gameObject.transform;

            while (MoveInArcTo(nextPos)) { yield return null; }

            yield return new WaitForSeconds(0.1f);

            switch (CurrentPosition.gameObject.tag)
            {
                case "Slipperyfield":
                    CurrentPosition.performActions(this);
                    break;
                case "Stickyfield":
                    steps -= 1;
                    CurrentPosition.performActions(this);
                    break;
                default:
                    steps -= 1;
                    break;
            }
            cTime = 0;
        }

        // Kick stone on new position if there is one
        if(KickMovePossible(0))
        {
            // Since the current position node still has the "old"/enemy stone, we can make it return to base
            CurrentPosition.stone.LeaveBase(false);
        }

        // Set up final position by setting node values
        CurrentPosition.stone = this;
        CurrentPosition.isTaken = true;

        // Check if the player has won
        // This can only happen after moving, so that's why this is called (only) here
        Owner.CheckIfHasWon();

        if(CurrentPosition.gameObject.tag == "Actionfield")
        {
            CurrentPosition.performActions(this);
        }

        GameManager.StateMachine.ChangeState(GameManager.SwitchingPlayers);

        GameManager.StateMachine.Unlock();
    }

    // Enumerator for leaving or returning to base
    private IEnumerator LeaveBase(bool leave)
    {
        if (GameManager.StateMachine.IsLocked) yield break;

        GameManager.StateMachine.Lock();

        // Leaving the base
        if(leave)
        {
            var nextPos = Owner.StartNode.gameObject.transform;

            while (MoveInArcTo(nextPos)) { yield return null; }

            yield return new WaitForSeconds(0.1f);

            cTime = 0;

            Node newPosition = Owner.StartNode;

            // Kick stone on new position if there is one
            if (newPosition.isTaken)
            {
                newPosition.stone.LeaveBase(false);
            }

            newPosition.stone = this;
            newPosition.isTaken = true;

            CurrentPosition = newPosition;

            // Roll dice again after leaving the base
            GameManager.StateMachine.ChangeState(GameManager.RollingDice);
        }
        // Returning to base
        else
        {
            CurrentPosition = null; // Keep in mind that the only reason we return to base is when another player kicks us, so there is no need to reset current position node values!

            var endPos = BaseNode.gameObject.transform;

            while (MoveInArcTo(endPos)) { yield return null; }
        }

        // Make sure leaving the base resets the steps to 0 (previously whatever needed to be rolled to leave the base)
        Owner.StepsToMove = 0;

        GameManager.StateMachine.Unlock();
    }

    // Used to determine end nodes after performing the given steps
    // Essentially recursively traverses the tree and returns the final, valid positions the player can land on
    [CanBeNull]
    private List<Node> GetValidEndPositions(int steps, Node currentPos)
    {
        List<Node> endPointList = new List<Node>();

        // Do actions for the node type
        switch (currentPos.gameObject.tag)
        {
            case "Slipperyfield":
                currentPos.performActions(this);
                break;
            case "Stickyfield":
                steps--;
                currentPos.performActions(this);
                break;
            default:
                steps--;
                break;
        }

        // Get a list of the next nodes
        var nextNodes = GetNextNodesList(currentPos);

        // If nextNodes is empty and we have more steps to go, it means we have overshot the end of the route and the path is invalid
        if (nextNodes.Count == 0 && steps != 0)
        {
            return null;
        }

        // Only continue the recursion as long as we still have steps to go
        if (steps > 0)
        {
            // If only a single next node is found, continue the tree traversal normally
            if (nextNodes.Count == 1)
            {
                return GetValidEndPositions(steps, nextNodes[0]);
            }

            // If next nodes has more than 1 entry, it means there are multiple paths and we loop over all of them
            if (nextNodes.Count > 1)
            {
                foreach (var node in nextNodes)
                {
                    var pathEndPoints = GetValidEndPositions(steps, node);

                    // If the path endpoint is null, it means it's an invalid path and should be ignored as an endpoint
                    if(pathEndPoints != null)
                        endPointList.AddRange(pathEndPoints);
                }
            }
        }
        // If we have reached the end of our steps (= we reached the end of this path), check if one of our stones is at that position
        else
        {
            // Return null if the end position is taken by one of our stones
            if (currentPos.isTaken && currentPos.stone.Owner == Owner)
                return null;

            return new List<Node> { currentPos };
        }

        // Finally, return the endPointList
        return endPointList;
    }

    // Removes invalid paths from the paths list, returns the adjusted paths list
    private List<Node> GetValidPaths(int steps, List<Node> paths)
    {
        var validPaths = new List<Node>();

        foreach (var pathStartNode in paths)
        {
            // Determine if endpoints exist on the current path
            var possibleEndpoints = GetValidEndPositions(steps, pathStartNode);

            // If possible endpoints is not null and has elements, the current path is a valid path
            if (possibleEndpoints is { Count: > 0 })
            {
                validPaths.Add(pathStartNode);
            }
        }

        return validPaths;
    }

    // Determines the next node to a given position, either automatically or by choosing a path
    // Also makes sure the path options are all valid (e.g. at the end of the chosen path won't be a stone of the same player, ...)
    // Returns a list that is of length 1. If the list is longer, it means the human player is required to select a node
    private List<Node> DetermineNextNode(int steps, Node currentPos)
    {
        // Get adjacent node list
        // Should always have at least 1 entry since this function should never get called on the final goal position (which is the only node without an adjacent one)
        var adjacentNodes = GetNextNodesList(currentPos);

        int chosenNodeIndex = 0;

        // Check if our current position has more than one adjacent node
        if (adjacentNodes.Count > 1)
        {
            // If we do have multiple paths, we need to make sure to only include those in our choice that are valid (="walkable")
            // Note: We already determined before enabling movement that at least one valid path exists, so there should never be the case that this list is empty
            adjacentNodes = GetValidPaths(steps, adjacentNodes);

            // Make sure the adjacent nodes adjusted by validity of paths are still multiple, otherwise movement can happen without player choice (since he would only choose from 1 option anyway)
            if (adjacentNodes.Count > 1)
            {
                // If the player is a human, the next node is chosen by player
                if (Owner.Type == Player.PlayerTypes.HUMAN)
                {
                    return adjacentNodes;
                }
                // Otherwise the next node is chosen at random
                // TODO: Make the AI more intelligent by choosing the most optimal path
                else if (Owner.Type == Player.PlayerTypes.NPC)
                {
                    chosenNodeIndex = Random.Range(0, adjacentNodes.Count);
                }
            }
        }

        return new List<Node> { adjacentNodes[chosenNodeIndex] };
    }

    private List<Node> GetNextNodesList(Node currentPos)
    {
        // If the current position is the last position of the owner, return the first "goal route" node as the next one
        if (currentPos == Owner.EndNode)
        {
            return new List<Node> { Owner.GoalRoute.ChildNodeList[0].GetComponent<Node>() };
        }

        return currentPos.AdjacentNodes;
    }

    // Helper function to move & rotate to new position (position & rotation)
    // BUG: Rotation Lerp is often slower than position lerp, resulting in inconsistent movement speed
    private bool MoveInArcTo(Transform goalPos)
    {
        // Increase cTime
        cTime += GameManager.Instance.MovementSpeed * Time.deltaTime;

        // New horizontal position
        var myPosition = Vector3.Lerp(transform.position, goalPos.position, cTime);

        // Increase new position height with mafs that I don't understand but that work :)
        myPosition.y += GameManager.Instance.MovementArcHeight * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);

        // Rotate towards goal rotation using more dank mafs :)
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(goalPos.eulerAngles), Time.deltaTime * GameManager.Instance.MovementSpeed);

        // Move towards new position
        transform.position = Vector3.Lerp(transform.position, myPosition, cTime);

        // Returns false as long as movement and rotation is not complete
        return (goalPos.position != transform.position || transform.rotation != goalPos.rotation);
    }

    /// <summary>
    /// Own modulo function for modulo operations on negative integers.
    /// </summary>
    /// <param name="x">The dividend of the modulo operation.</param>
    /// <param name="m">The divisor of the modulo operation.</param>
    /// <returns></returns>
    private int Modulo(int x, int m)
    {
        return (x % m + m) % m;
    }
}
