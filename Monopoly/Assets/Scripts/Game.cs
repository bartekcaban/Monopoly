﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{    
    CameraMovement camera;
    List<string> playerNames;
    public List<Player> players;
    DialogMenu dialogMenu;
    public List<Property> properties;
    int numberOfTurns;
    int numberOfPlayers;
    public Player currentPlayer;
    public Player nextPlayer;
    int currentPlayerIndex;
    int nextPlayerIndex;
    bool start;
    bool moveFinished = true;
    float timeLeft;
    const int gameBoardSize = 40;
    int currentPlayerId;
    bool currentPlayerBoughtProperty = false;
    bool currentPlayerIsMakingDecision = false;
    Property currentPlayerStandingProperty;

    public void CreatePlayers(int number)
    {        
        players.Add((Player)GameObject.Find("Cat").GetComponent(typeof(Player)));
        players.Add((Player)GameObject.Find("Teapot").GetComponent(typeof(Player)));

        if (number < 2) number = 2;
        if (number > 4) number = 4;

        if (number == 2)
        {
            ((Player)GameObject.Find("Dog").GetComponent(typeof(Player)) as Player).Disable();
            ((Player)GameObject.Find("Hat").GetComponent(typeof(Player)) as Player).Disable();
        }

        else if (number == 3)
        {            
            players.Add((Player)GameObject.Find("Dog").GetComponent(typeof(Player)));
            ((Player)GameObject.Find("Hat").GetComponent(typeof(Player)) as Player).Disable();
        }

        else if (number == 4)
        {            
            players.Add((Player)GameObject.Find("Dog").GetComponent(typeof(Player)));
            players.Add((Player)GameObject.Find("Hat").GetComponent(typeof(Player)));
        }

        for (int i = 0; i < number; i++)
        {
            players[i].playerName = playerNames[i];
        }
    }

    void propertiesInit()
    {
        for(int i = 0; i < gameBoardSize; i++)
        {
            properties.Add((Property)GameObject.Find("Tile" + i).GetComponent(typeof(Property)));
            properties[i].SetId(i);
        }

        //Hotels:
        properties[1].SetPropertyData("Salzburg", 60, 50, 2, 40, 250);
        properties[3].SetPropertyData("Vienna", 60, 50, 4, 80, 450);

        properties[6].SetPropertyData("Cracow", 100, 50, 6, 100, 550);
        properties[8].SetPropertyData("Warsaw", 100, 50, 6, 100, 550);
        properties[9].SetPropertyData("Gliwice", 120, 50, 8, 110, 600);

        properties[11].SetPropertyData("Sevilla", 140, 100, 10, 150, 750);
        properties[13].SetPropertyData("Barcelona", 140, 100, 10, 150, 750);
        properties[14].SetPropertyData("Madrid", 160, 100, 12, 175, 900);

        properties[16].SetPropertyData("Manchester", 180, 100, 14, 185, 950);
        properties[18].SetPropertyData("Birmingham", 180, 100, 14, 185, 950);
        properties[19].SetPropertyData("London", 200, 100, 16, 200, 1000);

        properties[21].SetPropertyData("Toulouse", 220, 150, 18, 215, 1050);
        properties[23].SetPropertyData("Marseilles", 220, 150, 18, 215, 1050);
        properties[24].SetPropertyData("Paris", 240, 150, 20, 230, 1100);

        properties[26].SetPropertyData("Milan", 260, 150, 22, 245, 1150);
        properties[28].SetPropertyData("Florence", 260, 150, 22, 245, 1150);
        properties[29].SetPropertyData("Rome", 280, 150, 24, 255, 1200);

        properties[31].SetPropertyData("Munich", 300, 200, 26, 275, 1275);
        properties[33].SetPropertyData("Frankfurt", 300, 200, 26, 275, 1275);
        properties[34].SetPropertyData("Berlin", 320, 200, 28, 300, 1400);

        properties[37].SetPropertyData("Charleroi", 350, 200, 35, 325, 1500);
        properties[39].SetPropertyData("Brussels", 400, 200, 50, 425, 2000);

        //Others: - different payment rules
        properties[5].SetPropertyData("WestRailroad", 200, 0, 50, 0, 0);
        properties[15].SetPropertyData("NorthRailroad", 200, 0, 50, 0, 0);
        properties[25].SetPropertyData("EastRailroad", 200, 0, 50, 0, 0);
        properties[35].SetPropertyData("SouthRailroad", 200, 0, 50, 0, 0);

        properties[12].SetPropertyData("ElectricCompany", 150, 0, 50, 0, 0);
        properties[28].SetPropertyData("WaterWorks", 150, 0, 50, 0, 0);

        //Special fields:
        properties[0].SetPropertyData("Start", 0, 0, 0, 0, 0);
        properties[10].SetPropertyData("Jail", 0, 0, 0, 0, 0);
        properties[20].SetPropertyData("Parking", 0, 0, 0, 0, 0);
        properties[30].SetPropertyData("GoToJail", 0, 0, 0, 0, 0);

        properties[4].SetPropertyData("IncomeTax", 0, 0, 0, 0, 0);
        properties[38].SetPropertyData("LuxuryTax", 0, 0, 0, 0, 0);

        properties[2].SetPropertyData("Chance", 0, 0, 0, 0, 0);
        properties[7].SetPropertyData("Chance", 0, 0, 0, 0, 0);
        properties[17].SetPropertyData("Chance", 0, 0, 0, 0, 0);
        properties[22].SetPropertyData("Chance", 0, 0, 0, 0, 0);
        properties[33].SetPropertyData("Chance", 0, 0, 0, 0, 0);
        properties[36].SetPropertyData("Chance", 0, 0, 0, 0, 0);

    }

    // Start is called before the first frame update
    void Start()
    {
        playerNames = PlayerInfo.PlayerNames;
        numberOfPlayers = playerNames.Count;
        numberOfTurns = 1;
        players = new List<Player>();
        propertiesInit();
        camera = (CameraMovement)GameObject.Find("Main Camera").GetComponent(typeof(CameraMovement));
        currentPlayerIndex = 0;
        start = false;
        timeLeft = 8.0f;
        CreatePlayers(numberOfPlayers);
        dialogMenu = DialogMenu.Instance();
    }

    // Update is called once per frame
    void Update()
    {

        nextPlayerIndex = calculateNextPlayerIndex(currentPlayerIndex);
        currentPlayer = players[currentPlayerIndex];
        nextPlayer = players[nextPlayerIndex];

        if (!start)
        {
            //camera.SetCircumnavigation();
            //timeLeft -= Time.deltaTime;
            //if (timeLeft < 0)
            start = true;
        }
        else
        {
            if (!players[currentPlayerIndex].IsMoving() && !currentPlayerIsMakingDecision)
            {
                players[currentPlayerIndex].AllowRolling();
                camera.SetDiceCamera();
                timeLeft = 1.0f;
                moveFinished = false;
                currentPlayerBoughtProperty = false;
            }

            if (players[currentPlayerIndex].DiceRolled() && !currentPlayerIsMakingDecision)
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

            if (players[currentPlayerIndex].PawnMoved() && !currentPlayerIsMakingDecision)
            {

                int currentPlayerPosition = players[currentPlayerIndex].GetCurrentPosition();
                currentPlayerId = players[currentPlayerIndex].GetId();
                currentPlayerStandingProperty = properties[currentPlayerPosition];
                


                if (currentPlayerStandingProperty.HasOwner())
                {
                    if (currentPlayerStandingProperty.IsOwningBy(currentPlayerId))
                    {
                        HandleStandingOnOwnPosition(currentPlayerStandingProperty);
                    }
                    else
                    {
                        HandleRentPay(currentPlayerStandingProperty, currentPlayerId);
                    }
                }
                else
                {

                  HandleAbleToBuyProperty(currentPlayerStandingProperty, currentPlayerId);
                }


               
                currentPlayerIsMakingDecision = true;
            }
            if (currentPlayerIsMakingDecision)
            {
                if (currentPlayerBoughtProperty)
                {
                    currentPlayerStandingProperty.Buy(currentPlayerId);
                }
                if (moveFinished)
                {
                    players[currentPlayerIndex].SetMoveFinished();
                    currentPlayerIsMakingDecision = false;
                    currentPlayerIndex++;
                }
                if (currentPlayerIndex == numberOfPlayers)
                {
                    Debug.Log(currentPlayerIndex);

                    currentPlayerIndex = 0;
                    numberOfTurns++;
                }
            }
        }
    }

    int calculateNextPlayerIndex(int actualIndex)
    {   
        nextPlayerIndex = (actualIndex == numberOfPlayers) ? 0 : ++actualIndex;
        return (nextPlayerIndex == numberOfPlayers) ? 0 : nextPlayerIndex;        
    }

    void HandleRentPay(Property property, int payingPlayerId)
    {
        dialogMenu.ShowForRentPayment(property);

    }
    void HandleStandingOnOwnPosition(Property property)
    {
        dialogMenu.ShowForPropertyOwner(property);
    }
    void HandleAbleToBuyProperty(Property property, int playerId)
    {

        dialogMenu.ShowAbleToBuy(property,playerBoughtCurrentProperty, () => { moveFinished = true; });
        

    }
    void playerBoughtCurrentProperty()
    {
        currentPlayerBoughtProperty = true;
    }
    
}
