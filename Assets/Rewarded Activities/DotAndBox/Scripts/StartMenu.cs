using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject loadingPanel;
    public GameObject gridSize;
    public GameObject bgTheme;
    public GameObject boxImage;
    public GameObject exitPanel;
    public GameObject singlePlayer;
    public GameObject multiPlayer;
    public Sprite[] singlePlayerSprites;
    public Sprite[] multiPlayerSprites;
    public GameObject rateUsPanel;
    public Sprite fillStarSprite;

    public GameObject sound_Tick;
    public GameObject music_Tick;
    private int soundVal;
    private int musicVal;

    private void OnEnable()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Start()
    {
        Time.timeScale = 1;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        GameHandler.Instance.ref_MainMenu = this;

        if (GameHandler.Instance.isDirectPlayGame)
            GameHandler.Instance.StartGame();

        if (GameHandler.Instance.isMultiPlayerModeActive == true)
        {
            multiPlayer.GetComponent<Image>().sprite = multiPlayerSprites[1];
            singlePlayer.GetComponent<Image>().sprite = singlePlayerSprites[0];
        }
        else if (GameHandler.Instance.isMultiPlayerModeActive == false)
        {
            singlePlayer.GetComponent<Image>().sprite = singlePlayerSprites[1];
            multiPlayer.GetComponent<Image>().sprite = multiPlayerSprites[0];
        }

        SetToggle_Status();
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
        GameHandler.Instance.ref_SoundController._BtnSound();
        yield return new WaitForSeconds(0.1f);
        switch (btn)
        {
            case "GridSize":
                gridSize.SetActive(true);
                break;
            case "BgTheme":
                bgTheme.SetActive(true);
                break;
            case "BoxImage":
                boxImage.SetActive(true);
                break;
            case "Exit":
                exitPanel.SetActive(true);

                break;
            case "Music":
                if (GameHandler.Instance.ref_SoundMusicController._Get_Music_Status())
                {
                    GameHandler.Instance.ref_SoundMusicController._Set_Music_Status(1);
                    music_Tick.SetActive(true);
                }
                else
                {
                    GameHandler.Instance.ref_SoundMusicController._Set_Music_Status(0);
                    music_Tick.SetActive(false);
                }
                break;
            case "Sound":
                if (GameHandler.Instance.ref_SoundMusicController._Get_Sound_Status())
                {
                    GameHandler.Instance.ref_SoundMusicController._Set_Sound_Status(1);
                    sound_Tick.SetActive(true);
                }
                else
                {
                    GameHandler.Instance.ref_SoundMusicController._Set_Sound_Status(0);
                    sound_Tick.SetActive(false);
                }
                break;
            case "Star1":
                rateUsPanel.transform.GetChild(0).GetComponent<Image>().sprite = fillStarSprite;
                break;
            case "Star2":
                rateUsPanel.transform.GetChild(0).GetComponent<Image>().sprite = fillStarSprite;
                rateUsPanel.transform.GetChild(1).GetComponent<Image>().sprite = fillStarSprite;
                break;
            case "Star3":
                rateUsPanel.transform.GetChild(0).GetComponent<Image>().sprite = fillStarSprite;
                rateUsPanel.transform.GetChild(1).GetComponent<Image>().sprite = fillStarSprite;
                rateUsPanel.transform.GetChild(2).GetComponent<Image>().sprite = fillStarSprite;
                break;
            case "Star4":
                rateUsPanel.transform.GetChild(0).GetComponent<Image>().sprite = fillStarSprite;
                rateUsPanel.transform.GetChild(1).GetComponent<Image>().sprite = fillStarSprite;
                rateUsPanel.transform.GetChild(2).GetComponent<Image>().sprite = fillStarSprite;
                rateUsPanel.transform.GetChild(3).GetComponent<Image>().sprite = fillStarSprite;
                break;
            case "Star5":
                rateUsPanel.transform.GetChild(0).GetComponent<Image>().sprite = fillStarSprite;
                rateUsPanel.transform.GetChild(1).GetComponent<Image>().sprite = fillStarSprite;
                rateUsPanel.transform.GetChild(2).GetComponent<Image>().sprite = fillStarSprite;
                rateUsPanel.transform.GetChild(3).GetComponent<Image>().sprite = fillStarSprite;
                rateUsPanel.transform.GetChild(4).GetComponent<Image>().sprite = fillStarSprite;
                break;
            case "Play":
                GameHandler.Instance.StartGame();
                break;
            case "SinglePlayer":
                GameHandler.Instance.isMultiPlayerModeActive = false;
                multiPlayer.GetComponent<Image>().sprite = multiPlayerSprites[0];
                singlePlayer.GetComponent<Image>().sprite = singlePlayerSprites[1];
                break;
            case "MultiPlayer":
                GameHandler.Instance.isMultiPlayerModeActive = true;
                singlePlayer.GetComponent<Image>().sprite = singlePlayerSprites[0];
                multiPlayer.GetComponent<Image>().sprite = multiPlayerSprites[1];
                break;
        }

        yield return null;
    }

    public void ExitApp()
    {
        GameHandler.Instance.ref_SoundController._BtnSound();
        loadingPanel.SetActive(true);
    }

    public void NoExit()
    {
        GameHandler.Instance.ref_SoundController._BtnSound();
        exitPanel.SetActive(false);
    }

}
