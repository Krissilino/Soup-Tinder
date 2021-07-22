using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpellChecker : MonoBehaviour
{
    public string answer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMakeRequest(string input)
    {
        StartCoroutine(makeRequest(input));
    }

    IEnumerator makeRequest(string input)
    {
        answer = "wait";

        string url = "https://jspell-checker.p.rapidapi.com/check";



        UnityWebRequest spellCheck = UnityWebRequest.Post(url, "POST");

        string sendParameterString = "{\"language\": \"deDE\",\"fieldvalues\": \"" + input + "\",\"config\": { \"forceUpperCase\": false,\"ignoreIrregularCaps\": false," +
            "\"ignoreFirstCaps\": true,\"ignoreNumbers\": true,\"ignoreUpper\": false,\"ignoreDouble\": false,\"ignoreWordsWithNumbers\": true}}";

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(sendParameterString);
        spellCheck.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        spellCheck.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        spellCheck.SetRequestHeader("x-rapidapi-key", "oIwqhInLuWmshMZd4a7eqk4XmBmEp16RRCujsnldP1sLc1kzjr");
        spellCheck.SetRequestHeader("x-rapidapi-host", "jspell-checker.p.rapidapi.com");

        spellCheck.SetRequestHeader("Content-Type", "application/json");


        yield return spellCheck.SendWebRequest();

        if (spellCheck.isNetworkError)
        {
            Debug.Log("Error: " + spellCheck.error);
            answer = "error";
        }
        else
        {
            Debug.Log("Received " + spellCheck.downloadHandler.text);
            answer = spellCheck.downloadHandler.text;
        }

    }

}
