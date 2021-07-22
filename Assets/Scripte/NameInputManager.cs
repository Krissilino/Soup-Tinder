using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NameInputManager : MonoBehaviour
{
    public InputField nickname;
    public Text fehlermeldung;
    public Toggle bedingungen;
    public Button fertig;

    private bool validName = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckIfValidName()
    {
        validName = true;

        foreach (char c in nickname.text)
        {
            if (!Char.IsLetterOrDigit(c))
            {
                validName = false;
                break;
            }
        }

        if (nickname.text.Length > 4 && validName && nickname.text.Length <= 30)
        {
            validName = true;
            fehlermeldung.text = "";
            if (bedingungen.isOn)
            {
                fertig.interactable = true;
            }
        }
        else
        {
            validName = false;
            fehlermeldung.text = "Name darf nur Buchstaben und Zahlen enthalten und muss zwischen 5 und 30 Zeichen enthalten.";
            fertig.interactable = false;
        }

        
    }

    public void CheckTick()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");
        if (bedingungen.isOn && validName)
        {
            fertig.interactable = true;
        }
        else
        {
            fertig.interactable = false;
        }
    }
}
