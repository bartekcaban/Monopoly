using UnityEngine;
using System;


public class Property : MonoBehaviour
{
    public int id;
    public PropertyType type;
    public PropertyGroup group;
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
    GameObject[] houses;

    void onMouseDown()
    {
        BuildHouse();

        Debug.Log("jestem");
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
    utility
}
public struct PropertyGroup
{
    PropertyGroupName name;
    int numberofProperties;
}
