using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject menuCanvas, gameplayCanvas;
    public GameObject pausePanel;
    public GameObject PlayerTurnText;
    public GameObject AITurnText;
    public Text playerTextShow;
    public Text aiTextShow;
    public int player1BoxScore=0;
    public int player2BoxScore = 0;
    public Text PanelHeadingText;
    public GameObject levelCompletePanel;
    public Text levelComppleteScoreText;
    public Sprite[] levelCompleteFailedSprite; 
    int _index=0;
    List<GameObject> AllNodes;


    private void Start()
    {
        GameHandler.Instance.ref_LevelManager = this;
    }

    public void CheckPlayType()
    {
        if (GameHandler.Instance.isMultiPlayerModeActive == true)
        {
            PlayerTurnText.SetActive(false);
            AITurnText.SetActive(true);
            AITurnText.GetComponent<Text>().text = "Player1 Turn";
            playerTextShow.text = "Player1 Score: " + player1BoxScore;
            aiTextShow.text = "Player2 Score: " + player2BoxScore;
        }
    }

    public IEnumerator LevelComplete()
    {
        _index = 0;
        AllNodes = GameHandler.Instance.ref_SlidersAndNodes.AllNodes;

        if (GameHandler.Instance.isMultiPlayerModeActive == true)
        {
            if (GameHandler.Instance.ref_MultiPlayerModeController.playerNumberTurn==1)
            {
                player1BoxScore++;
                playerTextShow.text = "Player1 Score: " + player1BoxScore;
            }
            else if (GameHandler.Instance.ref_MultiPlayerModeController.playerNumberTurn == 2)
            {
                player2BoxScore++;
                aiTextShow.text = "Player2 Score: " + player2BoxScore;
            }
        }
        else if (GameHandler.Instance.isMultiPlayerModeActive == false)
        {
            if (GameHandler.Instance.PlayerTurn == true && GameHandler.Instance.ref_AIScript.isMakeBox == false)
            {
                player1BoxScore++;
                playerTextShow.text = "Player Score: " + player1BoxScore;
            }

            else if (GameHandler.Instance.PlayerTurn == true && GameHandler.Instance.ref_AIScript.isMakeBox == true)
            {
                player2BoxScore++;
                aiTextShow.text = "AI Score: " + player2BoxScore;
            }
        }
        
        for (int i = 0; i < AllNodes.Count; i++)
        {
            if (AllNodes[i].GetComponent<GetConnectedSliders>().SlidersEmptySides == 0)
            {
                _index++;
            }
            if (_index == AllNodes.Count)
            {
                yield return new WaitForSeconds(1.4f);

                if (GameHandler.Instance.isMultiPlayerModeActive == false)
                {
                    levelComppleteScoreText.text = "Your Score: " + player1BoxScore + " out of " + AllNodes.Count;
                    if (player1BoxScore > player2BoxScore)
                    {
                        levelCompletePanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = levelCompleteFailedSprite[0];
                        PanelHeadingText.text = "YOU WIN!!!";
                        GameHandler.Instance.ref_SoundController.PlayWinSound();
                    }
                    else if (player1BoxScore < player2BoxScore)
                    {
                        levelCompletePanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = levelCompleteFailedSprite[1];
                        PanelHeadingText.text = "YOU LOSE!!!";
                        GameHandler.Instance.ref_SoundController.PlayLoseSound();
                    }
                    else if (player1BoxScore == player2BoxScore)
                    {
                        PanelHeadingText.text = "TIE !!!";
                        GameHandler.Instance.ref_SoundController.PlayLoseSound();
                    }
                }

                if (GameHandler.Instance.isMultiPlayerModeActive == true)
                {
                    levelComppleteScoreText.text = "Player 1 Score: " + player1BoxScore + "      Player 2 Score: " + player2BoxScore;
                    if (player1BoxScore > player2BoxScore)
                    {
                        PanelHeadingText.text = "Player1 WIN!!!";
                        GameHandler.Instance.ref_SoundController.PlayWinSound();
                    }
                    else if (player1BoxScore < player2BoxScore)
                    {
                        PanelHeadingText.text = "Player2 WIN!!!";
                        GameHandler.Instance.ref_SoundController.PlayWinSound();
                    }
                    else if (player1BoxScore == player2BoxScore)
                    {
                        PanelHeadingText.text = "TIE !!!";
                        GameHandler.Instance.ref_SoundController.PlayLoseSound();
                    }
                }
                levelCompletePanel.SetActive(true);

                Time.timeScale = 0;
            }
        }
      }

    public void ButtonClick(string btnName)
    {
        StartCoroutine(btnInput(btnName));
    }

    IEnumerator btnInput(string btnName)
    {
        GameHandler.Instance.ref_SoundController._BtnSound();
        switch (btnName)
        {           
            case "Pause":
                Time.timeScale = 0;
                pausePanel.SetActive(true);
                break;
            case "Resume":
                Time.timeScale = 1;
                pausePanel.SetActive(false);
                break;
            case "Restart":
                Time.timeScale = 1;
                levelCompletePanel.SetActive(false);
                GameHandler.Instance.isDirectPlayGame = true;
                SceneManager.LoadScene("DotAndBox");
                break;
            case "MainMenu":
                SceneManager.LoadScene("DotAndBox");
                break;
               
        }
        yield return null;
    }

}
