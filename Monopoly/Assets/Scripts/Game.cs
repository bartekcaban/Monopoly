using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    const int gameBoardSize = 40;

    CameraMovement camera;

    PlayerInitializer playerInitializer;
    List<string> playerNames;
    public List<Player> players;
    int numberOfPlayers;
    public Player currentPlayer;
    public Player nextPlayer;
    int currentPlayerIndex;
    int nextPlayerIndex;
    int currentPlayerId;

    PropertiesInitializer propertiesInitializer;
    Dictionary<PropertyGroupName,int> propertyGroups;
    public List<Property> properties;
    Property currentPlayerStandingProperty;
    List<Chance> chanceList;

    public MoneyManager moneyManager;
    public GameUI gameUIManager;

    DialogMenu dialogMenu;
    InfoPopup infoPopup;

    bool start;
    bool moveFinished = true;
    float timeLeft;
    int numberOfTurns;
    bool currentPlayerBoughtProperty = false;
    bool currentPlayerIsMakingDecision = false;
    bool fieldHandled = false;

    // Start is called before the first frame update
    void Start()
    {
        propertiesInitializer = new PropertiesInitializer();
        properties = propertiesInitializer.InitializeProperties(gameBoardSize);
        propertyGroups = propertiesInitializer.GetPropertyGroups();

        playerInitializer = new PlayerInitializer();
        playerNames = playerInitializer.GetPlayerNames();
        numberOfPlayers = playerNames.Count;
        players = playerInitializer.CreatePlayers(numberOfPlayers);
        
        numberOfTurns = 1;
        camera = (CameraMovement)GameObject.Find("Main Camera").GetComponent(typeof(CameraMovement));
        currentPlayerIndex = 0;
        start = false;
        timeLeft = 8.0f;
        moneyManager = new MoneyManager(players);
        gameUIManager.gameObject.SetActive(true);
        dialogMenu = DialogMenu.Instance();
        infoPopup = InfoPopup.Instance();
        ChanceInit();

        foreach(Player player in players)
        {
            Debug.Log("player: " + player.playerName);
        }
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
            if (!players[currentPlayerIndex].IsMoving() && !currentPlayerIsMakingDecision && !infoPopup.active)
            {
                if (players[currentPlayerIndex].CanMove() )
                {
                players[currentPlayerIndex].AllowRolling();
                camera.SetDiceCamera();
                timeLeft = 1.0f;
                moveFinished = false;
                currentPlayerBoughtProperty = false;
                }
                else
                {
                    infoPopup.ShowMessage("Więzienie", "Czekasz jeszcze " + players[currentPlayerIndex].ReturnTurnsPausing() + " tury");
                    players[currentPlayerIndex].PauseOneTurn();
                    players[currentPlayerIndex].SetMoveFinished();
                    currentPlayerIndex++;
                    if (currentPlayerIndex == numberOfPlayers)
                    {
                        currentPlayerIndex = 0;
                        numberOfTurns++;
                    }
                }
            }

            if (players[currentPlayerIndex].DiceRolled() && !currentPlayerIsMakingDecision)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft < 0)
                {
                    if(!players[currentPlayerIndex].AllowMovement())
                    {
                        GetStartMoney();
                    }
                    camera.SetPawnFollowing(players[currentPlayerIndex].transform.position);
                }
                else
                    camera.SetPawnCamera(players[currentPlayerIndex].transform.position);
            }

            if (players[currentPlayerIndex].PawnMoved() && !currentPlayerIsMakingDecision && !players[currentPlayerIndex].MoveFinished())
            {

                int currentPlayerPosition = players[currentPlayerIndex].GetCurrentPosition();
                currentPlayerId = players[currentPlayerIndex].GetId();
                currentPlayerStandingProperty = properties[currentPlayerPosition];
                
                switch (currentPlayerStandingProperty.type)
                {
                    case PropertyType.forSale:
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
                     
                       
                    break;
                    case PropertyType.chance:
                        PerformChanceAction(DrawAChance());
                      
                        break;
                    case PropertyType.start:
                        GetStartMoney();
                       
                        break;
                    case PropertyType.goToJail:
                        SetJail();
                     
                        break;
                    case PropertyType.parking:
                        
                        break;
                    case PropertyType.jail:
                      
                        break;
                    case PropertyType.tax:
                        PayTax();
                      
                        break;
                        

                }
                currentPlayerIsMakingDecision = true;
            }

            if (currentPlayerIsMakingDecision)
            {
                if (currentPlayerBoughtProperty)
                {
                    currentPlayer.Buy(currentPlayerStandingProperty);
                    currentPlayerStandingProperty.Buy(currentPlayerId);
                    infoPopup.BoughtPropertyInfo(currentPlayerStandingProperty.propertyName);
                    if (currentPlayerStandingProperty.groupName != PropertyGroupName.station && 
                        currentPlayer.IsOwnerOfWholeGroup(currentPlayerStandingProperty.groupName,propertyGroups))
                    {
                        setPropertyGroupAbleToBuild(currentPlayerStandingProperty.groupName);
                    }
                    currentPlayerBoughtProperty = false;


                    
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

    public void finishTurn()
    {
        moveFinished = true;
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

        dialogMenu.ShowAbleToBuy(property,playerBoughtCurrentProperty, () => {  });
        

    }
    void playerBoughtCurrentProperty()
    {
        currentPlayerBoughtProperty = true;

    }

    void setPropertyGroupAbleToBuild(PropertyGroupName name)
    {
        var properties = this.properties.FindAll(prop => prop.groupName == name);
        properties.ForEach(prop => prop.onAbleToBuild());
    }

    void ChanceInit()
    {
        chanceList = new List<Chance>();
        chanceList.Add(new Chance("Płacisz do banku 50$", -50));
        chanceList.Add(new Chance("Kara za przekroczenie prędkości 10$", -10));
        chanceList.Add(new Chance("Zaległy podatek 200$", -200));
        chanceList.Add(new Chance("Nagroda za znalezienie psa 20$", 20));
        chanceList.Add(new Chance("Nagroda za płacenie podatków na czas 100$", 100));
        chanceList.Add(new Chance("Idziesz do dentysty -koszt 70$", -70));
        chanceList.Add(new Chance("Kupujesz prezent z okazji dnia matki - 40$", -40));
        chanceList.Add(new Chance("Znajdujesz na ulicy banknot 50$", 50));
    }

    Chance DrawAChance()
    {
        int size = chanceList.Count;
        return chanceList[Random.Range(0, size)];
    }

    void PerformChanceAction( Chance chance )
    {
        moneyManager.DepositOnAccount(currentPlayer, chance.ReturnValue());
        infoPopup.ShowMessage("Szansa", chance.ReturnDescription());
    }

    void PayTax()
    {
        moneyManager.WithdrawFromAccount(currentPlayer, 200);
        infoPopup.ShowMessage("Podatek", "Płacisz 200$ podatku");
    }

    void GetStartMoney()
    {
        moneyManager.DepositOnAccount(currentPlayer, 200);
        infoPopup.ShowMessage("Start", "Przechodzisz przez pole start, dostajesz 200$");
    }

    void SetJail()
    {
        currentPlayer.GoToJail();
        infoPopup.ShowMessage("Idziesz do więzienia", "Będziesz pauzował 3 tury");
    }
}
