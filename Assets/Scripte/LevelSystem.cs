using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static int expForNextLevel;

    public static void SetUpExpForNextLevel()
    {
        if (GameManager.playerLevel == 29)
        {
            expForNextLevel = 3800;
        }
        else
        {
            expForNextLevel = 400 + ((GameManager.playerLevel - 1) * 120);
        }
    }

    // returns if there is a level up
    public static bool AddExp(int amount)
    {
        if(GameManager.playerLevel < 30)
        {
            GameManager.experience += amount;

            if(GameManager.experience >= 3800)
            {
                GameManager.experience = 3800;
                GameManager.playerLevel++;
                return true;
            }
            else if (GameManager.experience >= expForNextLevel)
            {
                GameManager.playerLevel++;
                GameManager.experience -= expForNextLevel;
                SetUpExpForNextLevel();
                return true;
            }
        }
        return false;
    }
}
