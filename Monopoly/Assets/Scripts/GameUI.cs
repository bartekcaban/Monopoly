using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public Texture[] textures; //List of Sprites added from the Editor to be created as GameObjects at runtime
    public GameObject playerStatisticsPanel; //Parent Panel you want the new Images to be children of
    public Game game;
    public TMP_Text currentPlayerName;
    public TMP_Text currentPlayerCash;
    public TMP_Text nextPlayerName;
    private int cardSpriteIndex = 1;

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
            currentPlayerCash.text = game.currentPlayer.cash.ToString();
            resolvePlayerCardImage();
        }
        if (game.nextPlayer) nextPlayerName.text = game.nextPlayer.playerName;
    }

    void resolvePlayerCardImage()
    {
        GameObject NewObj = new GameObject();
        Image newImage = NewObj.AddComponent<Image>();
        //newImage.sprite = Sprites[cardSpriteIndex];
        NewObj.GetComponent<RectTransform>().SetParent(playerStatisticsPanel.transform);
        NewObj.SetActive(true);
    }   
}
