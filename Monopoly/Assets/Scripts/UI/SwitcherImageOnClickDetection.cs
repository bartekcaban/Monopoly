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

    // Start is called before the first frame update
    void Start()
    {
        dialogMenu = DialogMenu.Instance();
        gameUIScript = GameObject.Find("GameUIManager").GetComponent<GameUI>();
        textures = gameUIScript.textures;
        currentTextureIndex = gameUIScript.currentTextureIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("On pointer click!");
        currentTextureIndex = gameUIScript.currentTextureIndex;
        Property displayedProperty = game.currentPlayer.ownedProperties.ElementAt(currentTextureIndex);
        dialogMenu.ShowForPropertyOwner(displayedProperty, game.playerExpandedCurrentProperty, game.playerDepositedCurrentProperty, ()=> { });
    }
}
