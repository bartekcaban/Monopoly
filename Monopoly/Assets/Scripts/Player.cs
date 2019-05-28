using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Property> ownedProperties;
    public PawnMovement pawn;
    public Dice dice;
    public string playerName;
    int cash;
    int turnsPausing;
    bool moving;
    bool diceRolled;
    int id;
    bool moveFinished;
    int currentFieldIndex;

    public int GetCash()
    {
        return this.cash;
    }

    public void SetCash(int money)
    {
        this.cash = money;
    }

    public bool CanMove()
    {
        return (turnsPausing == 0) ? true : false;
    }

    public void PauseOneTurn()
    {
        turnsPausing -= 1;
    }

    public int ReturnTurnsPausing()
    {
        return turnsPausing;
    }

    public void GoToJail()
    {
        turnsPausing = 3;
        currentFieldIndex = 10;
        pawn.AllowMovement(10);
    }

    public void MoveToPosition(int index) //przesunięcie na wybraną pozycję
    {
        currentFieldIndex = index;
        if (!pawn.IsDestinationReached())
            pawn.AllowMovement(index);
    }
    public void MoveByNumberOfFields(int number)
    {
        currentFieldIndex += number;
        if (currentFieldIndex > 40)
            currentFieldIndex = currentFieldIndex - 41;
        if (!pawn.IsDestinationReached())
            pawn.AllowMovement(currentFieldIndex);
    }
    public void SetMoveFinished()
    {
        moveFinished = true;
    }
    public bool MoveFinished()
    {
        return moveFinished;
    }

    public bool AllowMovement()
    {
        int destinationFieldIndex = currentFieldIndex + dice.GetRolledValue();
        if (destinationFieldIndex > 40)
        {
            destinationFieldIndex = destinationFieldIndex - 41;
            return false;

        }
        if(!pawn.IsDestinationReached())
            pawn.AllowMovement(destinationFieldIndex);

        return true;
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
    public void Buy(Property property)
    {
        ownedProperties.Add(property);
    }
    public bool IsOwnerOfWholeGroup(PropertyGroupName groupName, Dictionary<PropertyGroupName,int> numberOfPropertiesInGroup)
    {

        var groupProperties = this.ownedProperties.FindAll(prop => prop.groupName == groupName);
        if (numberOfPropertiesInGroup[groupName] == groupProperties.Count) return true;
        else return false;

    }
    public int GetCurrentPosition()
    {
        return currentFieldIndex;
    }
    public int GetId()
    {
       return id;
    }
    public void SetId(int id)
    {
        this.id = id;
    }

    public bool PawnMoved()
    {
        if (pawn.IsDestinationReached() && moving)
        {
            moving = false;
            diceRolled = false;
            currentFieldIndex = currentFieldIndex + dice.GetRolledValue();
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
        ownedProperties = new List<Property>();
        moving = false;
        diceRolled = false;
        this.SetCash(1500);
        currentFieldIndex = 0;
        turnsPausing = 0;
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
