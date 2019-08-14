using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

struct PlayerStorage
{
    public Player player;
}

public class GameUI : MonoBehaviour
{
    public Canvas gameUICanvas;
    public Texture[] textures; // wszystkie tekstury pól dodane z poziomu edytora
    private Texture currentTexture;
    public int currentTextureIndex = 0;
    MoneyManager moneyManager;
    public Game game;
    DialogMenu dialogMenu;
    public TMP_Text currentPlayerName;
    public TMP_Text currentPlayerCash;
    public TMP_Text nextPlayerName;
    public TMP_Text noneTextField;
    public Image cardImage;    
    public Button shiftLeftButton;
    public Button shiftRightButton;
    public Button finishTurnButton;

    private Texture[] currentPlayerTextures;
    public List<Texture> chosenTextures;

    private bool switcherEnabled = true;
    public bool spriteResolved = false;
    public bool texturesResolved = false;

    private PlayerStorage currentPlayerStorage;

    // Start is called before the first frame update
    void Start()
    {
        gameUICanvas.gameObject.SetActive(true);
        shiftLeftButton.onClick.AddListener(handleLeftButtonClick);
        shiftRightButton.onClick.AddListener(handleRightButtonClick);
        finishTurnButton.onClick.AddListener(game.finishTurn);
        finishTurnButton.gameObject.SetActive(false);
        dialogMenu = DialogMenu.Instance();
        game = GameObject.Find("Plane").GetComponent<Game>();
        moneyManager = game.moneyManager;
        currentPlayerStorage.player = game.players[0];
    }

    // Update is called once per frame
    void Update()
    {        
        if (game.currentPlayer)
        {
            if (game.currentPlayer != currentPlayerStorage.player)
            {
                currentPlayerStorage.player = game.currentPlayer;
                texturesResolved = false;
                spriteResolved = false;
                currentTextureIndex = 0;
            }
            currentPlayerName.text = game.currentPlayer.playerName;
            currentPlayerCash.text = moneyManager.GetAccountState(currentPlayerName.text).ToString();

            if (game.currentPlayer.ownedProperties.Count <= 0 && switcherEnabled) disableImageSwitcher();
            else if (game.currentPlayer.ownedProperties.Count > 0 && !switcherEnabled) enableImageSwitcher();

            if (switcherEnabled)
            {
                resolveCurrentPlayerTextures();
            }
        }
        if (game.nextPlayer) nextPlayerName.text = game.nextPlayer.playerName;
        if (game.endTurnButtonVisible) finishTurnButton.gameObject.SetActive(true);
        else finishTurnButton.gameObject.SetActive(false);
    }

    private void disableImageSwitcher()
    {
        cardImage.enabled = false;
        cardImage.gameObject.SetActive(false);
        shiftLeftButton.gameObject.SetActive(false);
        shiftRightButton.gameObject.SetActive(false);
        noneTextField.gameObject.SetActive(true);
        switcherEnabled = false;
    }

    private void enableImageSwitcher()
    {
        cardImage.enabled = true;
        cardImage.gameObject.SetActive(true);
        shiftLeftButton.gameObject.SetActive(true);
        shiftRightButton.gameObject.SetActive(true);
        noneTextField.gameObject.SetActive(false);
        switcherEnabled = true;
    }

    private void handleLeftButtonClick()
    {
        currentTextureIndex--;
        if (currentTextureIndex < 0) currentTextureIndex = chosenTextures.Count - 1;
        spriteResolved = false;
        resolvePlayerCardImage();
    }
    
    private void handleRightButtonClick()
    {
        currentTextureIndex++;
        spriteResolved = false;
        if (currentTextureIndex > chosenTextures.Count - 1) currentTextureIndex = 0;
        resolvePlayerCardImage();
    }

    void resolvePlayerCardImage()
    {
        if (!spriteResolved && chosenTextures.Count > 0) {
            currentTexture = chosenTextures.ElementAt(currentTextureIndex);
            cardImage.sprite = Sprite.Create((Texture2D)currentTexture, new Rect(0.0f, 0.0f, currentTexture.width, currentTexture.height), new Vector2(0.0f, 0.0f), 100.0f);
            spriteResolved = true;
        }
    }

    private void resolveCurrentPlayerTextures()
    {
        if (!texturesResolved)
        {
            Player currentPlayer;
            if (game.players != null)
            {
                currentPlayer = game.players.Find(x => x.playerName == currentPlayerName.text);

                chosenTextures = new List<Texture> { };
                foreach (Texture texture in textures)
                {
                    foreach (Property property in currentPlayer.ownedProperties)
                    {
                        if (property.propertyName.ToLower() == texture.name.ToLower())
                        {
                            chosenTextures.Add(texture);
                        }
                    }
                }
                texturesResolved = true;
                resolvePlayerCardImage();
            }
        }
    }
}
