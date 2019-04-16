using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PawnMovement pawn;
    public Dice dice;
    public string playerName;
    public int cash;

    bool moving;
    bool diceRolled;

    bool moveFinished;

    int currentFieldId;

    //TODO: lista posiadanych pól

    public void MoveToPosition(int index) //przesunięcie na wybraną pozycję
    {
        currentFieldId = index;
        if (!pawn.IsDestinationReached())
            pawn.AllowMovement(index);
    }
    public void SetMoveFinished()
    {
        moveFinished = true;
    }
    public bool MoveFinished()
    {
        return moveFinished;
    }

    public void AllowMovement()
    {
        int destinationFieldId = currentFieldId + dice.GetRolledValue();
        if (destinationFieldId > 40)
            destinationFieldId = destinationFieldId - 41;
        if(!pawn.IsDestinationReached())
            pawn.AllowMovement(destinationFieldId);
        Debug.Log("dest " + currentFieldId);
    }

    public bool IsMoving()
    {
        return moving;
    }

    public void AllowRolling()
    {
        dice.EnableRolling();
        moving = true;
        moveFinished = false;
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
        if (pawn.IsDestinationReached() && moving)
        {
            moving = false;
            diceRolled = false;
            currentFieldId = currentFieldId + dice.GetRolledValue();
        }
        Debug.Log(pawn.IsDestinationReached());
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
        cash = 1500;
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
