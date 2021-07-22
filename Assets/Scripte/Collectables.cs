using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collectables
{

    public static bool[] avatarUnlocked = new bool[9];

    public static bool newAvatarHat = false;

    public static void CheckForAchievments(bool firstStart)
    {
        //Nach Hüten überprüfen
        int level = GameManager.levelPlayed;
        if (level == 30)
        {
            GameManager.bestHatNum = 4;
        }
        else if (level >= 23)
        {
            GameManager.bestHatNum = 3;
        }
        else if (level >= 15)
        {
            GameManager.bestHatNum = 2;
        }
        else if (level >= 6)
        {
            GameManager.bestHatNum = 1;
        }


        // Otter Freischalten: Erreiche Level 20
        if (GameManager.levelPlayed >= 20)
        {
            if(!avatarUnlocked[0])
                newAvatarHat = true;
            avatarUnlocked[0] = true;

        }

        //Bonbon: Teste jeden Modus einmal
        if (GameManager.levelPlayed > 0 && GameManager.foodInputsHighscore > 0 && GameManager.foodRatings > 0)
        {
            if (!avatarUnlocked[1])
                newAvatarHat = true;
            avatarUnlocked[1] = true;
        }

        //Donut: Erreiche einen Highscore von 5000
        if (GameManager.highscore >= 5000)
        {
            if (!avatarUnlocked[2])
                newAvatarHat = true;
            avatarUnlocked[2] = true;
        }

        //König: Erreiche Level 30
        if (GameManager.levelPlayed == 30)
        {
            if (!avatarUnlocked[3])
                newAvatarHat = true;
            avatarUnlocked[3] = true;
        }

        //Katze: Erreiche Level 10
        if (GameManager.levelPlayed >= 10)
        {
            if (!avatarUnlocked[4])
                newAvatarHat = true;
            avatarUnlocked[4] = true;
        }

        //Hund: Erreiche einen Highscore von 2500
        if (GameManager.highscore >= 2500)
        {
            if (!avatarUnlocked[5])
                newAvatarHat = true;
            avatarUnlocked[5] = true;
        }

        //Hase: Spiele 10 mal den Highscore-Modus
        if (GameManager.highscoreModeClicked >= 10)
        {
            if (!avatarUnlocked[6])
                newAvatarHat = true;
            avatarUnlocked[6] = true;
        }

        //Professor: 200 Bewertungen in RATE
        if (GameManager.foodRatings >= 200)
        {
            if (!avatarUnlocked[7])
                newAvatarHat = true;
            avatarUnlocked[7] = true;
        }

        //Koch on Fire: Spiele jeden Tag bis zur Umfrage
        if (GameManager.daysPlayed >= 7)
        {
            if (!avatarUnlocked[8])
                newAvatarHat = true;
            avatarUnlocked[8] = true;
        }

        if (firstStart)
        {
            newAvatarHat = false;
        }
    }
    
}
