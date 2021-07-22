using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowExp : MonoBehaviour
{
    public GameObject exp;
    public GameObject background;

    public void ShowExperience()
    {
        StartCoroutine(ShowThem());
    }

    IEnumerator ShowThem()
    {
        exp.GetComponent<Text>().text = GameManager.experience + " / " + LevelSystem.expForNextLevel + "\n EXP";

        background.SetActive(true);
        exp.SetActive(true);
        yield return new WaitForSeconds(2);
        exp.SetActive(false);
        background.SetActive(false);
    }
}
