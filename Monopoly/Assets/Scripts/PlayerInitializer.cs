using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    const int minAmountOfPlayers = 2;
    const int maxAmountOfPlayers = 4;
    
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
        if (number < minAmountOfPlayers) number = minAmountOfPlayers;
        if (number > maxAmountOfPlayers) number = maxAmountOfPlayers;

        players.Add((Player)GameObject.Find("Cat").GetComponent(typeof(Player)));
        players.Add((Player)GameObject.Find("Teapot").GetComponent(typeof(Player)));

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
