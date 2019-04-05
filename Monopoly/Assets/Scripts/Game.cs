using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    string[] characters = { "Cat", "Teapot", "Dog", "Hat" };
    public ChoosingPlayersMenu choosingPlayersMenu;
    CameraMovement camera;
    List<Player> players = new List<Player>();
    List<string> playerNames = new List<string>();
    DialogMenu dialogMenu;
    //public ChoosingPlayersMenu choosingPlayersMenu;
    public List<Property> properties;
    int numberOfTurns;
    int numberOfPlayers;
    int currentPlayerIndex;
    Player currentPlayer;
    bool start;
    float timeLeft;
    const int gameBoardSize = 42;

    public void SetNumberOfPlayers(int number)
    {
        numberOfPlayers = number;

        if (number < 2)
            number = 2;
        if (number > 4)
            number = 4;

        if (number == 2)
        {
            players.Add((Player)GameObject.Find("Cat").GetComponent(typeof(Player)));
            players.Add((Player)GameObject.Find("Teapot").GetComponent(typeof(Player)));

            Player p = (Player)GameObject.Find("Dog").GetComponent(typeof(Player));
            p.Disable();
            p = (Player)GameObject.Find("Hat").GetComponent(typeof(Player));
            p.Disable();
        }

        if (number == 3)
        {
            players.Add((Player)GameObject.Find("Cat").GetComponent(typeof(Player)));
            players.Add((Player)GameObject.Find("Teapot").GetComponent(typeof(Player)));
            players.Add((Player)GameObject.Find("Dog").GetComponent(typeof(Player)));

            Player p = (Player)GameObject.Find("Hat").GetComponent(typeof(Player));
            p.Disable();
        }

        if (number == 4)
        {
            players.Add((Player)GameObject.Find("Cat").GetComponent(typeof(Player)));
            players.Add((Player)GameObject.Find("Teapot").GetComponent(typeof(Player)));
            players.Add((Player)GameObject.Find("Dog").GetComponent(typeof(Player)));
            players.Add((Player)GameObject.Find("Hat").GetComponent(typeof(Player)));
        }
    }

    void propertiesInit()
    {
        for(int i = 0; i < gameBoardSize; i++)
        {
            properties.Add((Property)GameObject.Find("Tile" + i).GetComponent(typeof(Property)));
            properties[i].SetId(i);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        choosingPlayersMenu = ChoosingPlayersMenu.Instance();
        while (!choosingPlayersMenu.finishedCreatingPlayers)
        {

        }
        numberOfTurns = 1;
        propertiesInit();
        camera = (CameraMovement)GameObject.Find("Main Camera").GetComponent(typeof(CameraMovement));
        currentPlayerIndex = 0;
        start = false;
        timeLeft = 8.0f;
        managePlayersInformations();
        SetNumberOfPlayers(2);
        dialogMenu = DialogMenu.Instance();
        choosingPlayersMenu = ChoosingPlayersMenu.Instance(); 
    }

    private void managePlayersInformations()
    {
        numberOfPlayers = choosingPlayersMenu.numberOfInputFields;
        playerNames = choosingPlayersMenu.playerNames;
    }
    
    //private void createPlayers()
    //{
    //    for (int i = 0; i < numberOfPlayers; i++)
    //    {
    //        players.Add((Player)GameObject.Find(characters[i]).GetComponent(typeof(Player)));
    //        players[i].setName(playerNames[i]);
    //    }
    //}

    //private Player getCurrentPlayer()
    //{
    //    return players[currentPlayerIndex];
    //}


    void Update()
    {
        if (choosingPlayersMenu.finishedCreatingPlayers)
        {

            //currentPlayer = getCurrentPlayer();
            if (!start)
            {
                //camera.SetCircumnavigation();
                //timeLeft -= Time.deltaTime;
                //if (timeLeft < 0)
                start = true;
            }
            else
            {
                if (!players[currentPlayerIndex].IsMoving())
                {
                    players[currentPlayerIndex].AllowRolling();
                    camera.SetDiceCamera();
                    timeLeft = 1.0f;
                }

                if (players[currentPlayerIndex].DiceRolled())
                {
                    timeLeft -= Time.deltaTime;
                    if (timeLeft < 0)
                    {
                        players[currentPlayerIndex].AllowMovement();
                        camera.SetPawnFollowing(players[currentPlayerIndex].transform.position);
                    }
                    else
                        camera.SetPawnCamera(players[currentPlayerIndex].transform.position);
                }

                if (players[currentPlayerIndex].PawnMoved())
                {
                    /*int currentPlayerPosition = players[currentPlayer].GetCurrentPosition();
                    int currentPlayerId = players[currentPlayer].GetId();
                    Property currentPlayerStandingProperty = properties[currentPlayerPosition];


                    if (currentPlayerStandingProperty.HasOwner())
                    {
                        if (currentPlayerStandingProperty.IsOwningBy(currentPlayerId))
                        {
                            HandleStandingOnOwnPosition();
                        }
                        else
                        {
                            HandleRentPay(currentPlayerStandingProperty, currentPlayerId);
                        }
                    }
                    else
                    {
                      HandleAbleToBuyProperty(currentPlayerStandingProperty, currentPlayerId);
                    }*/

                    currentPlayerIndex++;
                    if (currentPlayerIndex == numberOfPlayers)
                    {
                        currentPlayerIndex = 0;
                        numberOfTurns++;
                    }
                }
            }
        }
    }
    void HandleRentPay(Property property, int payingPlayerId)
    {
        //TODO
    }
    void HandleStandingOnOwnPosition()
    {
        //JUST SHOW POPUP INFO?
    }
    void HandleAbleToBuyProperty(Property property, int playerId)
    {
        //TODO: decision making menu + calls to game logic based on player decision        
    }
}
