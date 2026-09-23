using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UserProfile : MonoBehaviour
{
   
    public InputField userName_iField;
    public InputField opponentName_iField;
    private string userName;
    private string opponentName;
    public Text totalGamesPlayed;
    public Text gamesWin;
    public Text gamesLost;
    public Text gamesTied;
    public Text winPercentage;

    void Start()
    {
        UpdateInputField();
    }

    private void OnEnable()
    {
        gamesWin.text = "" + PlayerPrefs.GetInt("GamesWin",0);
        gamesLost.text = "" + PlayerPrefs.GetInt("GamesLost", 0); ;
        gamesTied.text = "" + PlayerPrefs.GetInt("GamesTie",0);
        float _total = PlayerPrefs.GetInt("GamesWin", 0) + PlayerPrefs.GetInt("GamesLost", 0) + PlayerPrefs.GetInt("GamesTie", 0);
        totalGamesPlayed.text = "" + _total;
        if (_total == 0)
        {
            _total = 1;
        }       
        winPercentage.text = "" + ((PlayerPrefs.GetInt("GamesWin", 0) / _total) * 100).ToString("f2") + " %";
    }

    void UpdateInputField()
    {
        string userGivenName = PlayerPrefs.GetString("UserName");
        userName_iField.text = userGivenName;
        string opponentGivenName = PlayerPrefs.GetString("OpponentName");
        opponentName_iField.text = opponentGivenName;
    }

    public void Back()
    {
        GameController.Instance.ref_SoundController._BtnSound();
        userName = userName_iField.text;
        opponentName = opponentName_iField.text;
        if (userName_iField.text == "")
        {
            userName = "Player1";
        }
        if (opponentName_iField.text == "")
        {
            opponentName = "Player2";
        }
        PlayerPrefs.SetString("UserName", userName);
        PlayerPrefs.SetString("OpponentName", opponentName);
        gameObject.SetActive(false);
    }
}
