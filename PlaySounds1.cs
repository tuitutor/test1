using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds1 : MonoBehaviour
{
    public AudioClip din; // source audio
    /// 
    /// 
    AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(din);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) //здесь задаете  любую кнопку
            audio.PlayOneShot(din);
    }
}
