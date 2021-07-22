using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnect : MonoBehaviour
{
    
    public void CallSendGapInput(string food, string verb, string attribute, string sentence)
    {
        StartCoroutine(SendGapInput(food, verb, attribute,sentence));
    }

    /** Sendet die eingegebenen Lebensmittel an die PHP die mit der Datenbank kommuniziert **/
    IEnumerator SendGapInput(string food, string verb, string attribute, string sentence)
    {
        string addFoodURL = "https://krizzdesign.de/addFoodInput.php";

        string name = GameManager.name;
        string version = GameManager.GetGameVersion().ToString();

        if(string.IsNullOrEmpty(attribute))
        {
            attribute = "null";
        }

        //TEST
        List<IMultipartFormSection> sendForm = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("version", version),
            new MultipartFormDataSection("name", name),
            new MultipartFormDataSection("food", food),
            new MultipartFormDataSection("verb", verb),
            new MultipartFormDataSection("sentence", sentence),
            new MultipartFormDataSection("attribute", attribute)
        };


        using (UnityWebRequest send_post = UnityWebRequest.Post(addFoodURL, sendForm))
        {
            yield return send_post.SendWebRequest();

            if (send_post.isNetworkError || send_post.isHttpError)
            {
                print("There was an error getting the high score: " + send_post.error);
            }
            else
            {
                print(send_post.downloadHandler.text);
            }
        }

    }

    public void CallSendSwipeInput(bool rating, string id)
    {
        StartCoroutine(SendSwipeInput(rating, id));
    }

    IEnumerator SendSwipeInput(bool isLike, string id)
    {
        string addRatingURL = "https://krizzdesign.de/addRating.php";

        string s_isLike;

        s_isLike = isLike? "1": "0";

        List<IMultipartFormSection> getForm = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("isLike", s_isLike),
            new MultipartFormDataSection("id", id)
        };


        using (UnityWebRequest rating_sent = UnityWebRequest.Post(addRatingURL, getForm))
        {
            yield return rating_sent.SendWebRequest();

            if (rating_sent.isNetworkError || rating_sent.isHttpError)
            {
                print("There was an error sending the rating to the server " + rating_sent.error);
            }
            else
            {
                print(rating_sent.downloadHandler.text);
            }
        }
    }

    public void CallSendHighscore(int highscore)
    {
        StartCoroutine(SendHighscore(highscore));
    }

    IEnumerator SendHighscore(int hs)
    {
        string addRatingURL = "https://krizzdesign.de/addHighscore.php";

        string highs = hs.ToString();
        string name = GameManager.name;
        string avatar = GameManager.avatarNum.ToString();
        string hat = GameManager.bestHatNum.ToString();

        List<IMultipartFormSection> sendForm = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("highscore", highs),
            new MultipartFormDataSection("name", name),
            new MultipartFormDataSection("avatarNum", avatar),
            new MultipartFormDataSection("hatNum", hat)
        };


        using (UnityWebRequest hs_sent = UnityWebRequest.Post(addRatingURL, sendForm))
        {
            yield return hs_sent.SendWebRequest();

            if (hs_sent.isNetworkError || hs_sent.isHttpError)
            {
                print("There was an error sending the highscore to the server " + hs_sent.error);
            }
            else
            {
                print(hs_sent.downloadHandler.text);
            }
        }
    }

    public void CallUploadPlayData()
    {
        StartCoroutine("UploadPlayData");
    }

    IEnumerator UploadPlayData()
    {
        string addDataURL = "https://krizzdesign.de/addUserData.php";


        //TEST
        List<IMultipartFormSection> sendForm = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("version", GameManager.GetGameVersion().ToString()),
            new MultipartFormDataSection("name", GameManager.name),
            new MultipartFormDataSection("playerLevel", GameManager.playerLevel.ToString()),
            new MultipartFormDataSection("experience", GameManager.experience.ToString()),
            new MultipartFormDataSection("highscore", GameManager.highscore.ToString()),
            new MultipartFormDataSection("daysPlayed", GameManager.daysPlayed.ToString()),
            new MultipartFormDataSection("firstDay", GameManager.firstDay),
            new MultipartFormDataSection("lastDayPlayed", GameManager.lastDayPlayed),
            new MultipartFormDataSection("collectablesClicked", GameManager.collectablesClicked.ToString()),
            new MultipartFormDataSection("highscoreModeClicked", GameManager.highscoreModeClicked.ToString()),
            new MultipartFormDataSection("levelClicked", GameManager.levelClicked.ToString()),
            new MultipartFormDataSection("tinderClicked", GameManager.tinderClicked.ToString()),
            new MultipartFormDataSection("leaderboardClicked", GameManager.leaderboardClicked.ToString()),
            new MultipartFormDataSection("timeLevel", GameManager.timeLevel.ToString()),
            new MultipartFormDataSection("timeTinder", GameManager.timeTinder.ToString()),
            new MultipartFormDataSection("timeHighscoreMode", GameManager.timeHighscoreMode.ToString()),
            new MultipartFormDataSection("foodInputsHighscore", GameManager.foodInputsHighscore.ToString()),
            new MultipartFormDataSection("foodInputs", GameManager.foodInputs.ToString()),
            new MultipartFormDataSection("foodRatings", GameManager.foodRatings.ToString()),
            new MultipartFormDataSection("levelPlayed", GameManager.levelPlayed.ToString()),
            new MultipartFormDataSection("questFeelings", GameManager.questFeelings),
            new MultipartFormDataSection("questTime", GameManager.questTime)
        };


        using (UnityWebRequest send_post = UnityWebRequest.Post(addDataURL, sendForm))
        {
            yield return send_post.SendWebRequest();

            if (send_post.isNetworkError || send_post.isHttpError)
            {
                print("There was an error getting the high score: " + send_post.error);
            }
            else
            {
                print(send_post.downloadHandler.text);
            }
        }
    }

}
