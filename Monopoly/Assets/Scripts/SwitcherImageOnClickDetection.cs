using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitcherImageOnClickDetection : MonoBehaviour, IPointerClickHandler
{
    DialogMenu dialogMenu;
    public Game game;
    private List<Texture> chosenTextures;
    private int currentTextureIndex;

    // Start is called before the first frame update
    void Start()
    {
        dialogMenu = DialogMenu.Instance();
        GameUI gameUIScript = GameObject.Find("GameUIManager").GetComponent<GameUI>();
        chosenTextures = gameUIScript.chosenTextures;
        currentTextureIndex = gameUIScript.currentTextureIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("I'm in OnPointerClick");
        var currentlyShowedProperty = game.currentPlayer.ownedProperties.FirstOrDefault(x => x.propertyName.ToLower() == chosenTextures[currentTextureIndex].name.ToLower());
        Debug.Log("chosenTextures: " + chosenTextures.Count);
        Debug.Log("currentlyShowedProperty name: " + currentlyShowedProperty.propertyName);
        dialogMenu.ShowForPropertyOwner(currentlyShowedProperty);
    }
}
