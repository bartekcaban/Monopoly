using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class created only to store information about playerNames
public static class PlayerInfo
{
    private static List<string> playerNames;
    private static int numberOfPlayers;

    public static List<string> PlayerNames
    {
        get
        {
            return playerNames;
        }
        set
        {
            playerNames = value;
        }
    }

    public static int NumberOfPlayers
    {
        get
        {
            return numberOfPlayers;
        }
        set
        {
            numberOfPlayers = value;
        }
    }


}
