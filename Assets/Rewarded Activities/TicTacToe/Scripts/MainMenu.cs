using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image playBtn;
    public GameObject boxImage;
    public GameObject exitPanel;
    public GameObject singlePlayer;
    public GameObject multiPlayer;
    public GameObject profilePanel;
    public GameObject gridPanel;
    public Sprite[] singlePlayerSprites;
    public Sprite[] multiPlayerSprites;
    //public GameObject rateUsPanel;
    public Sprite fillStarSprite;

    public GameObject sound_Tick;
    public GameObject music_Tick;
    private int soundVal;
    private int musicVal;

 
    private void Start()
    {
        Time.timeScale = 1;
        GameController.Instance.ref_MainMenu = this;
        SetToggle_Status();

        //if (PlayerPrefs.GetString("1stTimeGameLoad", "True") == "True")
        //{
        //    PlayerPrefs.SetString("1stTimeGameLoad", "False");
        //    profilePanel.SetActive(true);
        //}

        if (GameController.Instance.isMultiPlayerModeActive == true)
        {
            multiPlayer.GetComponent<Image>().sprite = multiPlayerSprites[1];
            singlePlayer.GetComponent<Image>().sprite = singlePlayerSprites[0];
        }
        else if (GameController.Instance.isMultiPlayerModeActive == false)
        {
            singlePlayer.GetComponent<Image>().sprite = singlePlayerSprites[1];
            multiPlayer.GetComponent<Image>().sprite = multiPlayerSprites[0];
        }
        playBtn.enabled = false;
        Invoke("Delay", 0.4f);
    }
    void Delay()
    {
        if (playBtn.enabled == true)
        {
            playBtn.enabled = false;
        }
        else if (playBtn.enabled == false)
        {
            playBtn.enabled = true;
        }
        Invoke("Delay", 0.4f);
    }

    void SetToggle_Status()
    {
        soundVal = PlayerPrefs.GetInt("SoundValue");
        if (soundVal == 0)
        {
            sound_Tick.SetActive(false);
        }
        else
        {
            sound_Tick.SetActive(true);
        }

        musicVal = PlayerPrefs.GetInt("MusicValue");
        if (musicVal == 0)
        {
            music_Tick.SetActive(false);
        }
        else
        {
            music_Tick.SetActive(true);
        }
    }


    public void InputButton(string btn)
    {
        StartCoroutine(InputBtnCoroutine(btn));
    }
    IEnumerator InputBtnCoroutine(string btn)
    {
        GameController.Instance.ref_SoundController._BtnSound();
        yield return new WaitForSeconds(0.1f);
        switch (btn)
        {
            case "BoxImage":
                boxImage.SetActive(true);
                break;
            case "Exit":
                exitPanel.SetActive(true);
                break;
            case "Profile":
                profilePanel.SetActive(true);
                break;
            case "Music":
                if (GameController.Instance.ref_Constants._Get_Music_Status())
                {
                    GameController.Instance.ref_Constants._Set_Music_Status(1);
                    music_Tick.SetActive(true);
                }
                else
                {
                    GameController.Instance.ref_Constants._Set_Music_Status(0);
                    music_Tick.SetActive(false);
                }
                break;
            case "Sound":
                if (GameController.Instance.ref_Constants._Get_Sound_Status())
                {
                    GameController.Instance.ref_Constants._Set_Sound_Status(1);
                    sound_Tick.SetActive(true);
                }
                else
                {
                    GameController.Instance.ref_Constants._Set_Sound_Status(0);
                    sound_Tick.SetActive(false);
                }
                break;
            case "Play":
                gridPanel.SetActive(true);
                break;
            case "SinglePlayer":
                GameController.Instance.isMultiPlayerModeActive = false;
                multiPlayer.GetComponent<Image>().sprite = multiPlayerSprites[0];
                singlePlayer.GetComponent<Image>().sprite = singlePlayerSprites[1];
                break;
            case "MultiPlayer":
                GameController.Instance.isMultiPlayerModeActive = true;
                singlePlayer.GetComponent<Image>().sprite = singlePlayerSprites[0];
                multiPlayer.GetComponent<Image>().sprite = multiPlayerSprites[1];
                break;
        }

        yield return null;
    }

}
