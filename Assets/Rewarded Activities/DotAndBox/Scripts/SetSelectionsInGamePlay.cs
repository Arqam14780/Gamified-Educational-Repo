using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSelectionsInGamePlay : MonoBehaviour
{
    public GameObject bgTheme;
    public GameObject gridContainer;
    public GameObject playerImageContainer;
    public GameObject aiImageContainer;
    public Sprite[] BGThemeSprites;
    public Sprite[] PlayerFilledSprites;
    public Sprite[] AIFilledSprites;

    GameObject activeGrid;
    int index = 0;

    private void Start()
    {
        GameHandler.Instance.ref_SetSelectionsInGamePlay = this;
    }
    public void DrawGrid()
    {
        index=PlayerPrefs.GetInt("ThemeImgSelected");
        bgTheme.GetComponent<Image>().sprite = BGThemeSprites[index];
        activeGrid = gridContainer.transform.GetChild(PlayerPrefs.GetInt("GridSelected")).gameObject;
        activeGrid.SetActive(true);
        aiImageContainer = activeGrid.transform.GetChild(0).gameObject;
        playerImageContainer = activeGrid.transform.GetChild(1).gameObject;

        for (int i = 0; i < aiImageContainer.transform.childCount; i++)
        {
            aiImageContainer.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = AIFilledSprites[PlayerPrefs.GetInt("BoxImageSelected")];
            playerImageContainer.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = PlayerFilledSprites[PlayerPrefs.GetInt("BoxImageSelected")];
        }

        switch (PlayerPrefs.GetInt("BoxImageSelected"))
        {
            case 0:
                if(PlayerPrefs.GetString("SwitchText1") == "Yes")
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
    void SwitchedBoxImages() {
        for (int i = 0; i < aiImageContainer.transform.childCount; i++)
        {
            aiImageContainer.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = PlayerFilledSprites[PlayerPrefs.GetInt("BoxImageSelected")];
            playerImageContainer.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = AIFilledSprites[PlayerPrefs.GetInt("BoxImageSelected")];
        }
    }


}
