using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
    public Image avatar;
    public Image hut;
    public Text highscore;
    public Text name;
    public Text rang;

    public AvatarsAndHats avatarsHats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEntry(int avatarNum, int hutNum, int score, string playerName)
    {
        (Sprite,Sprite) sprites = avatarsHats.ReturnAvatarAndHatSprites(avatarNum, hutNum);

        avatar.sprite = sprites.Item1;
        hut.sprite = sprites.Item2;

        highscore.text = score.ToString();
        name.text = playerName;
    }

    public void UpdateRank(string platz)
    {
        rang.text = platz;
    }
}
