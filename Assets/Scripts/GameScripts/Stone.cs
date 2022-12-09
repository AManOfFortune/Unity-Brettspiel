using System;
using System.Collections;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [NonSerialized] public Node BaseNode;
    [NonSerialized] public Node CurrentPosition = null;

    [NonSerialized] public Player Owner; // Owner knows the routes & other player information

    private int RoutePosition = 0; // Index position of stone in full route

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
        int tempPos = RoutePosition + stepsToMove;

        if (tempPos >= Owner.FullRoute.Count) return false; // If new route position would be over the end of the route, return false

        return !Owner.FullRoute[tempPos].isTaken;
    }

    // Checks if this stone can move and kick another stone out
    public bool KickMovePossible(int stepsToMove)
    {
        int tempPos = RoutePosition + stepsToMove;

        if (tempPos >= Owner.FullRoute.Count) return false;

        // If the position is taken, check if stone belongs to the same player
        if (Owner.FullRoute[tempPos].isTaken)
        {
            if (Owner == Owner.FullRoute[tempPos].stone.Owner)
            {
                return false;
            }

            return true;
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

    public void MoveStone(int steps)
    {
        StartCoroutine(MoveSteps(steps));
    }

    // Enumerator for regular board movement
    // Takes care of "cleaning" node, kicking stones and checking for win too
    private IEnumerator MoveSteps(int steps)
    {
        if (StateMachine.Instance.SomethingIsHappening) yield break;

        StateMachine.Instance.SomethingIsHappening = true;
        int forward = 1;
        if (steps < 0) forward = -1;
        // Move stone visually
        while (steps * forward > 0)
        {
            RoutePosition += forward;

            Node nextNode = Owner.FullRoute[RoutePosition];
            var nextPos = nextNode.gameObject.transform;

            while (MoveInArcTo(nextPos)) { yield return null; }

            yield return new WaitForSeconds(0.1f);

            switch (nextNode.gameObject.tag)
            {
                case "Slipperyfield":
                    nextNode.performActions(this);
                    break;
                case "Stickyfield":
                    steps -= forward;
                    nextNode.performActions(this);
                    break;
                default:
                    steps -= forward;
                    break;
            }
            cTime = 0;
        }

        Node newPosition = Owner.FullRoute[RoutePosition];

        // Kick stone on new position if there is one
        if(KickMovePossible(0))
        {
            newPosition.stone.LeaveBase(false);
        }

        // Clean current node
        CurrentPosition.stone = null;
        CurrentPosition.isTaken = false;

        // Set up next node
        newPosition.stone = this;
        newPosition.isTaken = true;

        // Set current position to new position
        CurrentPosition = newPosition;

        // Check if the player has won
        // This can only happen after moving, so that's why this is called (only) here
        Owner.CheckIfHasWon();

        if(CurrentPosition.gameObject.tag == "Actionfield")
        {
            CurrentPosition.performActions(this);
        }

        StateMachine.Instance.State = StateMachine.States.SWITCH_PLAYER;

        StateMachine.Instance.SomethingIsHappening = false;
    }

    // Enumerator for leaving or returning to base
    private IEnumerator LeaveBase(bool leave)
    {
        if (StateMachine.Instance.SomethingIsHappening) yield break;

        StateMachine.Instance.SomethingIsHappening = true;

        // Leaving the base
        if(leave)
        {
            var nextPos = Owner.StartNode.gameObject.transform;

            while (MoveInArcTo(nextPos)) { yield return null; }

            yield return new WaitForSeconds(0.1f);

            cTime = 0;

            Node newPosition = Owner.FullRoute[RoutePosition]; // Should be the same as StartNode

            // Kick stone on new position if there is one
            if (KickMovePossible(0))
            {
                newPosition.stone.LeaveBase(false);
            }

            newPosition.stone = this;
            newPosition.isTaken = true;

            CurrentPosition = newPosition;

            // Roll dice again after leaving the base
            StateMachine.Instance.State = StateMachine.States.ROLL_DICE;
        }
        // Returning to base
        else
        {
            RoutePosition = 0;
            CurrentPosition = null; // Keep in mind that the only reason we return to base is when another player kicks us, so there is no need to reset current position node values!

            var endPos = BaseNode.gameObject.transform;

            while (MoveInArcTo(endPos)) { yield return null; }
        }

        // Make sure leaving the base resets the steps to 0 (previously whatever needed to be rolled to leave the base)
        Owner.StepsToMove = 0;

        StateMachine.Instance.SomethingIsHappening = false;
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
}
