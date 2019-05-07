using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class GameUI : MonoBehaviour
{
    public Texture[] textures; // wszystkie tekstury pól dodane z poziomu edytora
    private Texture currentTexture;
    private int currentTextureIndex = 0;

    public Game game;
    public TMP_Text currentPlayerName;
    public TMP_Text currentPlayerCash;
    public TMP_Text nextPlayerName;
    public TMP_Text noneTextField;
    public Image cardImage;    
    public Button shiftLeftButton;
    public Button shiftRightButton;

    private Texture[] currentPlayerTextures;

    private bool switcherEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        // playersList = GameObject.Find("Board").GetComponent<Game>().players;
        shiftLeftButton.onClick.AddListener(handleLeftButtonClick);
        shiftRightButton.onClick.AddListener(handleRightButtonClick);

        resolvePlayerCardImage();        
    }

    // Update is called once per frame
    void Update()
    {        
        if (game.currentPlayer)
        {
            currentPlayerName.text = game.currentPlayer.playerName;
            currentPlayerCash.text = game.currentPlayer.cash.ToString();

            if (game.currentPlayer.ownedProperties.Count <= 0 && switcherEnabled) disableImageSwitcher();
            else if (game.currentPlayer.ownedProperties.Count > 0 && !switcherEnabled) enableImageSwitcher();
        }
        if (game.nextPlayer) nextPlayerName.text = game.nextPlayer.playerName;
    }

    private void disableImageSwitcher()
    {
        cardImage.enabled = false;
        shiftLeftButton.gameObject.SetActive(false);
        shiftRightButton.gameObject.SetActive(false);
        noneTextField.gameObject.SetActive(true);
        switcherEnabled = false;
    }

    private void enableImageSwitcher()
    {
        cardImage.enabled = true;
        shiftLeftButton.gameObject.SetActive(true);
        shiftRightButton.gameObject.SetActive(true);
        noneTextField.gameObject.SetActive(false);
        switcherEnabled = true;
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

    void resolvePlayerCardImage()
    {        
        currentTexture = textures[currentTextureIndex];
        cardImage.sprite = Sprite.Create((Texture2D)currentTexture, new Rect(0.0f, 0.0f, currentTexture.width, currentTexture.height), new Vector2(0.5f, 0.5f), 100.0f); ;
    }

    private void resolveCurrentPlayerTextures()
    {
        Player currentPlayer;
        if (game.players != null)
        {
            currentPlayer = game.players.Find(x => x.name.Equals(currentPlayerName));
            // zmapowanie ownedProperties currentPlayera na tekstury z tablicy textures[].
            // nazwa tekstury jest przechowywana w texture.name (np. Warsaw, jail, chance)

            // List<Texture> chosenTextures = textures.Where(x => currentPlayer.ownedProperties.Exists(p => p.name == x.name)); //////////// TO DO
        }
    }
}
