﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUI : MonoBehaviour
{
    public Texture[] textures; //List of Sprites added from the Editor to be created as GameObjects at runtime
    private Texture currentTexture;
    private int currentTextureIndex = 0;

    public GameObject playerStatisticsPanel; //Parent Panel you want the new Images to be children of
    public Game game;
    public TMP_Text currentPlayerName;
    public TMP_Text currentPlayerCash;
    public TMP_Text nextPlayerName;
    private int cardSpriteIndex = 1;
    public Image cardImage;
    private Sprite sprite;
    
    public Button shiftLeftButton;
    public Button shiftRightButton;

    private List<Player> playersList;
    private Player currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // playersList = GameObject.Find("Board").GetComponent<Game>().players;
        shiftLeftButton.onClick.AddListener(handleLeftButtonClick);
        shiftRightButton.onClick.AddListener(handleRightButtonClick);

        resolvePlayerCardImage();

        //sprite = Sprite.Create((Texture2D)textures[0], new Rect(0.0f, 0.0f, textures[0].width, textures[0].height), new Vector2(0.5f, 0.5f), 100.0f);
        //cardImage.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (game.currentPlayer)
        {
            currentPlayerName.text = game.currentPlayer.playerName;
            currentPlayerCash.text = game.currentPlayer.cash.ToString();
        }
        if (game.nextPlayer) nextPlayerName.text = game.nextPlayer.playerName;
    }

    void resolvePlayerCardImage()
    {
        currentTexture = textures[currentTextureIndex];
        sprite = Sprite.Create((Texture2D)currentTexture, new Rect(0.0f, 0.0f, currentTexture.width, currentTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        cardImage.sprite = sprite;
    }

    private void handleLeftButtonClick()
    {
        currentTextureIndex--;
        if (currentTextureIndex < 0) currentTextureIndex = 0;
        resolvePlayerCardImage();
    }

    private void handleRightButtonClick()
    {
        currentTextureIndex++;
        if (currentTextureIndex > textures.Length - 1) currentTextureIndex = textures.Length - 1;
        resolvePlayerCardImage();
    }
}
