using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoosingPlayersMenu : MonoBehaviour
{
    public GameObject choosingPlayersMenuCanvas;
    private static ChoosingPlayersMenu choosingPlayersMenu;

    public List<string> playerNames = new List<string>();

    int numberOfInputFields = 4;
    public TMP_InputField player1InputField;
    public TMP_InputField player2InputField;
    public TMP_InputField player3InputField;
    public TMP_InputField player4InputField;
    public List<TMP_InputField> playerInputFields;

    public Button addPlayerButton;
    public Button removePlayerButton;
    public Button letsStartButton;

    public TMP_Text removePlayerButtonLabel;

    void Start()
    {
        playerInputFields = new List<TMP_InputField>() { player1InputField, player2InputField, player3InputField, player4InputField };
        addPlayerButton.onClick.AddListener(handleAddPlayerButtonClick);
        removePlayerButton.onClick.AddListener(handleRemovePlayerButtonClick);
        letsStartButton.onClick.AddListener(handleLetsStartButtonClick);
    }

    void Update()
    {
        // Let's start button interctability
        if (checkIfStartIsPossible() && !letsStartButton.interactable) letsStartButton.interactable = true;
        else if (!checkIfStartIsPossible() && letsStartButton.interactable) letsStartButton.interactable = false;

        // Add player button interctability
        if (shouldAddPlayerButtonBeInteractable() && !addPlayerButton.interactable) addPlayerButton.interactable = true;
        else if (!shouldAddPlayerButtonBeInteractable() && addPlayerButton.interactable) addPlayerButton.interactable = false;

        // Remove player button activity
        if (shouldRemovePlayerButtonBeActive() && !removePlayerButton.gameObject.activeSelf)
        {
            removePlayerButton.gameObject.SetActive(true);
            removePlayerButtonLabel.gameObject.SetActive(true);
        }
        else if (!shouldRemovePlayerButtonBeActive() && removePlayerButton.gameObject.activeSelf)
        {
            removePlayerButton.gameObject.SetActive(false);
            removePlayerButtonLabel.gameObject.SetActive(false);
        }
    }

    public static ChoosingPlayersMenu Instance()
    {
        if (!choosingPlayersMenu)
        {
            choosingPlayersMenu = FindObjectOfType(typeof(ChoosingPlayersMenu)) as ChoosingPlayersMenu;
            if (!choosingPlayersMenu)
                Debug.LogError("There is no choosing players menu object in scene!");
        }

        return choosingPlayersMenu;
    }

    public void handleAddPlayerButtonClick()
    {
        for (int i = 2; i < numberOfInputFields; i++) 
        {
            if (!playerInputFields[i].gameObject.activeSelf)
            {
                playerInputFields[i].gameObject.SetActive(true);
                break;
            }
        }

    }

    public void handleRemovePlayerButtonClick()
    {
        for (int i = numberOfInputFields - 1; i > 1; i--)
        {
            if (playerInputFields[i].gameObject.activeSelf)
            {
                playerInputFields[i].gameObject.SetActive(false);
                break;
            }
        }
    }

    public void handleLetsStartButtonClick()
    {
        choosingPlayersMenuCanvas.SetActive(false);
    }

    // Start is possible when at least 2 players have a name
    bool checkIfStartIsPossible()
    {
        int playersCounter = 0;
        foreach(TMP_InputField inputField in playerInputFields)
        {
            if (inputField.text.Length != 0) playersCounter++;
        }
        return (playersCounter > 1) ? true : false;
    }

    bool shouldAddPlayerButtonBeInteractable()
    {
        int activeFieldsCounter = 0;
        foreach (TMP_InputField inputField in playerInputFields)
        {
            if (inputField.gameObject.activeSelf) activeFieldsCounter++;
        }
        return (activeFieldsCounter < numberOfInputFields) ? true : false;
    }

    bool shouldRemovePlayerButtonBeActive()
    {
        int activeFieldsCounter = 0;
        foreach (TMP_InputField inputField in playerInputFields)
        {
            if (inputField.gameObject.activeSelf) activeFieldsCounter++;
        }
        return (activeFieldsCounter > 2) ? true : false;
    }

    //void DescriptionInit(Property property)
    //{
    //    housePrice.text = property.housePrice.ToString();
    //    hotelPrice.text = property.hotelPrice.ToString();
    //    oneHouseRent.text = property.rentPerHouse.ToString();
    //    twoHouseRent.text = (property.rentPerHouse * 2).ToString();
    //    threeHouseRent.text = (property.rentPerHouse * 3).ToString();
    //    fourHouseRent.text = (property.rentPerHouse * 4).ToString();
    //}

    //public void ShowAbleToBuy(Property property)
    //{
    //    decisionDescription.text = "Ta nieruchomość nie ma jeszcze właściciela";

    //    dialogCanvasObject.SetActive(true);
    //    okButton.gameObject.SetActive(true);
    //    buyButton.gameObject.SetActive(true);
    //    expandButton.gameObject.SetActive(false);
    //    depositButton.gameObject.SetActive(false);
    //    DescriptionInit(property);


    //}
    //public void ShowForPropertyOwner(Property property)
    //{
    //    decisionDescription.text = "Jesteś właścicielem tej nieruchomości";

    //    dialogCanvasObject.SetActive(true);
    //    okButton.gameObject.SetActive(true);
    //    buyButton.gameObject.SetActive(false);
    //    expandButton.gameObject.SetActive(true);
    //    depositButton.gameObject.SetActive(true);
    //    DescriptionInit(property);
    //}
    //public void ShowForRentPayment(Property property)
    //{
    //    decisionDescription.text = "Płacisz czynsz na rzecz gracza: GRACZ w wysokości 100 zł";


    //    dialogCanvasObject.SetActive(true);
    //    okButton.gameObject.SetActive(true);
    //    buyButton.gameObject.SetActive(false);
    //    expandButton.gameObject.SetActive(false);
    //    depositButton.gameObject.SetActive(false);
    //    DescriptionInit(property);

    //}
}
