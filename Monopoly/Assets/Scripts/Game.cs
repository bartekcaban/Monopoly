using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    
    CameraMovement camera;
    List<Player> players;
    DialogMenu dialogMenu;
    public List<Property> properties;
    int numberOfTurns;
    int numberOfPlayers;
    int currentPlayerIndex;
    bool start;
    bool moveFinished = true;
    float timeLeft;
    const int gameBoardSize = 40;
    int currentPlayerId;
    bool currentPlayerBoughtProperty = false;
    bool currentPlayerIsMakingDecision = false;
    Property currentPlayerStandingProperty;

    public void SetNumberOfPlayers(int number)
    {
        numberOfPlayers = number;

        if ( number < 2)
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

        if(number == 4)
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
        numberOfTurns = 1;
        players = new List<Player>();
        propertiesInit();
        camera = (CameraMovement)GameObject.Find("Main Camera").GetComponent(typeof(CameraMovement));
        currentPlayerIndex = 0;
        start = false;
        timeLeft = 8.0f;
        SetNumberOfPlayers(2);
        dialogMenu = DialogMenu.Instance();
      
    }

    // Update is called once per frame
    void Update()
    {
      
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
