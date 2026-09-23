using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public static SoundController _instance;
    [HideInInspector]
    public AudioSource ref_AudioSourceBG;
    AudioSource ref_audio;
    public AudioClip ButtonSound;
    public AudioClip rewardedSound;
    public AudioClip gameLostSound;
    public AudioClip boxFilledSound;
    public AudioClip effectSound;
    public AudioClip[] AllMusic;
    private int selected_Music = 1;

 

    void Awake()
    { 
        ref_AudioSourceBG = GetComponent<AudioSource>();
        ref_audio = GetComponent<AudioSource>();
    }


    void Start()
    {
        GameController.Instance.ref_SoundController = this;

        if (!GameController.Instance.ref_Constants._Get_Music_Status())
        {
            ref_AudioSourceBG.Stop();
        }
    }


    public void _BtnSound()
    {

        if (GameController.Instance.ref_Constants != null && GameController.Instance.ref_Constants._Get_Sound_Status())
        {
            ref_audio.PlayOneShot(ButtonSound);
        }

    }
    public void PlayBoxFillSound()
    {
        if (GameController.Instance.ref_Constants != null && GameController.Instance.ref_Constants._Get_Sound_Status())
        {
            ref_audio.PlayOneShot(boxFilledSound);
        }
    }
    public void PlayEffectSound()
    {

        ref_audio.PlayOneShot(effectSound);

    }


    public void PlayRewardedSound()
    {

        ref_audio.PlayOneShot(rewardedSound);

    }
    public void PlayLostSound()
    {

        ref_audio.PlayOneShot(gameLostSound);

    }

    public void Play_backGroundMusic()
    {

        if (GameController.Instance.ref_Constants._Get_Music_Status())
        {
            ref_AudioSourceBG.Play();
        }
    }

    public void Change_Music()
    {
        if (GameController.Instance.ref_Constants._Get_Music_Status())
        {
            ref_AudioSourceBG.clip = AllMusic[selected_Music];
            ref_AudioSourceBG.Play();
            selected_Music++;
            if (selected_Music == AllMusic.Length)
            {
                selected_Music = 0;
            }
        }
    }

    public void StopBG_Music()
    {

        ref_AudioSourceBG.Stop();

    }

}
