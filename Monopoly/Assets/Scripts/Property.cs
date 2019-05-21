﻿using UnityEngine;
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
        this.propertyName = name;
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
            position += calculateSignOffset(this.id);
            soldSign = Instantiate(soldSignPrefab, position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
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
            houses[numberOfHouses]=Instantiate(housePrefab,position, UnityEngine.Quaternion.identity);
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
        position += calculateConstructionOffset(this.id);
        Destroy(soldSign);
        constructionSite = Instantiate(constructionSitePrefab, position, UnityEngine.Quaternion.identity);
    }
    Vector3 calculateSignOffset(int fieldId)
    {
        if (fieldId < 11) return new Vector3(7, 0, 0);
        else if (fieldId < 21) return new Vector3(2, 0, -7);
        else if (fieldId < 31) return new Vector3(-7, 0, 2);
        else   return new Vector3(0, 0, 7);
    }
    Vector3 calculateConstructionOffset(int fieldId)
    {
        if (fieldId < 11) return new Vector3(6, 0, 0);
        else if (fieldId < 21) return new Vector3(0, 0, -6);
        else if (fieldId < 31) return new Vector3(-6, 0, 0);
        else return new Vector3(0, 0, 6);
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

