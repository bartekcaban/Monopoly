using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InfoPopup : MonoBehaviour
{

    public GameObject popupCanvasObject;
    private static InfoPopup popup;

    public Text popupHeader;
    public Text popupDescription;

    public Button okButton;

    public bool active = false;

    public static InfoPopup Instance()
    {
        if (!popup)
        {
            popup = FindObjectOfType(typeof(InfoPopup)) as InfoPopup;
            if (!popup)
                Debug.LogError("There is no popup object in scene!");
        }

        return popup;
    }

    void Close()
    {
        popupCanvasObject.SetActive(false);
        active = false;
    }

    
    
    public void BoughtPropertyInfo(string propertyName)
    {
        popupHeader.text = "Zakupiłeś nieruchomość!";
        popupDescription.text = "Zostajesz właścicielem nieruchomości: " + propertyName + ".";

        okButton.onClick.RemoveAllListeners();
        
        okButton.onClick.AddListener(Close);

        popupCanvasObject.gameObject.SetActive(true);
    }
    public void ShowMessage(string header, string description)
    {
        active = true;

        popupHeader.text = header;
        popupDescription.text = description;

        okButton.onClick.RemoveAllListeners();

        okButton.onClick.AddListener(Close);

        popupCanvasObject.gameObject.SetActive(true);
    }


}
