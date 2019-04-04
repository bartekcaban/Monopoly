using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PawnMovement pawn;
    public Dice dice;
    bool moving;
    bool diceRolled;
    int cash;
    int currentFieldId;
    //TODO: lista posiadanych pól

    public void MoveToPosition(int index) //przesunięcie na wybraną pozycję
    {
        currentFieldId = index;
        if (!pawn.IsDestinationReached())
            pawn.AllowMovement(index);
    }

    public void AllowMovement()
    {
        int destinationFieldId = currentFieldId + dice.GetRolledValue();
        if (destinationFieldId > 42)
            destinationFieldId = destinationFieldId - 43;
        if(!pawn.IsDestinationReached())
            pawn.AllowMovement(destinationFieldId);
    }

    public bool IsMoving()
    {
        return moving;
    }

    public void AllowRolling()
    {
        dice.EnableRolling();
        moving = true;
        pawn.SetDestinationReached(false);
    }

    public bool DiceRolled()
    {
        return diceRolled;
    }
    public int GetCurrentPosition()
    {
        return currentFieldId;
    }
    public int GetId()
    {
        //Just for now, to overload in specific player class 
        return 0;
    }

    public bool PawnMoved()
    {
        if (pawn.IsDestinationReached())
        {
            moving = false;
            diceRolled = false;
            currentFieldId = currentFieldId + dice.GetRolledValue();
        }
        return pawn.IsDestinationReached();
    }

    public void Disable()
    {
        transform.position = new Vector3(0.0f, -10.0f, 0.0f);
        GetComponent<Renderer>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        diceRolled = false;
        cash = 10000;
        currentFieldId = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(dice.Rolled() && moving)
        {
            dice.GetRolledValue();
            diceRolled= true;
        }
    }
}
