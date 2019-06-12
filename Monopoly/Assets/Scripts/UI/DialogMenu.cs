using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogMenu : MonoBehaviour
{

    public GameObject dialogCanvasObject;
    private static DialogMenu dialogMenu;

    //Property Description area
    public Image propertyBackground;
    public Text propertyName;
    public Text propertyPrice;

    public Text perHouseRent;
    public Text basicRent;
    public Text hotelRent;
   
    public Text housePrice;
    public Text hotelPrice;

    //Decision making panel
    public Text decisionDescription;
    
    public Button okButton;
    public Button buyButton;
    public Button expandButton;
    public Button depositButton;



    public static DialogMenu Instance()
    {
        if (!dialogMenu)
        {
           dialogMenu = FindObjectOfType(typeof(DialogMenu)) as DialogMenu;
            if (!dialogMenu)
                Debug.LogError("There is no dialog menu object in scene!");
        }

        return dialogMenu;
    }

    void Close()
    {
        dialogCanvasObject.SetActive(false);
    }

    void DescriptionInit(Property property)
    {
        housePrice.text = property.housePrice.ToString();
        hotelPrice.text = property.hotelPrice.ToString();
        perHouseRent.text = property.rentPerHouse.ToString();
        basicRent.text = property.rent.ToString();
        hotelRent.text = property.hotelRent.ToString();
        propertyPrice.text = property.price.ToString();

        propertyBackground.color = GetpropertyGroupColor(property.groupName);
        propertyName.text = property.propertyName;
        
    }

    public void ShowAbleToBuy(Property property, UnityAction onBuyClicked, UnityAction onOkClicked )
    {
        decisionDescription.text = "Ta nieruchomość nie ma jeszcze właściciela";

        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(onOkClicked);
        okButton.onClick.AddListener(Close);
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(onBuyClicked);

        dialogCanvasObject.SetActive(true);
        okButton.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(true);
        expandButton.gameObject.SetActive(false);
        depositButton.gameObject.SetActive(false);
        DescriptionInit(property);
    }
    public void ShowForPropertyOwner(Property property, UnityAction onExpandClicked, UnityAction onDepositClicked, UnityAction onOkClicked)
    {
        decisionDescription.text = "Jesteś właścicielem tej nieruchomości";

        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(onOkClicked);
        okButton.onClick.AddListener(Close);
        expandButton.onClick.RemoveAllListeners();
        expandButton.onClick.AddListener(onExpandClicked);
        depositButton.onClick.RemoveAllListeners();
        depositButton.onClick.AddListener(onDepositClicked);

        dialogCanvasObject.SetActive(true);
        okButton.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(false);
        expandButton.gameObject.SetActive(true);
        depositButton.gameObject.SetActive(true);
        DescriptionInit(property);
    }
    public void ShowForRentPayment(Property property, string playerName, int amount)
    {
        decisionDescription.text = "Płacisz czynsz " + amount + "$" + " graczowi: " + playerName;
        dialogCanvasObject.SetActive(true);
        okButton.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(false);
        expandButton.gameObject.SetActive(false);
        depositButton.gameObject.SetActive(false);
        DescriptionInit(property);
    }
    Color32 GetpropertyGroupColor(PropertyGroupName groupName)
    {
        Color32 color = new Color32(255, 255, 8, 255);
        switch (groupName)
        {
            case PropertyGroupName.brown:
                color = new Color32(142, 90, 55, 255);
                break;
            case PropertyGroupName.darkBlue:
                color = new Color32(31, 89, 121, 255);
                break;
            case PropertyGroupName.green:
                color = new Color32(27, 130, 49, 255);
                break;
            case PropertyGroupName.lightBlue:
                color = new Color32(118, 160, 150, 255);
                break;
            case PropertyGroupName.orange:
                color = new Color32(152, 143, 127, 255);
                break;
            case PropertyGroupName.pink:
                color = new Color32(126, 53, 106, 255);
                break;
            case PropertyGroupName.red:
                color = new Color32(177, 20, 21, 255);
                break;
            case PropertyGroupName.yellow:
                color = new Color32(194, 177, 0, 255);
                break;

        }
        return color;
    }

}
