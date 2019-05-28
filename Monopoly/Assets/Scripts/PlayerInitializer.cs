using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    private static List<string> playerNames;

    public static void SetPlayerNames(List<string> listOfNames)
    {
        playerNames = listOfNames;
    }

    public List<string> GetPlayerNames()
    {
        return playerNames;
    }

    public List<Player> CreatePlayers(int number)
    {
        List<Player> players = new List<Player>();

        players.Add((Player)GameObject.Find("Cat").GetComponent(typeof(Player)));
        players.Add((Player)GameObject.Find("Teapot").GetComponent(typeof(Player)));

        if (number < 2) number = 2;
        if (number > 4) number = 4;

        if (number == 2)
        {
            ((Player)GameObject.Find("Dog").GetComponent(typeof(Player)) as Player).Disable();
            ((Player)GameObject.Find("Hat").GetComponent(typeof(Player)) as Player).Disable();
        }

        else if (number == 3)
        {
            players.Add((Player)GameObject.Find("Dog").GetComponent(typeof(Player)));
            ((Player)GameObject.Find("Hat").GetComponent(typeof(Player)) as Player).Disable();
        }

        else if (number == 4)
        {
            players.Add((Player)GameObject.Find("Dog").GetComponent(typeof(Player)));
            players.Add((Player)GameObject.Find("Hat").GetComponent(typeof(Player)));
        }

        for (int i = 0; i < number; i++)
        {
            players[i].playerName = playerNames[i];
        }

        return players;
    }
}
