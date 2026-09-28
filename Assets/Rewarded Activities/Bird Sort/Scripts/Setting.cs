using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public GameObject dropDown;
    [Space(5)]
    public Image musicBtn;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    [Space(5)]
    public Image vibrationBtn;
    public Sprite vibrationOnSprite;
    public Sprite vibrationOffSprite;
    [Space(5)]
    public GameObject bgMusic;

    void Start()
    {
        if (PlayerPrefs.GetInt("Music", 0) == 1)
            bgMusic.SetActive(false);
    }


    public void SettingPressed()
    {
        if (!dropDown.activeInHierarchy)
        {
            dropDown.GetComponent<Animator>().SetBool("Close", false);
            dropDown.SetActive(true);
            CheckStatus();
        }
        else
        {
            dropDown.GetComponent<Animator>().SetBool("Close", true);
            StartCoroutine(Delay());
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        dropDown.SetActive(false);
    }

    public void VibrationStatus()
    {
        if (PlayerPrefs.GetInt("Vibration", 0) == 0)
        {
            PlayerPrefs.SetInt("Vibration", 1);
            vibrationBtn.sprite = vibrationOffSprite;
        }
        else
        {
            PlayerPrefs.SetInt("Vibration", 0);
            vibrationBtn.sprite = vibrationOnSprite;
            Handheld.Vibrate();
        }
    }
    public void CheckMusicStatus()
    {
        if (PlayerPrefs.GetInt("Music", 0) == 0)
        {
            PlayerPrefs.SetInt("Music", 1);
            musicBtn.sprite = musicOffSprite;
            bgMusic.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 0);
            musicBtn.sprite = musicOnSprite;
            bgMusic.SetActive(true);
        }
    }

    void CheckStatus()
    {
        if (PlayerPrefs.GetInt("Vibration", 0) == 0)
            vibrationBtn.sprite = vibrationOnSprite;
        else
            vibrationBtn.sprite = vibrationOffSprite;

        if (PlayerPrefs.GetInt("Music", 0) == 0)
            musicBtn.sprite = musicOnSprite;
        else
            musicBtn.sprite = musicOffSprite;

    }


}
