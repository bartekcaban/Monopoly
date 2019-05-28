using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogWindow : ScriptableObject
{
    
    string text;
    public GameObject dialogWindow;


    public void Show()
    {
        dialogWindow.SetActive(true);
       
    }
}
