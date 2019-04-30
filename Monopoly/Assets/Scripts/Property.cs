using UnityEngine;
using System;


public class Property : MonoBehaviour
{
    public int id;
    public PropertyType type;
    public PropertyGroupName groupName;
    String Name { get; set; }
    int? ownerId { get; set; }
    public int numberOfHouses
    { get; set; }
    public int price;
    public int housePrice;
    public int hotelPrice;
    public int rent;
    public int rentPerHouse;
    bool hotelBuilt;
    public int hotelRent;
    public GameObject housePrefab;
    public GameObject soldSignPrefab;
    public GameObject constructionSitePrefab;
    GameObject soldSign;
    GameObject constructionSite;
    GameObject[] houses;

    

    public void SetPropertyData(String name, int price, int housePrice, int rent, int rentPerHouse,
        int hotelRent,PropertyGroupName groupName,PropertyType type)
    {
        Name = name;
        this.price = price;
        this.housePrice = housePrice;
        this.rent = rent;
        this.rentPerHouse = rentPerHouse;
        this.hotelRent = hotelRent;
        this.groupName = groupName;
        this.type = type;
    }

    public void SetId(int id)
    {
        this.id = id; 
    }
    
    public void Buy(int ownerId)
    {
        if (this.ownerId==null)
        {
            this.ownerId = ownerId;
            var position = transform.position;
            position.x += 7;
            position.z -= 2;
            soldSign = Instantiate(soldSignPrefab, position, Quaternion.identity);
         }
        
    }
    public bool HasOwner()
    {
        if (ownerId == null) return false;
        else return true;
    }
    public bool IsOwningBy(int PlayerId)
    {
        if (PlayerId == ownerId) return true;
        else return false;
    }
    void BuildHouse()
    {
        if(numberOfHouses < 4)
        {
            var position = transform.position;
            houses[numberOfHouses]=Instantiate(housePrefab,position, Quaternion.identity);
            Destroy(soldSign);
            numberOfHouses++;
        }
    }
    void BuildHotel()
    {
        if (numberOfHouses == 4)
        {
            hotelBuilt = true;
        }
    }
    int GetRent()
    {
        if (hotelBuilt)
        {
            return rent + hotelRent;
        }
        else
        {
            return rent + numberOfHouses * rentPerHouse;
        }
    }
    public void onAbleToBuild()
    {
        var position = transform.position;
        position.x += 7;
        position.z -= 2;
        Destroy(soldSign);
        constructionSite = Instantiate(constructionSitePrefab, position, Quaternion.identity);
    }
}

public enum PropertyType
{
    forSale,
    startField,
    special
}
public enum PropertyGroupName
{
    brown,
    lightBlue,
    pink,
    orange,
    red,
    yellow,
    green,
    darkBlue,
    station,
    utility,
    tax,
    chance,
    other
}

