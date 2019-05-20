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


    public List<Property> ownedProperties;


    public void MoveToPosition(int index) //przesunięcie na wybraną pozycję
    {
        currentFieldId = index;
        if (!pawn.IsDestinationReached())
            pawn.AllowMovement(index);
    }
    public void MoveByNumberOfFields(int number)
    {
        currentFieldId += number;
        if (currentFieldId > 40)
            currentFieldId = currentFieldId - 41;
        if (!pawn.IsDestinationReached())
            pawn.AllowMovement(currentFieldId);
    }*/
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
        {
            destinationFieldId = destinationFieldId - 41;
            cash += 200;
        }
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
