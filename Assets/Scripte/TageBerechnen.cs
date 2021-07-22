using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TageBerechnen
{
    public static DateTime firstDay;
    public static DateTime lastDayPlayed;
    public static DateTime currDay;


    public static bool SetUpDaysClass()
    {
        firstDay = DateTime.Parse(GameManager.firstDay);
        lastDayPlayed = DateTime.Parse(GameManager.lastDayPlayed);
        currDay = System.DateTime.Today;

        return IsANewDay();
    }

    public static void DisplayDaysTillStudy()
    {
        int restDays = CalcDayUntilEnd();

        if(restDays <= 0)
        {
            //Zeige nun die Studie an
            Debug.Log("ready for study");
        }
    }
    public static int CalcDayUntilEnd()
    {
        Debug.Log(firstDay.Date.ToString() + "first day again");
        Debug.Log(currDay.ToString());
        return (firstDay.AddDays(6).Date - currDay.Date).Days;
    }

    private static bool IsANewDay()
    {
        if(lastDayPlayed.Date != currDay.Date)
        {
            GameManager.daysPlayed++;
            lastDayPlayed = currDay;
            GameManager.lastDayPlayed = currDay.ToString();
            return true;
        }
        return false;
    }

    public static void SaveFirstDay()
    {
        DateTime first = System.DateTime.Today;
        firstDay =  lastDayPlayed = currDay = first;

        GameManager.firstDay = firstDay.ToString();
        GameManager.lastDayPlayed = firstDay.ToString();
        GameManager.daysPlayed = 1;
        GameManager.SavePlayDates();
        Debug.Log(GameManager.firstDay + "first day");
    }
}
