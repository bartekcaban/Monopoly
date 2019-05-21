using UnityEngine;


public class Chance : MonoBehaviour
{
    string description;
    int value;

    public Chance(string description, int value)
    {
        this.description = description;
        this.value = value;
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
