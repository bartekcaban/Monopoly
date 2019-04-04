using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        // stopping the game
        Time.timeScale = 0f;

        playerInputFields = new List<TMP_InputField>() { player1InputField, player2InputField, player3InputField, player4InputField };
        addPlayerButton.onClick.AddListener(handleAddPlayerButtonClick);
        removePlayerButton.onClick.AddListener(handleRemovePlayerButtonClick);
        letsStartButton.onClick.AddListener(handleLetsStartButtonClick);
    }

    void Update()
    {
        manageButtonsVisibility();
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

    void handleAddPlayerButtonClick()
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

    void handleRemovePlayerButtonClick()
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

    void handleLetsStartButtonClick()
    {
        choosingPlayersMenuCanvas.SetActive(false);

        // resuming the game
        Time.timeScale = 1f;
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

    void manageButtonsVisibility()
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
}
