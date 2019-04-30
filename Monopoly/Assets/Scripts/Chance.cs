using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChanceType
{
    money,
    moveTo,
    move
}

public class Chance : MonoBehaviour
{
    string description;
    int value;
    ChanceType type;

    public Chance(string description, int value, ChanceType type)
    {
        this.description = description;
        this.value = value;
        this.type = type;
    }

    public int ReturnValue()
    {
        return value;
    }

    public string ReturnDescription()
    {
        return description;
    }
}
