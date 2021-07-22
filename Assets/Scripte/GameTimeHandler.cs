using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class GameTimeHandler : MonoBehaviour
{
    private int internet = 0; // 0 = noch nicht verbunden, 1 = verbunden, -1 = internetfehler
    // Start is called before the first frame update

    private bool gameLoaded = false;

    public ServerConnect server;

    public GameObject internetPopUp;
    public GameObject loadingScreen;
    public GameObject nameScreen;
    public GameObject dailyScreen;
    public GameObject tutorial;

    public InputField nameInput;
    public Text nameFehlermeldung;

    public Text nameText;
    public Text levelText;
    public Slider expSlider;

    public Button daysStudyButton;
    public Text daysTillStudyText;
    public Text daysTillStudyText2;

    public ChooseAvatar avatarInfo;

    public BelohnungSystem belohnung;
    

    void Start()
    {
        if (GameManager.firstStart)
        {
            if (GameManager.IsFirstStart())
            {
                //frage nach einem Usernamen

                nameScreen.SetActive(true);
                tutorial.SetActive(true);
                //öffnen introduction screen
                //speicher den start tag
                //lege eine save file an

                //Speicher den Starttag des Spielers
                TageBerechnen.SaveFirstDay();

                dailyScreen.SetActive(true);
            }

            //starte die Spielstart coroutine
            StartCoroutine("StartGame");
            
            //TageBerechnung wird geladen und überprüft ob neuer tag gestartet ist
            if (TageBerechnen.SetUpDaysClass())
            {
                if(!GameManager.studyFilled)
                {
                    CreateNotificationChannel();
                    SendNotification("Hast du heute schon gekocht?", "Hilf den Kochrobotern und spiele Soup Tinder!", 1);
                }

                //Speichere den neuen Tag ab
                GameManager.SavePlayDates();

                //Öffne die Daily Nachfrage
                dailyScreen.SetActive(true);

                server.CallUploadPlayData();
            }

  
        }
        else
        {
            //Lade und überprüfe die Collectables
            if (!Collectables.avatarUnlocked[1] && GameManager.levelPlayed > 0 && GameManager.foodInputsHighscore > 0 && GameManager.foodRatings > 0)
            {
                belohnung.OpenAvatarBelohnung(7);
            }
            else if (!Collectables.avatarUnlocked[6] && GameManager.highscoreModeClicked >= 10)
            {
                belohnung.OpenAvatarBelohnung(12);
            }
            else if (!Collectables.avatarUnlocked[8] && GameManager.daysPlayed >= 7)
            {
                belohnung.OpenAvatarBelohnung(14);
            }
        }

        //Setup vom Startscreen
        //Name des Spielers eintragen
        nameText.text = GameManager.name;
        //Level des Spielers anzeigen
        levelText.text = GameManager.playerLevel.ToString();
        print(GameManager.playerLevel + "ja moin");
        //Experience points anzeigen
        expSlider.maxValue = LevelSystem.expForNextLevel;
        expSlider.value = GameManager.experience;
        //Zeige Tage bis zur Studie an
        if(TageBerechnen.CalcDayUntilEnd() <= 0 && !GameManager.studyFilled)
        {
            //Zeige nun an dass man die Umfrage ausführen kann
            daysTillStudyText.text = "Hier klicken";
            daysTillStudyText2.text = "Um Studie zu beenden";
            daysStudyButton.interactable = true;
        }
        else if(TageBerechnen.CalcDayUntilEnd() <= 0 && GameManager.studyFilled)
        {
            daysTillStudyText.text = "Danke!";
            daysTillStudyText2.text = "Die Umfrage wurde ausgefüllt";
        }
        else
        {
            daysTillStudyText.text = TageBerechnen.CalcDayUntilEnd() + " TAGE";
        }
        

        //Lade und überprüfe die Collectables
        Collectables.CheckForAchievments(false);

        avatarInfo.UpdateCharacterInMenu();
        avatarInfo.UpdateNewAvatarCircle();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateNotificationChannel()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }

    private void SendNotification(string titel, string text, int days)
    {
        var notification = new AndroidNotification();
        notification.Title = titel;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddDays(days);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }

    IEnumerator CheckForConnection()
    {
        string testURL = "https://krizzdesign.de";

        using (UnityWebRequest www = UnityWebRequest.Get(testURL))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                internet = -1;
            }
            else
            {
                Debug.Log("Get Request Completed!");
                internet = 1;
            }
        }
    }

    public void TryInternetAgain()
    {
        StartCoroutine("StartGame");
    }

    IEnumerator StartGame()
    {
        GameManager.firstStart = false;
        //Zeige loading screen
        loadingScreen.SetActive(true);
        StartCoroutine("CheckForConnection");

        if (!gameLoaded)
        {
            GameManager.LoadFromSaveFile();
            GameManager.LoadPlayDates();
            LevelSystem.SetUpExpForNextLevel();
            gameLoaded = true;
        }

        while(gameLoaded == false || internet == 0)
        {
            //prüfe das Internet
            //lade save file

            yield return null;
        }

        if(internet == -1)
        {
            //zeige internet fehlermeldung
            internetPopUp.SetActive(true);
            
        }
        else
        {
            internetPopUp.SetActive(false);
            loadingScreen.SetActive(false);
            //schließe loading bildschirm
        }

        //Lade freigeschaltete Avatare
        Collectables.CheckForAchievments(true);

        //Wenn internet gefunden zeige Startscreen
    }

    public void NameChosen()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        StartCoroutine(CheckName());
    }

    IEnumerator CheckName()
    {
        string getNameURL = "https://krizzdesign.de/getPlayerName.php";

        bool noDoubleName = true;

        using (UnityWebRequest name_get = UnityWebRequest.Get(getNameURL))
        {
            yield return name_get.SendWebRequest();

            if (name_get.isNetworkError || name_get.isHttpError)
            {
                print("There was an error getting the rank: " + name_get.error);
            }
            else
            {
                string table = name_get.downloadHandler.text;
                List<string> tab = new List<string>(table.Split('#'));

                for (int r = 0; r < tab.Count; r++)
                {
                    if (tab[r].ToLower() == nameInput.text.ToLower())
                    {
                        noDoubleName = false;
                        break;
                    }
                }
            }
        }

        if (noDoubleName)
        {
            GameManager.name = nameInput.text;
            nameScreen.SetActive(false);
            GameManager.SaveGame();

            nameText.text = GameManager.name;

            CreateNotificationChannel();
            SendNotification("Hast du heute schon gekocht?", "Hilf den Kochrobotern und spiele Soup Tinder!", 1);
            SendNotification("Du hast es fast geschafft!", "Fülle jetzt die Studie aus, falls du es noch nicht getan hast.", 6);
        }
        else
        {
            nameFehlermeldung.text = "Name ist bereits vergeben..";
        }
    }

    public void EndStudy()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        GameManager.studyFilled = true;
        GameManager.SaveGame();
        //Sende die Nutzerdaten an Server
        server.CallUploadPlayData();
        //Öffne Umfrage
        Application.OpenURL("https://www.survio.com/survey/d/G5A6O9W2C9V5H8F9A");
    }


}
