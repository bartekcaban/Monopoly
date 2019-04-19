using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public Game game;
    public TMP_Text currentPlayerName;
    public TMP_Text nextPlayerName;

    // Start is called before the first frame update
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        if (game.currentPlayer)
        {
            currentPlayerName.text = game.currentPlayer.playerName;
        }
        if (game.nextPlayer) nextPlayerName.text = game.nextPlayer.playerName;
    }
}
