using System.Collections;
using System.Collections.Generic;
using SimpleJSON;       //https://github.com/Bunny83/SimpleJSON
using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    //PlayerUI
    public Slider expSlider;
    public Image avatarUI;
    public Image hatUI;
    public AvatarsAndHats avatarHats;
    public Text levelplayer;

    private int level;
    private int currentSnip;
    private float countTime;

    public Text snippetText;
    public Text levelText;
    public Button nextLevelButton;

    public GameObject endScreen;
    public BelohnungSystem belohnung;

    // Start is called before the first frame update
    void Start()
    {
        //Set-Up UI
        expSlider.maxValue = LevelSystem.expForNextLevel;
        expSlider.value = GameManager.experience;

        (Sprite, Sprite) sprites = avatarHats.ReturnAvatarAndHatSprites(GameManager.avatarNum, GameManager.bestHatNum);

        avatarUI.sprite = sprites.Item1;
        hatUI.sprite = sprites.Item2;
        levelplayer.text = GameManager.playerLevel.ToString();


        level = GameManager.level;
        levelText.text = "Level " + (level + 1).ToString();
        currentSnip = 0;

        //Getlevel;

        LoadLevel();
        
        //load level JSON
        //load first snippet from level [0] JSON

    }

    // Update is called once per frame
    void Update()
    {
        countTime += Time.deltaTime;
    }

    public void LoadNextText()
    {

        if(currentSnip+1 == 10)
        {
            EndGame();
        }
        else
        {
            currentSnip++;

            //var allLevels = JSON.Parse(Resources.Load<TextAsset>("rezept").ToString());

            //(snippetText.text = allLevels["level"][level]["recipePart"][currentSnip]["sentence"];
            LoadLevel();
        }

    }

    public string[] GetTextVerbAttribute()
    {
        var allLevels = JSON.Parse(Resources.Load<TextAsset>("rezept").ToString());

        string verb = allLevels["level"][level]["recipePart"][currentSnip]["verb"];
        string attribute = allLevels["level"][level]["recipePart"][currentSnip]["attribute"];
        string sentence = allLevels["level"][level]["recipePart"][currentSnip]["sentence"];

        string[] info = { verb, attribute, sentence };

        return info;
    }

    private void LoadLevel()
    {
        var allLevels = JSON.Parse(Resources.Load<TextAsset>("rezept").ToString());

        string newText = allLevels["level"][level]["recipePart"][currentSnip]["sentence"];
        snippetText.text = newText.Replace("_", "_____");
    }

    private void EndGame()
    {
        FindObjectOfType<AudioManager>().Play("levelFinished");

        GameManager.timeLevel += countTime;

        endScreen.SetActive(true);
        nextLevelButton.interactable = (level +1 < GameManager.playerLevel);

        //Nach Hüten und Avataren überprüfen
        if (level == 5)
        {
            GameManager.bestHatNum = 1;
            belohnung.OpenHutBelohnung();
        }
        else if (level == 9)
        {
            belohnung.OpenAvatarBelohnung(10);
        }
        else if (level == 14)
        {
            GameManager.bestHatNum = 2;
            belohnung.OpenHutBelohnung();
        }
        else if (level == 19)
        {
            belohnung.OpenAvatarBelohnung(6);
        }
        else if (level == 22)
        {
            GameManager.bestHatNum = 3;
            belohnung.OpenHutBelohnung();
        }
        else if (level == 29)
        {
            GameManager.bestHatNum = 4;
            belohnung.OpenAvatarBelohnung(9);
        }
    }

    public void BackToMenu()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        GameManager.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNextLevel()
    {
        FindObjectOfType<AudioManager>().Play("levelStart");

        GameManager.SaveGame();

        GameManager.level++;

        SceneManager.LoadScene("lueckenfuellen");
    }
    public void LoadSameLevelAgain()
    {
        FindObjectOfType<AudioManager>().Play("levelStart");

        GameManager.SaveGame();
        SceneManager.LoadScene("lueckenfuellen");
    }
}
