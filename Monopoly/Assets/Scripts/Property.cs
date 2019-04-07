using UnityEngine;
using System;


public class Property : MonoBehaviour
{
    public int id;
    String Name { get; set; }
    public int CountryId { get; set; }
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

    public void SetPropertyData(String name, int price, int housePrice, int rent, int rentPerHouse, int hotelRent)
    {
        Name = name;
        this.price = price;
        this.housePrice = housePrice;
        this.rent = rent;
        this.rentPerHouse = rentPerHouse;
        this.hotelRent = hotelRent;
    }

    public void SetId(int id)
    {
        this.id = id; 
    }
    
    void BuyProperty(int ownerId)
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
