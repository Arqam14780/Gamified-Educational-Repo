using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxImageSelection : MonoBehaviour
{
    string previousText;
    Sprite previousSprite;
    public GameObject[] tick;
    public GameObject[] textContainer;
    public GameObject[] imagesContainer;

    private void OnEnable()
    {
        int temp = PlayerPrefs.GetInt("BoxImageSelected");
        tick[temp].SetActive(true);
    }

    private void Start()
    {
        ResetSwitchItems();
    }


    public void ButtonClicked(int Id)
    {
        StartCoroutine(butttonClick(Id));
    }
    IEnumerator butttonClick(int Id)
    {
        GameController.Instance.ref_SoundController._BtnSound();
        yield return new WaitForSeconds(0.2f);

        foreach (GameObject item in tick)
        {
            item.SetActive(false);
        }
        tick[Id].SetActive(true);
        PlayerPrefs.SetInt("BoxImageSelected", Id);
    }

    public void SwitchItems(int id)
    {
        GameController.Instance.ref_SoundController.PlayBoxFillSound();
        SetSwitchItems(id);
        switch (id)
        {
            case 0:                               
                if (PlayerPrefs.GetString("SwitchText1", "No") == "No")
                {
                    PlayerPrefs.SetString("SwitchText1", "Yes");
                }
                else if(PlayerPrefs.GetString("SwitchText1") == "Yes")
                {
                    PlayerPrefs.SetString("SwitchText1", "No");
                }
                break;
            case 1:               
                if (PlayerPrefs.GetString("SwitchText2", "No") == "No")
                {
                    PlayerPrefs.SetString("SwitchText2", "Yes");
                }
                else if (PlayerPrefs.GetString("SwitchText2") == "Yes")
                {
                    PlayerPrefs.SetString("SwitchText2", "No");
                }

                break;
            case 2:
                if (PlayerPrefs.GetString("SwitchText3", "No") == "No")
                {
                    PlayerPrefs.SetString("SwitchText3", "Yes");
                }
                else if (PlayerPrefs.GetString("SwitchText3") == "Yes")
                {
                    PlayerPrefs.SetString("SwitchText3", "No");
                }
                break;
            case 3:
                if (PlayerPrefs.GetString("SwitchText4", "No") == "No")
                {
                    PlayerPrefs.SetString("SwitchText4", "Yes");
                }
                else if (PlayerPrefs.GetString("SwitchText4") == "Yes")
                {
                    PlayerPrefs.SetString("SwitchText4", "No");
                }
                break;
            case 4:
                if (PlayerPrefs.GetString("SwitchText5", "No") == "No")
                {
                    PlayerPrefs.SetString("SwitchText5", "Yes");
                }
                else if (PlayerPrefs.GetString("SwitchText5") == "Yes")
                {
                    PlayerPrefs.SetString("SwitchText5", "No");
                }
                break;
            case 5:
                if (PlayerPrefs.GetString("SwitchText6", "No") == "No")
                {
                    PlayerPrefs.SetString("SwitchText6", "Yes");
                }
                else if (PlayerPrefs.GetString("SwitchText6") == "Yes")
                {
                    PlayerPrefs.SetString("SwitchText6", "No");
                }
                break;
        }   
    }
     
    void SetSwitchItems(int id)
    {
        previousText = textContainer[id].transform.GetChild(0).GetComponent<Text>().text.ToString();
        textContainer[id].transform.GetChild(0).GetComponent<Text>().text = textContainer[id].transform.GetChild(1).GetComponent<Text>().text;
        textContainer[id].transform.GetChild(1).GetComponent<Text>().text = previousText;

        previousSprite = imagesContainer[id].transform.GetChild(0).GetComponent<Image>().sprite;
        imagesContainer[id].transform.GetChild(0).GetComponent<Image>().sprite = imagesContainer[id].transform.GetChild(1).GetComponent<Image>().sprite;
        imagesContainer[id].transform.GetChild(1).GetComponent<Image>().sprite = previousSprite;
    }

    void ResetSwitchItems()
    {
        if (PlayerPrefs.GetString("SwitchText1") == "Yes")
        {
            SetSwitchItems(0);
        }
        if (PlayerPrefs.GetString("SwitchText2") == "Yes")
        {
            SetSwitchItems(1);
        }
        if (PlayerPrefs.GetString("SwitchText3") == "Yes")
        {
            SetSwitchItems(2);
        }
        if (PlayerPrefs.GetString("SwitchText4") == "Yes")
        {
            SetSwitchItems(3);
        }
        if (PlayerPrefs.GetString("SwitchText5") == "Yes")
        {
            SetSwitchItems(4);
        }
        if (PlayerPrefs.GetString("SwitchText6") == "Yes")
        {
            SetSwitchItems(5);
        }
    }

    public void BackPressed()
    {
        GameController.Instance.ref_SoundController._BtnSound();
        gameObject.SetActive(false);
    }
}
