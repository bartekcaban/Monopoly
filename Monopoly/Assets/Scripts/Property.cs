using UnityEngine;
using System;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class Property : MonoBehaviour
{
    public string propertyName;
    public int id;
    public PropertyType type;
    public PropertyGroupName groupName;
    String Name { get; set; }
    int? ownerId { get; set; }
    public int numberOfHouses; 
    public int price;
    public int housePrice;
    public int hotelPrice;
    public int rent;
    public int rentPerHouse;
    bool hotelBuilt;
    bool ableToBuild;
    bool deposit = false;
    public int hotelRent;
    public GameObject hotelPrefab;
    public GameObject[] housesPrefabs;
    public GameObject soldSignPrefab;
    public GameObject constructionSitePrefab;
    GameObject soldSign;
    GameObject constructionSite;
    GameObject house;
    GameObject hotel;

    public void SetPropertyData(String name, int price, int housePrice, int rent, int rentPerHouse,
        int hotelRent,PropertyGroupName groupName,PropertyType type)
    {
        Name = name;
        this.propertyName = name;
        this.price = price;
        this.housePrice = housePrice;
        this.rent = rent;
        this.rentPerHouse = rentPerHouse;
        this.hotelRent = hotelRent;
        this.groupName = groupName;
        this.type = type;
        this.numberOfHouses = 0;
        
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
            var rotation = transform.rotation * Quaternion.Euler(calculateSignRotation(this.id));
            position += calculateSignOffset(this.id);
            soldSign = Instantiate(soldSignPrefab, position, rotation);
         }
        
    }
    public void ChangeOwner(int ownerId)
    {
        this.ownerId = ownerId;
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
    public bool IsAbleToBuild()
    {
        if (deposit)
            return false;
        return ableToBuild;
    }
    public bool IsDeposited()
    {
        return deposit;
    }
    public void SetDeposit(bool state)
    {
        deposit = state;
    }
    public bool HasHotel()
    {
        return hotelBuilt;
    }
   public void BuildHouse()
    {
        if(numberOfHouses < 4 && ableToBuild)
        {
            var rotation = transform.rotation * Quaternion.Euler(calculateBuildingRotation(this.id));
            if (house)
            {
                Destroy(house);
            }
            var position = transform.position + calculateBuildingOffset(this.id);
            
            house =Instantiate(housesPrefabs[numberOfHouses],position,rotation);
            house.transform.localScale = new Vector3(10, 10, 10);
            Destroy(constructionSite);
            numberOfHouses++;
            
        }
        else if(numberOfHouses == 4)
        {
            BuildHotel();
        }
    }
    public void BuildHotel()
    {
        if (hotelBuilt == false)
        {
            Destroy(house);
            var rotation = transform.rotation * Quaternion.Euler(calculateBuildingRotation(this.id));
            var position = transform.position + calculateBuildingOffset(this.id);
            hotel = Instantiate(hotelPrefab, position, rotation);
            hotel.transform.localScale = new Vector3(10, 10, 10);
            hotelBuilt = true;
        }
    }
    public int GetRent()
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
        ableToBuild = true;
        var position = transform.position;
        position += calculateConstructionOffset(this.id);
        Destroy(soldSign);
        constructionSite = Instantiate(constructionSitePrefab, position, UnityEngine.Quaternion.identity);
    }

    public int GetOwnerId()
    {
        return (int)ownerId;
    }

    Vector3 calculateSignOffset(int fieldId)
    {
        if (fieldId < 11) return new Vector3(4.5f, 0,0);
        else if (fieldId < 21) return new Vector3(0, 0, -4.5f);
        else if (fieldId < 31) return new Vector3(-7, 0, 1.5f);
        else   return new Vector3(0, 0, 5);
    }
    Vector3 calculateSignRotation(int fieldId)
    {
        if (fieldId < 11) return new Vector3(0f, -90f, 0f);
        else if (fieldId < 21) return new Vector3(0f, -90f, 180f);
        else if (fieldId < 31) return new Vector3(0f, 90f, 0f);
        else return new Vector3(0f, -90f, 0f);
    }
    Vector3 calculateBuildingOffset(int fieldId)
    {
        if (fieldId < 11) return new Vector3(5.5f, 0, -0.5f);
        else if (fieldId < 21) return new Vector3(-0.1f, 0, -6.5f);
        else if (fieldId < 31) return new Vector3(-5.5f, 0, 0.2f);
        else return new Vector3(0.5f, 0, 5.5f);
    }
    Vector3 calculateConstructionOffset(int fieldId)
    {
        if (fieldId < 11) return new Vector3(6f, 0, 0.0f);
        else if (fieldId < 21) return new Vector3(0f, 0, -6.0f);
        else if (fieldId < 31) return new Vector3(-6.0f, 0, 0.0f);
        else return new Vector3( 0f, 0, 6f);
    }
    Vector3 calculateBuildingRotation(int fieldId)
    {
        if (fieldId < 11) return new Vector3(-90f,180f,0f);
        else if (fieldId < 21) return new Vector3(90f, 0f, 0f);
        else if (fieldId < 31) return new Vector3(-90f, 180f, 0f);
        else return new Vector3(-90f, 0f, 0f);
    }

}

public enum PropertyType
{
    forSale,
    startField,
    chance,
    start,
    jail,
    goToJail,
    tax,
    parking
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
    other
   
}

