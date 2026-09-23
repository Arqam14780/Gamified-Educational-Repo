using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMusicController : MonoBehaviour
{

    void Start()
    {
        GameHandler.Instance.ref_SoundMusicController = this;
    }

    public void _Set_Sound_Status(int soundValue)
    {

        if (soundValue > 1)
            soundValue = 1;

        PlayerPrefs.SetInt("SoundValue", soundValue);

        GameHandler.Instance.ref_SoundController._BtnSound();

    }

    public bool _Get_Sound_Status()
    {
        bool isSoundON;

        return isSoundON = PlayerPrefs.GetInt("SoundValue", 0) == 0 ? true : false;

    }

    public void _Set_Music_Status(int musicValue)
    {

        if (musicValue > 1)
            musicValue = 1;

        PlayerPrefs.SetInt("MusicValue", musicValue);
        if (musicValue == 0)
        {
            GameHandler.Instance.ref_SoundController.Play_backGroundMusic();
        }
        else
        {
            GameHandler.Instance.ref_SoundController.ref_AudioSourceBG.Stop();
        }

    }
    public bool _Get_Music_Status()
    {

        bool isMusicON;

        return isMusicON = PlayerPrefs.GetInt("MusicValue", 0) == 0 ? true : false;

    }

}
