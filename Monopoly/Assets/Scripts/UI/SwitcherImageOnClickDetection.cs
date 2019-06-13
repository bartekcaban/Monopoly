using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitcherImageOnClickDetection : MonoBehaviour, IPointerDownHandler
{
    GameUI gameUIScript;
    DialogMenu dialogMenu;
    public Game game;
    private Texture[] textures;
    private int currentTextureIndex;
    
    void Start()
    {
        dialogMenu = DialogMenu.Instance();
        gameUIScript = GameObject.Find("GameUIManager").GetComponent<GameUI>();
        textures = gameUIScript.textures;
        currentTextureIndex = gameUIScript.currentTextureIndex;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentTextureIndex = gameUIScript.currentTextureIndex;
        Property displayedProperty = game.currentPlayer.ownedProperties.ElementAt(currentTextureIndex);
        dialogMenu.ShowForPropertyOwner(displayedProperty, game.playerExpandedCurrentProperty, game.playerDepositedCurrentProperty, ()=> { });
    }
}
