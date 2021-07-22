using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON; //https://github.com/Bunny83/SimpleJSON
using System.IO;
using System;

public static class GameManager
{
    private static int GAMEVERSION = 2; //1 = Programm, 2 = Spiel

    public static bool firstStart = true;

    public static int level = 0; //Das Level was zuletzt ausgewählt wurde

    public static string name = "FirstTime";

    public static bool studyFilled = false;

    public static int playerLevel = 1;
    public static int experience = 0;

    public static int highscore = 0;
    public static int daysPlayed = 0;

    public static string firstDay = "01.04.2021";
    public static string lastDayPlayed = "01.04.2021";

    
    //Nutzerstatistiken, wie oft wurden Spielmodi genutzt?

    public static int collectablesClicked = 0;  // Wie oft wurden die Collectables angeklickt?
    public static int highscoreModeClicked = 0; // Wie oft wurde der HighscoreModus angeklickt?
    public static int levelClicked = 0;         // Wie oft wurden die normalen LücklenfüllLevel angeklickt?
    public static int tinderClicked = 0;        // Wie oft wurde Soup Tinder angeklickt?
    public static int leaderboardClicked = 0;   // Wie oft wurde die Ranglistenfunktion angeklickt?

    public static float timeHighscoreMode = 0;  // Wie viel Zeit wurde im Highscoremodus verbracht?
    public static float timeLevel = 0;          // Wie viel Zeit wurde in den normalen Lückenfüllleveln verbracht?
    public static float timeTinder = 0;         // Wie viel Zeit wurde im Bewertungsmodus Soup Tinder verbacht?

    public static int foodInputsHighscore = 0;  // Anzahl der Lebensmittel, die in Lücken Füllen Highscore Mode eingegeben wurden
    public static int foodInputs = 0;           // Anzahl der Lebensmittel, die in Lücken Füllen eingegeben wurden
    public static int foodRatings = 0;          // Anzahl der Lebensmittel, die in Soup Tinder bewertet wurden

    public static int levelPlayed = 0;          // Das höchste Level, dass durchgespielt wurde

    //Gefühle des Nutzers

    public static string questFeelings = "0";
    public static string questTime = "0";


    //Gamification ONLY
    public static int avatarNum = 0;            // Der ausgewählte Avatar des Spielers
    public static int bestHatNum = 0;           // Der Hut des Spielers (Durch das Höchste Level)

    public static int GetGameVersion()
    {
        return GAMEVERSION;
    }

    public static void LoadFromSaveFile()
    {
        StreamReader inp_stm = new StreamReader(Application.persistentDataPath + "/saveData.json");

        string inp_ln = "";

        while (!inp_stm.EndOfStream)
        {
            inp_ln = inp_stm.ReadLine();
            // Do Something with the input.

            Debug.Log(inp_ln);
        }

        inp_stm.Close();



        var saveJson = JSON.Parse(inp_ln);

        name = saveJson["name"];

        studyFilled = saveJson["studyFilled"];

        experience = saveJson["experience"];
        playerLevel = saveJson["playerLevel"];
        highscore = saveJson["highscore"];

        levelPlayed = saveJson["levelPlayed"];

        collectablesClicked = saveJson["collectablesClicked"];
        highscoreModeClicked = saveJson["highscoreModeClicked"];
        levelClicked = saveJson["levelClicked"];
        tinderClicked = saveJson["tinderClicked"];
        leaderboardClicked = saveJson["leaderboardClicked"];

        timeHighscoreMode = saveJson["timeHighscoreMode"];
        timeLevel = saveJson["timeLevel"];
        timeTinder = saveJson["timeTinder"];

        foodInputsHighscore = saveJson["foodInputsHighscore"];
        foodInputs = saveJson["foodInputs"];
        foodRatings = saveJson["foodRatings"];

        avatarNum = saveJson["avatarNum"];
        questFeelings = saveJson["questFeelings"];
        questTime = saveJson["questTime"];

        Debug.Log("loaded");
    }

