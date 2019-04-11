using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    CameraMovement camera;
    List<string> playerNames;
    List<Player> players;    
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
            if (!players[currentPlayerIndex].IsMoving()&& !currentPlayerIsMakingDecision)
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
