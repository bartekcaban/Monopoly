using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{    
    CameraMovement camera;
    List<string> playerNames;
    public List<Player> players;
    Dictionary<PropertyGroupName,int> propertyGroups;

    DialogMenu dialogMenu;
    InfoPopup infoPopup;
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
    bool fieldHandled = false;
    Property currentPlayerStandingProperty;
    List<Chance> chanceList;

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
            players[i].SetId(i);
        }
    }

    void propertiesInit()
    {
        for(int i = 0; i < gameBoardSize; i++)
        {
            properties.Add((Property)GameObject.Find("Tile" + i).GetComponent(typeof(Property)));
            properties[i].SetId(i);
        }

        propertyGroups = new Dictionary<PropertyGroupName, int>
        {
            {PropertyGroupName.brown,2 },
            {PropertyGroupName.red,3 },
            {PropertyGroupName.yellow,3 },
            {PropertyGroupName.lightBlue,3 },
            {PropertyGroupName.darkBlue,3 },
            {PropertyGroupName.green,3 },
            {PropertyGroupName.orange,3 },
            {PropertyGroupName.pink,2 },
            {PropertyGroupName.utility, 2},
            {PropertyGroupName.other, 12}
        };

        //Hotels:
        properties[1].SetPropertyData("Salzburg", 60, 50, 2, 40, 250,PropertyGroupName.brown,PropertyType.forSale);
        properties[3].SetPropertyData("Vienna", 60, 50, 4, 80, 450, PropertyGroupName.brown, PropertyType.forSale);

        properties[6].SetPropertyData("Cracow", 100, 50, 6, 100, 550, PropertyGroupName.red, PropertyType.forSale);
        properties[8].SetPropertyData("Warsaw", 100, 50, 6, 100, 550, PropertyGroupName.red, PropertyType.forSale);
        properties[9].SetPropertyData("Gliwice", 120, 50, 8, 110, 600, PropertyGroupName.red, PropertyType.forSale);

        properties[11].SetPropertyData("Sevilla", 140, 100, 10, 150, 750, PropertyGroupName.yellow, PropertyType.forSale);
        properties[13].SetPropertyData("Barcelona", 140, 100, 10, 150, 750, PropertyGroupName.yellow, PropertyType.forSale);
        properties[14].SetPropertyData("Madrid", 160, 100, 12, 175, 900, PropertyGroupName.yellow, PropertyType.forSale);

        properties[16].SetPropertyData("Manchester", 180, 100, 14, 185, 950, PropertyGroupName.lightBlue, PropertyType.forSale);
        properties[18].SetPropertyData("Birmingham", 180, 100, 14, 185, 950, PropertyGroupName.lightBlue, PropertyType.forSale);
        properties[19].SetPropertyData("London", 200, 100, 16, 200, 1000, PropertyGroupName.lightBlue, PropertyType.forSale);

        properties[21].SetPropertyData("Toulouse", 220, 150, 18, 215, 1050, PropertyGroupName.darkBlue, PropertyType.forSale);
        properties[23].SetPropertyData("Marseilles", 220, 150, 18, 215, 1050, PropertyGroupName.darkBlue, PropertyType.forSale);
        properties[24].SetPropertyData("Paris", 240, 150, 20, 230, 1100, PropertyGroupName.darkBlue, PropertyType.forSale);


        properties[26].SetPropertyData("Milan", 260, 150, 22, 245, 1150, PropertyGroupName.green, PropertyType.forSale);
        properties[27].SetPropertyData("Florence", 260, 150, 22, 245, 1150, PropertyGroupName.green, PropertyType.forSale);
        properties[29].SetPropertyData("Rome", 280, 150, 24, 255, 1200, PropertyGroupName.green, PropertyType.forSale);

        properties[31].SetPropertyData("Munich", 300, 200, 26, 275, 1275, PropertyGroupName.orange, PropertyType.forSale);
        properties[33].SetPropertyData("Frankfurt", 300, 200, 26, 275, 1275, PropertyGroupName.orange, PropertyType.forSale);
        properties[34].SetPropertyData("Berlin", 320, 200, 28, 300, 1400, PropertyGroupName.orange, PropertyType.forSale);

        properties[37].SetPropertyData("Charleroi", 350, 200, 35, 325, 1500, PropertyGroupName.pink, PropertyType.forSale);
        properties[39].SetPropertyData("Brussels", 400, 200, 50, 425, 2000, PropertyGroupName.pink, PropertyType.forSale);

        //Others: - different payment rules
        properties[5].SetPropertyData("WestRailroad", 200, 0, 50, 0, 0,PropertyGroupName.station,PropertyType.forSale);
        properties[15].SetPropertyData("NorthRailroad", 200, 0, 50, 0, 0, PropertyGroupName.station, PropertyType.forSale);
        properties[25].SetPropertyData("EastRailroad", 200, 0, 50, 0, 0, PropertyGroupName.station, PropertyType.forSale);
        properties[35].SetPropertyData("SouthRailroad", 200, 0, 50, 0, 0, PropertyGroupName.station, PropertyType.forSale);

        properties[12].SetPropertyData("ElectricCompany", 150, 0, 50, 0, 0, PropertyGroupName.utility, PropertyType.forSale);
        properties[28].SetPropertyData("WaterWorks", 150, 0, 50, 0, 0, PropertyGroupName.utility, PropertyType.forSale);

        //Special fields:
        properties[0].SetPropertyData("Start", 0, 0, 0, 0, 0, PropertyGroupName.other, PropertyType.start);
        properties[10].SetPropertyData("Jail", 0, 0, 0, 0, 0,PropertyGroupName.other, PropertyType.jail);
        properties[20].SetPropertyData("Parking", 0, 0, 0, 0, 0,PropertyGroupName.other, PropertyType.parking);
        properties[30].SetPropertyData("GoToJail", 0, 0, 0, 0, 0, PropertyGroupName.other, PropertyType.goToJail);

        properties[4].SetPropertyData("IncomeTax", 0, 0, 0, 0, 0, PropertyGroupName.other, PropertyType.tax);
        properties[38].SetPropertyData("LuxuryTax", 0, 0, 0, 0, 0, PropertyGroupName.other, PropertyType.tax);

        //Chance fields
        properties[2].SetPropertyData("Chance", 0, 0, 0, 0, 0, PropertyGroupName.other, PropertyType.chance);
        properties[7].SetPropertyData("Chance", 0, 0, 0, 0, 0, PropertyGroupName.other, PropertyType.chance);
        properties[17].SetPropertyData("Chance", 0, 0, 0, 0, 0, PropertyGroupName.other, PropertyType.chance);
        properties[22].SetPropertyData("Chance", 0, 0, 0, 0, 0, PropertyGroupName.other, PropertyType.chance);
        properties[33].SetPropertyData("Chance", 0, 0, 0, 0, 0, PropertyGroupName.other, PropertyType.chance);
        properties[36].SetPropertyData("Chance", 0, 0, 0, 0, 0, PropertyGroupName.other, PropertyType.chance);

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

        infoPopup = InfoPopup.Instance();
        ChanceInit();
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
                   


                    infoPopup.BoughtPropertyInfo(currentPlayerStandingProperty.name);
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
        currentPlayer.cash += chance.ReturnValue();
        infoPopup.ShowMessage("Szansa", chance.ReturnDescription());
    }

    void PayTax()
    {
        currentPlayer.cash -= 200;
        infoPopup.ShowMessage("Podatek", "Płacisz 200$ podatku");
    }

    void GetStartMoney()
    {
        currentPlayer.cash += 200;
        infoPopup.ShowMessage("Start", "Przechodzisz przez pole start, dostajesz 200$");
    }

    void SetJail()
    {
        currentPlayer.GoToJail();
        infoPopup.ShowMessage("Idziesz do więzienia", "Będziesz pauzował 3 tury");
    }
}
