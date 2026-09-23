using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public AudioSource ref_AudioSourceBG;
    public AudioSource ref_audio;
    public AudioClip ButtonSound;
    public AudioClip rewardedSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip[] AllMusic;
    private int selected_Music = 1;

    public AudioClip sliderFillSound;


    void Start()
    {
        GameHandler.Instance.ref_SoundController = this;

        if (!GameHandler.Instance.ref_SoundMusicController._Get_Music_Status())
        {
            ref_AudioSourceBG.Stop();
        }
    }


    public void _BtnSound()
    {

        if (GameHandler.Instance.ref_SoundMusicController != null && GameHandler.Instance.ref_SoundMusicController._Get_Sound_Status())
        {
            ref_audio.PlayOneShot(ButtonSound);
        }

    }
    public void PlayWinSound()
    {
        ref_audio.PlayOneShot(winSound);
        
    }
    public void PlayLoseSound()
    {
        ref_audio.PlayOneShot(loseSound);
    }
    public void _SliderFillSound()
    {
        if (GameHandler.Instance.ref_SoundMusicController != null && GameHandler.Instance.ref_SoundMusicController._Get_Sound_Status())
        {
            ref_audio.PlayOneShot(sliderFillSound);
        }
    }

     public void PlayRewardedSound()
    {

            ref_audio.PlayOneShot(rewardedSound);

    }

    public void Play_backGroundMusic()
    {

        if (GameHandler.Instance.ref_SoundMusicController._Get_Music_Status())
        {
            ref_AudioSourceBG.Play();
        }
    }

    public void Change_Music()
    {
        if (GameHandler.Instance.ref_SoundMusicController._Get_Music_Status())
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
