using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int stoneID;

    [Header("Routes")]
    public Route CommonRoute; // Outer route
    public Route InnerRoute;

    [SerializeField] private List<Node> fullRoute = new();

    [Header("Nodes")]
    public Node StartNode; // Node that player starts the route on

    public Node baseNode; // Node in home base
    public Node currentNode;
    public Node goalNode;

    private int routePosition; // Index in full Route of current position
    private int startNodeIndex; // Index in the common route of the starting node

    private int steps; // Rolled dice amount (number of steps we want to walk)
    private int doneSteps = 0; // Steps completed

    [Header("Bools")]
    public bool isOut;
    private bool isMoving = false;

    private bool hasTurn; // Is for human input

    [Header("Selector")]
    public GameObject selector;

    // Arc movement
    private float amplitude = 0.5f;
    private float cTime = 0;


    private void Start()
    {
        startNodeIndex = CommonRoute.GetIndexOfPosition(StartNode.gameObject.transform);

        CreateFullRoute();

        SetSelector(false);
    }

    // Fills full route list
    private void CreateFullRoute()
    {
        // Adds common route to full route, starts at starting postion
        for (int i = 0; i < CommonRoute.childNodeList.Count; i++)
        {
            int tempPos = startNodeIndex + i;
            tempPos %= CommonRoute.childNodeList.Count;

            fullRoute.Add(CommonRoute.childNodeList[tempPos].GetComponent<Node>());
        }

        // Adds inner route at the end of full route
        for (int i = 0; i < InnerRoute.childNodeList.Count; i++)
        {
            fullRoute.Add(InnerRoute.childNodeList[i].GetComponent<Node>());
        }
    }

    private IEnumerator Move(int rolledDice) // Coroutine to move player all his steps
    {
        if(isMoving) yield break;

        isMoving = true;

        while(steps > 0)
        {
            routePosition++;

            var nextPos = fullRoute[routePosition].gameObject.transform.position;
            var startPos = fullRoute[routePosition - 1].gameObject.transform.position;

            while(MoveInArcToNextNode(startPos, nextPos, 5f)) { yield return null; }

            yield return new WaitForSeconds(0.1f);

            cTime = 0;
            steps--;
            doneSteps++;
        }

        goalNode = fullRoute[routePosition];

        // Check possible kick
        if(goalNode.isTaken)
        {
            // Kick the other stone
            goalNode.stone.ReturnToBase();
        }

        // Clean current node
        currentNode.stone = null;
        currentNode.isTaken = false;

        // Set up next node
        goalNode.stone = this;
        goalNode.isTaken = true;

        // Set current to next node
        currentNode = goalNode;
        goalNode = null;

        // Report to game manager
        // Check if player won
        if(WinCondition())
        {
            GameManager.instance.ReportWinning();
        }

        // Roll dice again if a 6 was rolled
        if (rolledDice == 6) GameManager.instance.state = GameManager.States.ROLL_DICE;
        // Switch the player otherwise
        else GameManager.instance.state = GameManager.States.SWITCH_PLAYER;

        isMoving = false;
    }

    private IEnumerator MoveOutOfBase() // Coroutine to move player out of base
    {
        if (isMoving) yield break;

        isMoving = true;

        while (steps > 0)
        {

            var nextPos = fullRoute[routePosition].gameObject.transform.position;
            var startPos = baseNode.gameObject.transform.position;

            while (MoveInArcToNextNode(startPos, nextPos, 4f)) { yield return null; }

            yield return new WaitForSeconds(0.1f);

            cTime = 0;
            steps--;
            doneSteps++;
        }

        // Update node
        goalNode = fullRoute[routePosition];
        
        // Check if we kick another stone
        if(goalNode.isTaken)
        {
            goalNode.stone.ReturnToBase();
        }

        goalNode.stone = this;
        goalNode.isTaken = true;

        currentNode = goalNode;
        goalNode = null;

        // Report back to game manager
        GameManager.instance.state = GameManager.States.ROLL_DICE;

        isMoving = false;
    }

    bool MovingToNextNode(Vector3 goalPos, float speed)
    {
        return goalPos != (transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime));
    }

    bool MoveInArcToNextNode(Vector3 startPos, Vector3 goalPos, float speed)
    {
        cTime += speed * Time.deltaTime;
        var myPosition = Vector3.Lerp(startPos, goalPos, cTime);

        myPosition.y += amplitude * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);

        return goalPos != (transform.position = Vector3.Lerp(transform.position, myPosition, cTime));
    }

    public bool ReturnIsOut()
    {
        return isOut;
    }

    public void LeaveBase()
    {
        steps = 1; // Move once inside MoveOutOfBase() function
        isOut = true;
        routePosition = 0;

        StartCoroutine(MoveOutOfBase());
    }

    public bool CheckPossibleMove(int rolledDice)
    {
        int tempPos = routePosition + rolledDice;

        if (tempPos >= fullRoute.Count) return false;

        return !fullRoute[tempPos].isTaken;
    }

    public bool CheckPossibleKick(int stoneID, int rolledDice)
    {
        int tempPos = routePosition + rolledDice;

        if (tempPos >= fullRoute.Count) return false;

        if (fullRoute[tempPos].isTaken)
        {
            if (stoneID == fullRoute[tempPos].stone.stoneID)
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public void StartMove(int rolledDice)
    {
        steps = rolledDice;
        StartCoroutine(Move(rolledDice));
    }

    public void ReturnToBase()
    {
        StartCoroutine(Return());
    }

    private IEnumerator Return()
    {
        GameManager.instance.ReportTurnPossible(false);

        routePosition = 0;
        currentNode = null;
        goalNode = null;
        isOut = false;
        doneSteps = 0;

        var baseNodePos = baseNode.gameObject.transform.position;

        while(MovingToNextNode(baseNodePos, 8f)) { yield return null; }

        GameManager.instance.ReportTurnPossible(true);
    }

    private bool WinCondition()
    {
        foreach(var nodeTransform in InnerRoute.childNodeList)
        {
            if(!nodeTransform.GetComponent<Node>().isTaken)
            {
                return false;
            }
        }

        return true;
    }

    #region human input

    public void SetSelector(bool active)
    {
        selector.SetActive(active);
        // This is for having the click ability
        hasTurn = active;
    }

    private void OnMouseDown()
    {
        if(hasTurn)
        {
            if(!isOut)
            {
                LeaveBase();
            }
            else
            {
                StartMove(GameManager.instance.rolledHumanDice);
            }

            GameManager.instance.DeactivateSelectors();
        }
    }

    #endregion
}