    public static void SaveGame()
    {      
        StreamReader inp_stm = new StreamReader(Application.persistentDataPath + "/saveData.json");

        string inp_ln = "";

        while (!inp_stm.EndOfStream)
        {
            inp_ln = inp_stm.ReadLine();
            // Do Something with the input.

            Debug.Log(inp_ln);
        }

        inp_stm.Close();

        

        var saveJson = JSON.Parse(inp_ln);

        saveJson["name"] = name;

        saveJson["studyFilled"] = studyFilled;

        saveJson["experience"] = experience;
        saveJson["playerLevel"] = playerLevel;
        saveJson["highscore"] = highscore;

        saveJson["levelPlayed"] = levelPlayed;

        saveJson["collectablesClicked"] = collectablesClicked;
        saveJson["highscoreModeClicked"] = highscoreModeClicked;
        saveJson["levelClicked"] = levelClicked;
        saveJson["tinderClicked"] = tinderClicked;
        saveJson["leaderboardClicked"] = leaderboardClicked;

        saveJson["timeHighscoreMode"] = Mathf.RoundToInt(timeHighscoreMode);
        saveJson["timeLevel"] = Mathf.RoundToInt(timeLevel);
        saveJson["timeTinder"] = Mathf.RoundToInt(timeTinder);

        saveJson["foodInputsHighscore"] = foodInputsHighscore;
        saveJson["foodInputs"] = foodInputs;
        saveJson["foodRatings"] = foodRatings;

        saveJson["avatarNum"] = avatarNum;
        saveJson["questFeelings"] = questFeelings;
        saveJson["questTime"] = questTime;

        //File.WriteAllText(Environment.CurrentDirectory + "/Assets/Resources/" + @"\saveData.json", saveJson.ToString());
        File.WriteAllText(Application.persistentDataPath + "/saveData.json", saveJson.ToString());

        Debug.Log(Application.persistentDataPath + "/saveData.json");
        
        Debug.Log("saved");
    }

    public static void SavePlayDates()
    {
        StreamReader inp_stm = new StreamReader(Application.persistentDataPath + "/saveData.json");

        string inp_ln = "";

        while (!inp_stm.EndOfStream)
        {
            inp_ln = inp_stm.ReadLine();
            // Do Something with the input.

            Debug.Log(inp_ln);
        }

        inp_stm.Close();

        var saveJson = JSON.Parse(inp_ln);

        saveJson["firstDay"] = firstDay;
        saveJson["lastDayPlayed"] = lastDayPlayed;
        saveJson["daysPlayed"] = daysPlayed;

        //File.WriteAllText(Environment.CurrentDirectory + "/Assets/Resources/" + @"\saveData.json", saveJson.ToString());
        File.WriteAllText(Application.persistentDataPath + "/saveData.json", saveJson.ToString());

    }

    public static void LoadPlayDates()
    {
        StreamReader inp_stm = new StreamReader(Application.persistentDataPath + "/saveData.json");

        string inp_ln = "";

        while (!inp_stm.EndOfStream)
        {
            inp_ln = inp_stm.ReadLine();
            // Do Something with the input.

            Debug.Log(inp_ln);
        }

        inp_stm.Close();

        var saveJson = JSON.Parse(inp_ln);

        firstDay = saveJson["firstDay"];
        lastDayPlayed = saveJson["lastDayPlayed"];
        daysPlayed = saveJson["daysPlayed"];
    }

    public static bool IsFirstStart()
    {
        try
        {
            StreamReader inp_stm = new StreamReader(Application.persistentDataPath + "/saveData.json");

            Debug.Log(Application.persistentDataPath + "/saveData.json");

            string inp_ln = "";

            while (!inp_stm.EndOfStream)
            {
                inp_ln = inp_stm.ReadLine();
                // Do Something with the input.

                Debug.Log(inp_ln);
            }

            inp_stm.Close();

            var saveJson = JSON.Parse(inp_ln);

            if (saveJson["name"].ToString().Length <= 3 || saveJson["name"].Equals("FirstTime"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (FileNotFoundException e)
        {
            var saveJson = JSON.Parse(Resources.Load<TextAsset>("saveData").ToString());

            File.WriteAllText(Application.persistentDataPath + "/saveData.json", saveJson.ToString());

            Debug.Log("File not found!" + e);
            return true;
        }
    }



    
}
