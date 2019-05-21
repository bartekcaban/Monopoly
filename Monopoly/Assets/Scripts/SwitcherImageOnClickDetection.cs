using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitcherImageOnClickDetection : MonoBehaviour, IPointerClickHandler
{
    DialogMenu dialogMenu;
    public Game game;
    private Texture[] textures;
    private int currentTextureIndex;

    // Start is called before the first frame update
    void Start()
    {
        dialogMenu = DialogMenu.Instance();
        GameUI gameUIScript = GameObject.Find("GameUIManager").GetComponent<GameUI>();
        textures = gameUIScript.textures;
        currentTextureIndex = gameUIScript.currentTextureIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("I'm in OnPointerClick");
        Debug.Log("currentTextureIndex: " + currentTextureIndex);
        Property currentlyShowedProperty = game.currentPlayer.ownedProperties.FirstOrDefault(x => x.propertyName.ToLower() == textures[currentTextureIndex].name.ToLower());
        if (currentlyShowedProperty) Debug.Log("currentlyShowedProperty name: " + currentlyShowedProperty.propertyName); else Debug.Log("currentlyShowedProperty not found.");
        dialogMenu.ShowForPropertyOwner(currentlyShowedProperty);
    }
}
