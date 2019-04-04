using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogMenu : MonoBehaviour
{

    public GameObject dialogCanvasObject;
    private static DialogMenu dialogMenu;

    //Property Description area
    public Text oneHouseRent;
    public Text twoHouseRent;
    public Text threeHouseRent;
    public Text fourHouseRent;
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

    void DescriptionInit(Property property)
    {
        housePrice.text = property.housePrice.ToString();
        hotelPrice.text = property.hotelPrice.ToString();
        oneHouseRent.text = property.rentPerHouse.ToString();
        twoHouseRent.text = (property.rentPerHouse * 2).ToString();
        threeHouseRent.text = (property.rentPerHouse * 3).ToString();
        fourHouseRent.text = (property.rentPerHouse * 4).ToString();
    }

    public void ShowAbleToBuy(Property property)
    {
        decisionDescription.text = "Ta nieruchomość nie ma jeszcze właściciela";
       
        dialogCanvasObject.SetActive(true);
        okButton.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(true);
        expandButton.gameObject.SetActive(false);
        depositButton.gameObject.SetActive(false);
        DescriptionInit(property);

        
    }
    public void ShowForPropertyOwner(Property property)
    {
        decisionDescription.text = "Jesteś właścicielem tej nieruchomości";
        
        dialogCanvasObject.SetActive(true);
        okButton.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(false);
        expandButton.gameObject.SetActive(true);
        depositButton.gameObject.SetActive(true);
        DescriptionInit(property);
    }
    public void ShowForRentPayment(Property property)
    {
        decisionDescription.text = "Płacisz czynsz na rzecz gracza: GRACZ w wysokości 100 zł";
       
       
        dialogCanvasObject.SetActive(true);
        okButton.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(false);
        expandButton.gameObject.SetActive(false);
        depositButton.gameObject.SetActive(false);
        DescriptionInit(property);

    }

}
