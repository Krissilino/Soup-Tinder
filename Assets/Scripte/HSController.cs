using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class HSController : MonoBehaviour
{
    private float countTime = 0;
    private bool gameEnded;

    private int currentLevel;
    private int currentSnip;

    public ServerConnect server;

    public float timeStart = 45;
    public Text timeText;

    public Slider timeLeftSlider;
    public Image fillImage;

    public int highscore;
    public Text hsText;

    //PlayerUI
    public Slider expSlider;
    public Image avatarUI;
    public Image hatUI;
    public AvatarsAndHats avatarHats;
    public Text level;

    //Endscreen
    public Text endHighscore;
    public LeaderboardEntry spielerPlatzierung;
    public BelohnungSystem belohnung;

    [HideInInspector]
    public List<string> usedFood;
    public int combo;
    public GameObject comboFlame;
    public GameObject comboFail;

    public Text snippetText;

    public GameObject endScreen;


    void Start()
    {
        gameEnded = false;

        timeText.text = timeStart.ToString();

        //Set-Up UI
        expSlider.maxValue = LevelSystem.expForNextLevel;
        expSlider.value = GameManager.experience;

        (Sprite,Sprite) sprites = avatarHats.ReturnAvatarAndHatSprites(GameManager.avatarNum, GameManager.bestHatNum);

        avatarUI.sprite = sprites.Item1;
        hatUI.sprite = sprites.Item2;
        level.text = GameManager.playerLevel.ToString();

        LoadLevel();
    }

    void Update()
    {
        if (gameEnded)
            return;

        if (timeStart <= 0)
        {
            EndGame();
        }
        expSlider.value = GameManager.experience;

        timeStart -= Time.deltaTime;
        timeText.text = Mathf.Round(timeStart).ToString();

        countTime += Time.deltaTime;

        timeLeftSlider.value = timeStart > 60 ? 60 : timeStart;

        if(timeStart < 20 && timeStart > 10)
        {
            fillImage.color = new Color(0.85f, 0.58f, 0.549f);
            timeText.color = new Color(0.85f, 0.58f, 0.549f);
        }
        else if(timeStart < 10)
        {
            fillImage.color = new Color(0.898f, 0.45f, 0.45f);
            timeText.color = new Color(0.898f, 0.45f, 0.45f);
        }
    }

    public void LoadLevel()
    {
        var allLevels = JSON.Parse(Resources.Load<TextAsset>("rezept").ToString());

        //currentLevel = Random.Range(0, GameManager.playerLevel - 1);
        currentLevel = Random.Range(0, 2);
        currentSnip = Random.Range(0, 9);

        string newText = allLevels["level"][currentLevel]["recipePart"][currentSnip]["sentence"];
        snippetText.text = newText.Replace("_", "_____");
    }

    public void AddScore(bool usedAgain)
    {
        if (usedAgain)
        {
            FindObjectOfType<AudioManager>().Stop("boilingWater");

            highscore += 10;
            combo = 0;
            comboFlame.SetActive(false);
            comboFail.SetActive(true);
        }
        else if (combo >= 12)
        {
            FindObjectOfType<AudioManager>().ChangeVolume("boilingWater", .4f);

            highscore += 100 * 8;
            comboFail.SetActive(false);
            comboFlame.SetActive(true);
            comboFlame.GetComponentInChildren<Text>().text = "x8";
        }
        else if (combo >= 8)
        {
            FindObjectOfType<AudioManager>().ChangeVolume("boilingWater", .2f);

            highscore += 100 * 4;
            comboFail.SetActive(false);
            comboFlame.SetActive(true);
            comboFlame.GetComponentInChildren<Text>().text = "x4";
        }
        else if (combo >= 4)
        {
            FindObjectOfType<AudioManager>().ChangeVolume("boilingWater", .05f);
            FindObjectOfType<AudioManager>().Play("boilingWater");

            highscore += 100 * 2;
            comboFail.SetActive(false);
            comboFlame.SetActive(true);
            comboFlame.GetComponentInChildren<Text>().text = "x2";
        }
        else
        {
            highscore += 100;
            comboFail.SetActive(false);
        }

        hsText.text = highscore.ToString();
    }

    public string[] GetTextVerbAttribute()
    {
        var allLevels = JSON.Parse(Resources.Load<TextAsset>("rezept").ToString());

        string verb = allLevels["level"][currentLevel]["recipePart"][currentSnip]["verb"];
        string attribute = allLevels["level"][currentLevel]["recipePart"][currentSnip]["attribute"];
        string sentence = allLevels["level"][currentLevel]["recipePart"][currentSnip]["sentence"];

        string[] info = { verb, attribute, sentence };

        return info;
    }

    private void EndGame()
    {
        gameEnded = true;
        GameManager.timeHighscoreMode += countTime;
        FindObjectOfType<AudioManager>().Stop("boilingWater");

        if (highscore > GameManager.highscore)
        {
            FindObjectOfType<AudioManager>().Play("levelFinished");

            endHighscore.text = "Neuer Highscore: " + "\n" + highscore.ToString();
            GameManager.highscore = highscore;
            server.CallSendHighscore(highscore);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("comboFail");

            endHighscore.text = "Deine Punktzahl: " + "\n" + highscore.ToString();
        }

        //Set-Up Endscreen
        //TODO RANG ABFRAGEN!!!
        spielerPlatzierung.UpdateEntry(GameManager.avatarNum, GameManager.bestHatNum, GameManager.highscore, GameManager.name);
        StartCoroutine(GetScoreRank());
        endScreen.SetActive(true);

        if (!Collectables.avatarUnlocked[2] && GameManager.highscore >= 5000)
            belohnung.OpenAvatarBelohnung(8);
        else if (!Collectables.avatarUnlocked[5] && GameManager.highscore >= 2500)
            belohnung.OpenAvatarBelohnung(11);
    }

    IEnumerator GetScoreRank()
    {
        string getRankURL = "https://krizzdesign.de/getScoreRank.php";
        string rank = "";

        using (UnityWebRequest rank_get = UnityWebRequest.Get(getRankURL))
        {
            yield return rank_get.SendWebRequest();

            if (rank_get.isNetworkError || rank_get.isHttpError)
            {
                print("There was an error getting the rank: " + rank_get.error);
            }
            else
            {
                string table = rank_get.downloadHandler.text;
                List<string> tab = new List<string>(table.Split('#'));
                
                for(int r = 1; r <= tab.Count; r++)
                {
                    if (tab[r-1] == GameManager.name)
                    {
                        rank = r.ToString();
                        break;
                    }
                }
            }
        }

        spielerPlatzierung.UpdateRank(rank + ". Platz");
    }

    public void BackToMenu()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        GameManager.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSameLevelAgain()
    {
        FindObjectOfType<AudioManager>().Play("levelStart");

        GameManager.SaveGame();
        SceneManager.LoadScene("highscore");
    }

    public void LoadLeaderboard()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        GameManager.SaveGame();
        SceneManager.LoadScene("Rangliste");
    }


}
