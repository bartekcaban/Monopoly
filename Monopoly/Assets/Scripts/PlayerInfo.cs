using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfo
{
    private static List<string> playerNames;

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


}
