using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;

    private bool hasLanded = false;
    private bool thrown = false;

    Vector3 initialPosition;

    public List<DiceSide> diceSides;
    private int diceValue;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void RollDice()
    {
        ResetDice();

        UIManager.Instance.RollDiceButtonVisible(false);

        if(!thrown && !hasLanded)
        {
            thrown = true;
            rb.useGravity = true;

            rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
        }
        else if(thrown && hasLanded) // Reset dice
        {
            ResetDice();
        }
    }

    private void ResetDice()
    {
        transform.position = initialPosition;
        rb.isKinematic = false;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
    }

    private void Update()
    {
        // Dice is lying on the ground
        if(rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = true;
            rb.useGravity = false;
            rb.isKinematic = true;

            // Side value check
            SideValueCheck();
        }
        // Dice is stuck in some kind of invalid position (no "sensor" is triggering)
        else if(rb.IsSleeping() && hasLanded &&  diceValue == 0)
        {
            // Roll again
            RollAgain();
        }
    }

    private void RollAgain()
    {
        ResetDice();
        thrown = true;
        rb.useGravity = true;

        rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
    }

    private void SideValueCheck()
    {
        diceValue = 0;

        foreach(var side in diceSides)
        {
            if(side.OnGround)
            {
                diceValue = side.sideValue;

                // Send result to game manager
                GameManager.Instance.ReportDiceRollResults(diceValue);
            }
        }
    }
}
