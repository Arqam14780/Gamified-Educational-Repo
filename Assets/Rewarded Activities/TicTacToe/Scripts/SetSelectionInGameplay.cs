using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSelectionInGameplay : MonoBehaviour
{

    public Transform gridSize;
    public Transform player1;
    public Transform player2;
    public Sprite[] Player1FilledSprites;
    public Sprite[] Player2FilledSprites;
    List<GameObject> AllBoxes;
    public Color[] boxBgColor;
    public Sprite[] BgThemeSprites;

    private void Start()
    {
        GameController.Instance.ref_SetSelectionInGameplay = this;
    }
    private void OnEnable()
    {
        
    }

    public void DrawDesignGrid()
    {
        GridSizeSet();
        AllBoxes = GameController.Instance.ref_ControllerScript.AllBoxes;

        PanelColor();
        BoxImageSet();

        SetUserNames();
        SetBGTheme();
    }

    void GridSizeSet()
    {
        if (GameController.GridSize == 1)
        {
            gridSize.GetChild(0).gameObject.SetActive(true);
        }
        else if (GameController.GridSize == 2)
        {
            gridSize.GetChild(1).gameObject.SetActive(true);
        }
    }

    void PanelColor()
    {
        int _randomNumber = Random.Range(0, 2);
        switch (_randomNumber)
        {
            case 0:
                AllBoxes[0].gameObject.transform.parent.gameObject.GetComponent<Image>().color = Color.black;
                break;
            case 1:
                AllBoxes[0].gameObject.transform.parent.gameObject.GetComponent<Image>().color = Color.white;
                break;
        }
    }
    void BoxImageSet()
    {
        for (int i = 0; i < AllBoxes.Count; i++)
        {
            AllBoxes[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Player1FilledSprites[PlayerPrefs.GetInt("BoxImageSelected")];
            AllBoxes[i].transform.GetChild(1).gameObject.GetComponent<Image>().sprite = Player2FilledSprites[PlayerPrefs.GetInt("BoxImageSelected")];
            AllBoxes[i].GetComponent<Image>().color = boxBgColor[PlayerPrefs.GetInt("BoxImageSelected")];
        }
        switch (PlayerPrefs.GetInt("BoxImageSelected"))
        {
            case 0:
                if (PlayerPrefs.GetString("SwitchText1") == "Yes")
                {
                    SwitchedBoxImages();
                }
                break;
            case 1:
                if (PlayerPrefs.GetString("SwitchText2") == "Yes")
                {
                    SwitchedBoxImages();
                }
                break;
            case 2:
                if (PlayerPrefs.GetString("SwitchText3") == "Yes")
                {
                    SwitchedBoxImages();
                }
                break;
            case 3:
                if (PlayerPrefs.GetString("SwitchText4") == "Yes")
                {
                    SwitchedBoxImages();
                }
                break;
            case 4:
                if (PlayerPrefs.GetString("SwitchText5") == "Yes")
                {
                    SwitchedBoxImages();
                }
                break;
            case 5:
                if (PlayerPrefs.GetString("SwitchText6") == "Yes")
                {
                    SwitchedBoxImages();
                }
                break;
        }
    }


    void SwitchedBoxImages()
    {
        for (int i = 0; i < AllBoxes.Count; i++)
        {
            AllBoxes[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Player2FilledSprites[PlayerPrefs.GetInt("BoxImageSelected")];
            AllBoxes[i].transform.GetChild(1).gameObject.GetComponent<Image>().sprite = Player1FilledSprites[PlayerPrefs.GetInt("BoxImageSelected")];
        }
    }
    void SetUserNames()
    {
        player1.GetChild(1).gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("UserName");
        player2.GetChild(1).gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("OpponentName");
        player1.GetChild(2).gameObject.GetComponent<Image>().sprite = AllBoxes[0].transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
        player2.GetChild(2).gameObject.GetComponent<Image>().sprite = AllBoxes[0].transform.GetChild(1).gameObject.GetComponent<Image>().sprite;
    }
    public void SetBGTheme()
    {
        int _random = Random.Range(0, 7);
        switch (_random)
        {
            case 0:
                gridSize.gameObject.GetComponent<Image>().sprite = BgThemeSprites[0];
                break;
            case 1:
                gridSize.gameObject.GetComponent<Image>().sprite = BgThemeSprites[1];
                break;
            case 2:
                gridSize.gameObject.GetComponent<Image>().sprite = BgThemeSprites[2];
                break;
            case 3:
                gridSize.gameObject.GetComponent<Image>().sprite = BgThemeSprites[3];
                break;
            case 4:
                gridSize.gameObject.GetComponent<Image>().sprite = BgThemeSprites[4];
                break;
            case 5:
                gridSize.gameObject.GetComponent<Image>().sprite = BgThemeSprites[5];
                break;
        }
    }
}
