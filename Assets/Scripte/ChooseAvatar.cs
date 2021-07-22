using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseAvatar : MonoBehaviour
{
    public Image curAvatar;
    public Image curHat;
    public Text charText;
    public Text levelText;
    public Text nameText;

    //Aus dem Hauptmenü
    public Image avatar;
    public Image avatarHat;
    public Image avatarCircle;

    public Sprite[] allAvatars = new Sprite[15];
    public Sprite[] allHats = new Sprite[5];

    public Button[] avatarsShown = new Button[9];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCharacterCreator()
    {
        GameManager.collectablesClicked++;
        FindObjectOfType<AudioManager>().Play("buttonClick");

        //Disable sign for new
        Collectables.newAvatarHat = false;
        UpdateNewAvatarCircle();

        //Display current character
        curAvatar.sprite = allAvatars[GameManager.avatarNum];
        curHat.sprite = allHats[GameManager.bestHatNum];

        //Check which characters are available
        for(int i = 0; i < avatarsShown.Length; i++)
        {
            if (!Collectables.avatarUnlocked[i])
            {
                ColorBlock cb = avatarsShown[i].colors;
                cb.normalColor = new Color(0.164f, 0.164f, 0.164f);
                cb.selectedColor = cb.normalColor;
                avatarsShown[i].colors = cb;
            }
            
        }

        //richtigen Titel anzeigen
        string titel = "Tellerwäscher";
        int level = GameManager.playerLevel;
        if(level >= 6)
        {
            titel = "Auszubildener";
        }
        if(level >= 15)
        {
            titel = "Jungkoch";
        }
        if(level >= 23)
        {
            titel = "Chefkoch";
        }
        if(level == 30)
        {
            titel = "König der Küche";
        }

        levelText.text = "Level " + level + " " + titel;
        nameText.text = GameManager.name;

    }

    public void ChooseCharacter(int charNum)
    {
        if((charNum >= 6 && Collectables.avatarUnlocked[charNum - 6]) || charNum < 6)
        {
            curAvatar.sprite = allAvatars[charNum];
            GameManager.avatarNum = charNum;

            FindObjectOfType<AudioManager>().Play("buttonClick");
       
            UpdateCharacterInMenu();
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("error");
        }

        ShowCharText(charNum);
        GameManager.SaveGame();
    }

    public void UpdateCharacterInMenu()
    {
        avatar.sprite = allAvatars[GameManager.avatarNum];
        avatarHat.sprite = allHats[GameManager.bestHatNum];
    }

    public void UpdateNewAvatarCircle()
    {
        avatarCircle.enabled = Collectables.newAvatarHat;
    }

    private void ShowCharText(int charNum)
    {
        if(charNum < 6)
        {
            charText.text = "Wähle deinen Avatar!";
        }

        switch (charNum)
        {
            case 6:
                charText.text = "Otter: Schließe Level 20 ab";
                break;

            case 7:
                charText.text = "Bonbon: Spiele jeden Spielmodus ein Mal";
                break;

            case 8:
                charText.text = "Donut: Erreiche 5000 Punkte im Highscore-Modus";
                break;

            case 9:
                charText.text = "König: Schließe Level 30 ab";
                break;

            case 10:
                charText.text = "Katze: Schließe Level 10 ab";
                break;

            case 11:
                charText.text = "Hund: Erreiche 2500 Punkte im Highscore-Modus";
                break;

            case 12:
                charText.text = "Hase: Spiele 10 Mal den Highscore-Modus";
                break;

            case 13:
                charText.text = "Professor: Swipe 200 Mal in Rate";
                break;

            case 14:
                charText.text = "Chef on Fire: Spiele jeden Tag bis zur Umfrage";
                break;
        }

    }
}
