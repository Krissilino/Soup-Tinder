using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;       //https://github.com/Bunny83/SimpleJSON
using UnityEngine.Networking;

public class NextRound : MonoBehaviour
{
    public Text fehlerText;

    public ServerConnect server;
    public TextManager textMan;

    public InputField userInput;
    public Text abschnitt;

    private int abschnittCount = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        fehlerText.text = "Wort wird geprüft...";

        StartCoroutine(MakeRequestBingGet(userInput.text));


    }

    private void CheckResponseBing(string jsonResponse)
    {

        var feedback = JSON.Parse(jsonResponse);

        if (!string.IsNullOrEmpty(feedback["flaggedTokens"][0]["token"]))
        {
            FindObjectOfType<AudioManager>().Play("error");

            fehlerText.text = "Wort nicht erkannt oder falsch geschrieben";

        }
        else if (userInput.text.Trim() == "")
        {
            FindObjectOfType<AudioManager>().Play("error");

            fehlerText.text = "Es wurde kein Text eingegeben";
        }
        else
        {
            fehlerText.text = "";
            SubmitAnswer();
        }
    }


    private void SubmitAnswer()
    {
        string[] textInfo = textMan.GetTextVerbAttribute();

        string input = userInput.text.Trim();

        print(input + textInfo[0] + textInfo[1] + textInfo[2]);
        server.CallSendGapInput(input, textInfo[0], textInfo[1], textInfo[2]);

        GameManager.foodInputs += 1;

        userInput.text = "";

        textMan.LoadNextText();

        abschnittCount++;
        if (abschnittCount <= 10)
        {
            abschnitt.text = abschnittCount + " / 10";
        }
        else if (GameManager.levelPlayed <= GameManager.level)
        {
            GameManager.levelPlayed = GameManager.level + 1;
            GameManager.SaveGame();
        }

       
    }



    IEnumerator MakeRequestBingGet(string input)
    {

        string url = "https://api.bing.microsoft.com/v7.0/spellcheck?mode=spell&text=" + input + "&mkt=de-DE&setLang=de";



        UnityWebRequest spellCheck = UnityWebRequest.Get(url);



        spellCheck.SetRequestHeader("Ocp-Apim-Subscription-Key", "5c5eb894869143df90aa5b5c9416fb83");

        //spellCheck.SetRequestHeader("Content-Type", "x-www-form-urlencoded");


        yield return spellCheck.SendWebRequest();

        if (spellCheck.isNetworkError)
        {
            Debug.Log("Error: " + spellCheck.error);
            fehlerText.text = "Bing Check wirft error. Achtung bücken!";
        }
        else
        {
            Debug.Log("Received " + spellCheck.downloadHandler.text);
            CheckResponseBing(spellCheck.downloadHandler.text);
        }

    }

}
