using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarsAndHats : MonoBehaviour
{

    public Sprite[] allAvatars = new Sprite[15];
    public Sprite[] allHats = new Sprite[5];
    // Start is called before the first frame update
    
    public  (Sprite,Sprite) ReturnAvatarAndHatSprites(int avatarNum, int hatNum)
    {
        return (allAvatars[avatarNum], allHats[hatNum]);
    }
}
