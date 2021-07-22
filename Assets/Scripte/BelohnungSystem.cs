using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BelohnungSystem : MonoBehaviour
{
    public GameObject avatarObject;
    public AvatarsAndHats avatarHats;
    public Image avatar;
    public Image hat;
    public Text levelText;

    public Text titel;
    public GameObject belohnung;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            levelText.gameObject.SetActive(false);
            avatarObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void OpenLevelBelohnung(int level)
    {
        FindObjectOfType<AudioManager>().Play("unlock");

        titel.text = "Neues Level";
        belohnung.SetActive(true);
        levelText.gameObject.SetActive(true);
        levelText.text = level.ToString();
    }

    public void OpenAvatarBelohnung(int charnum)
    {
        FindObjectOfType<AudioManager>().Play("unlock");

        titel.text = "Neuer Avatar";
        belohnung.SetActive(true);

        (Sprite, Sprite) sprites = avatarHats.ReturnAvatarAndHatSprites(charnum, GameManager.bestHatNum);

        avatar.sprite = sprites.Item1;
        hat.sprite = sprites.Item2;
        avatarObject.SetActive(true);
    }

    public void OpenHutBelohnung()
    {
        FindObjectOfType<AudioManager>().Play("unlock");

        titel.text = "Neuer Hut";
        belohnung.SetActive(true);

        (Sprite, Sprite) sprites = avatarHats.ReturnAvatarAndHatSprites(GameManager.avatarNum, GameManager.bestHatNum);

        avatar.sprite = sprites.Item1;
        hat.sprite = sprites.Item2;
        avatarObject.SetActive(true);
    }
    
}
