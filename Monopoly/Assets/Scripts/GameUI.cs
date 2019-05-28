using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class GameUI : MonoBehaviour, IPointerClickHandler
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

    private Texture[] currentPlayerTextures;
    public List<Texture> chosenTextures;

    private bool switcherEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        gameUICanvas.gameObject.SetActive(true);
        shiftLeftButton.onClick.AddListener(handleLeftButtonClick);
        shiftRightButton.onClick.AddListener(handleRightButtonClick);
        dialogMenu = DialogMenu.Instance();
        game = GameObject.Find("Plane").GetComponent<Game>();
        moneyManager = game.moneyManager;
    }

    // Update is called once per frame
    void Update()
    {        
        if (game.currentPlayer)
        {
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("I'm in OnPointerClick");
        var currentlyShowedProperty = game.currentPlayer.ownedProperties.FirstOrDefault(x => x.propertyName.ToLower() == chosenTextures[currentTextureIndex].name.ToLower());
        dialogMenu.ShowForPropertyOwner(currentlyShowedProperty);
    }

    private void handleLeftButtonClick()
    {
        currentTextureIndex--;
        if (currentTextureIndex < 0) currentTextureIndex = chosenTextures.Count - 1;
        resolvePlayerCardImage();
    }

    private void handleRightButtonClick()
    {
        currentTextureIndex++;
        if (currentTextureIndex > chosenTextures.Count - 1) currentTextureIndex = 0;
        resolvePlayerCardImage();
    }

    void resolvePlayerCardImage()
    {
        if (chosenTextures.Count > 0) {
            currentTexture = chosenTextures.ElementAt(currentTextureIndex);
            cardImage.sprite = Sprite.Create((Texture2D)currentTexture, new Rect(0.0f, 0.0f, currentTexture.width, currentTexture.height), new Vector2(0.0f, 0.0f), 100.0f);
        }
    }

    private void resolveCurrentPlayerTextures()
    {
        Player currentPlayer;
        if (game.players != null)
        {
            currentPlayer = game.players.Find(x => x.playerName == currentPlayerName.text);
            // zmapowanie ownedProperties currentPlayera na tekstury z tablicy textures[].
            // nazwa tekstury jest przechowywana w texture.name (np. Warsaw, jail, chance)
            
            chosenTextures = new List<Texture>{ };
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

            resolvePlayerCardImage();
        }
    }
}
