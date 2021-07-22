using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelAuswahl : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject deckel;

    private Vector3 deckelStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        deckelStartPosition = deckel.transform.position;
        SetLevelActive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseLevel(int levelNum)
    {
        FindObjectOfType<AudioManager>().Play("levelStart");

        GameManager.levelClicked += 1;
        GameManager.level = levelNum;
        SceneManager.LoadScene("lueckenfuellen");
    }

    public void StartTinder()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        GameManager.tinderClicked += 1;
        SceneManager.LoadScene("SwipeScene");
    }

    public void StartRangliste()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        GameManager.leaderboardClicked += 1;

        SceneManager.LoadScene("Rangliste");
    }

    public void StartHighscore()
    {
        FindObjectOfType<AudioManager>().Play("levelStart");

        GameManager.highscoreModeClicked += 1;
        SceneManager.LoadScene("highscore");
    }

    private void SetLevelActive()
    {
        for (int i = 0; i < GameManager.levelPlayed && i < GameManager.playerLevel; i++)
        {
            buttons[i].GetComponent<Button>().interactable = true;
            buttons[i].GetComponent<Image>().color = Color.white;
        }

        if(GameManager.levelPlayed +1 <= GameManager.playerLevel && !buttons[GameManager.levelPlayed].GetComponent<Button>().interactable
            || GameManager.levelPlayed == 0)
        {
            buttons[GameManager.levelPlayed].GetComponent<Button>().interactable = true;
            deckel.transform.position = buttons[GameManager.levelPlayed].transform.position;
        }
        else
        {
            deckel.transform.position = deckelStartPosition;
        }
    }

}
