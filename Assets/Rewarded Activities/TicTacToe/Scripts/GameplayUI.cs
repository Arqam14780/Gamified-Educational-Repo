using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject pausePanel;
    public Sprite[] panelHeadingSprites;
    public Text player1ScoreText;
    public Text player2ScoreText;
    public int player1Score=0;
    public int player2Score=0;

    private void Start()
    {
        GameController.Instance.ref_GameplayUI = this;
    }

    public void InputButton(string BtnName)
    {
        StartCoroutine(ButtonInput(BtnName));        
    }
    IEnumerator ButtonInput(string BtnName)
    {
        
        switch (BtnName)
        {
            case "PlayAgain":               
                GameController.Instance.ref_SoundController._BtnSound();
                GameController.Instance.ref_SoundController.Play_backGroundMusic();
                panel.SetActive(false);
                pausePanel.SetActive(false);
                for (int i = 0; i < GameController.Instance.ref_ControllerScript.AllBoxes.Count; i++)
                {
                    GameController.Instance.ref_ControllerScript.AllBoxes[i].transform.GetChild(0).gameObject.SetActive(false);
                    GameController.Instance.ref_ControllerScript.AllBoxes[i].transform.GetChild(1).gameObject.SetActive(false);
                    GameController.Instance.ref_ControllerScript.AllBoxes[i].GetComponent<Button>().interactable = true;
                    GameController.Instance.ref_ControllerScript.AllBoxes[i].GetComponent<SequenceElements>().isPressed = false;
                }
                for(int j=0; j < GameController.Instance.ref_ControllerScript.effectImagesContainer.transform.childCount; j++)
                {
                    GameController.Instance.ref_ControllerScript.effectImagesContainer.transform.GetChild(j).gameObject.SetActive(false);
                }
                GameController.Instance.ref_ControllerScript.ActiveIndexes.Clear();
                GameController.Instance.ref_ControllerScript.PlayerFirstTurn();
                GameController.Instance.ref_SetSelectionInGameplay.SetBGTheme();
                break;
            case "MainMenu":
                //AdsNetwork.IncentiveAdReward += RewardedVideoAd;
                //AdsNetwork.ShowIncentiveAd("chartboost");
                GameController.Instance.ref_SoundController._BtnSound();
                SceneManager.LoadScene("TicTacToe");
                break;
            case "LevelWin":
              
                yield return new WaitForSeconds(2f);
                player1Score++;
                player1ScoreText.text = "" + player1Score;
                panel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = panelHeadingSprites[0];
                panel.transform.GetChild(1).gameObject.SetActive(true);
                panel.SetActive(true);
                GameController.Instance.ref_SoundController.StopBG_Music();
                GameController.Instance.ref_SoundController.PlayRewardedSound();              
                int _win = PlayerPrefs.GetInt("GamesWin", 0);
                _win++;
                PlayerPrefs.SetInt("GamesWin", _win);
                break;
            case "LevelLost":
                yield return new WaitForSeconds(2f);
                player2Score++;
                player2ScoreText.text = "" + player2Score;
                panel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = panelHeadingSprites[1];
                GameController.Instance.ref_SoundController.StopBG_Music();
                GameController.Instance.ref_SoundController.PlayLostSound();
                panel.SetActive(true);
                int _lost = PlayerPrefs.GetInt("GamesLost", 0);
                _lost++;
                PlayerPrefs.SetInt("GamesLost", _lost);
                break;
            case "LevelTie":
                yield return new WaitForSeconds(1f);
                panel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = panelHeadingSprites[2];
                GameController.Instance.ref_SoundController.StopBG_Music();
                GameController.Instance.ref_SoundController.PlayLostSound();
                panel.SetActive(true);            
                int _tie = PlayerPrefs.GetInt("GamesTie", 0);
                _tie++;
                PlayerPrefs.SetInt("GamesTie", _tie);
                break;
            case "Pause":
                GameController.Instance.ref_SoundController._BtnSound();
                pausePanel.SetActive(true);
                break;
            case "Resume":
                GameController.Instance.ref_SoundController._BtnSound();
                pausePanel.SetActive(false);
                break;
            case "Music":
                GameController.Instance.ref_SoundController._BtnSound();
                break;
            case "BoxClicked":
                GameController.Instance.ref_SoundController.PlayBoxFillSound();
                break;
        }        
    }
    //void RewardedVideoAd()
    //{
    //    GameController.Instance.ref_SoundController._BtnSound();
    //    GameController.Instance.ref_SoundController.StopBG_Music();
    //    AdsNetwork.IncentiveAdReward -= RewardedVideoAd;

    //}
}
