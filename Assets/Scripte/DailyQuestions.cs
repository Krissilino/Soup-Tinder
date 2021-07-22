using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyQuestions : MonoBehaviour
{
    public List<Button> emoButtons = new List<Button>();
    public List<Button> timeButtons = new List<Button>();

    private int emotionOfDay = 0;
    private int timeOfDay = 0;

    public Button weiterButton;


    public void SetEmotion(int num)
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        emotionOfDay = num;

        if (emotionOfDay > 0 && timeOfDay > 0)
            weiterButton.interactable = true;

        foreach(Button but in emoButtons)
        {
            but.interactable = true;
        }

        emoButtons[num - 1].interactable = false;

    }

    public void SaveTime(int num)
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        timeOfDay = num;

        if (emotionOfDay > 0 && timeOfDay > 0)
            weiterButton.interactable = true;

        foreach (Button but in timeButtons)
        {
            but.interactable = true;
        }

        timeButtons[num - 1].interactable = false;
    }

    public void SaveDailyEmotionsAndTime()
    {
        FindObjectOfType<AudioManager>().Play("buttonClick");

        GameManager.questFeelings += ("," + emotionOfDay.ToString());
        GameManager.questTime += ("," + timeOfDay.ToString());
    }
}
