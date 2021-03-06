﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System;

public class ChoosingPlayersMenu : MonoBehaviour
{
    public GameObject choosingPlayersMenuCanvas;
    private static ChoosingPlayersMenu choosingPlayersMenu;

    public List<string> playerNames = new List<string>();

    int numberOfInputFields = 4;
    public InputField player1InputField;
    public InputField player2InputField;
    public InputField player3InputField;
    public InputField player4InputField;
    public IList<InputField> playerInputFields;

    public Button addPlayerButton;
    public Button removePlayerButton;
    public Button letsStartButton;

    public Text removePlayerButtonLabel;

    private bool choosingFinished = false;

    void Start()
    {
        playerInputFields = new List<InputField>() { player1InputField, player2InputField, player3InputField, player4InputField };
        addPlayerButton.onClick.AddListener(handleAddPlayerButtonClick);
        removePlayerButton.onClick.AddListener(handleRemovePlayerButtonClick);
        letsStartButton.onClick.AddListener(handleLetsStartButtonClick);

        foreach(InputField inputField in playerInputFields)
        {
            playerNames.Add(inputField.text);
        }
    }

    void Update()
    {
        if (!choosingFinished)
        {
            manageButtonsVisibility();

            for (int i = 0; i < numberOfInputFields; i++)
            {
                playerNames[i] = playerInputFields[i].text;
            }
        }
    }

    void handleAddPlayerButtonClick()
    {
        playerInputFields.First(x => !x.gameObject.activeSelf).gameObject.SetActive(true);
    }

    void handleRemovePlayerButtonClick()
    {
        List<InputField> possibleToRemoveFrom = playerInputFields.Where(x => !x.Equals(playerInputFields.ElementAt(0)) && !x.Equals(playerInputFields.ElementAt(1))).ToList();
        possibleToRemoveFrom.Last(x => x.gameObject.activeSelf).gameObject.SetActive(false);      
    }

    void handleLetsStartButtonClick()
    {
        choosingFinished = true;
        removeEmptyElements(playerNames);
        PlayerInitializer.SetPlayerNames(playerNames);
        choosingPlayersMenuCanvas.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void removeEmptyElements(List<string> playerNames)
    {
        int howMany = playerNames.Count;
        for (int i = howMany - 1; i >= 0; i--)
        {
            if (playerNames[i].Length == 0) playerNames.RemoveAt(i);
        }
    }

    // Start is possible when at least 2 players have a name
    bool checkIfStartIsPossible()
    {
        int playersCounter = 0;
        foreach(InputField inputField in playerInputFields)
        {
            if (inputField.text.Length != 0) playersCounter++;
        }
        return (playersCounter > 1) ? true : false;
    }

    bool shouldAddPlayerButtonBeInteractable()
    {
        int activeFieldsCounter = 0;
        foreach (InputField inputField in playerInputFields)
        {
            if (inputField.gameObject.activeSelf) activeFieldsCounter++;
        }
        return (activeFieldsCounter < numberOfInputFields) ? true : false;
    }

    bool shouldRemovePlayerButtonBeActive()
    {
        int activeFieldsCounter = 0;
        foreach (InputField inputField in playerInputFields)
        {
            if (inputField.gameObject.activeSelf) activeFieldsCounter++;
        }
        return (activeFieldsCounter > 2) ? true : false;
    }

    void manageButtonsVisibility()
    {
        // Let's start button interactability
        if (checkIfStartIsPossible() && !letsStartButton.interactable) letsStartButton.interactable = true;
        else if (!checkIfStartIsPossible() && letsStartButton.interactable) letsStartButton.interactable = false;

        // Add player button interactability
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
