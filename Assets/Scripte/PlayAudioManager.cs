using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioManager : MonoBehaviour
{
    private AudioManager am;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    public void StartButtonClick()
    {
        am.Play("buttonClick");
    }
}
