using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSelection : MonoBehaviour
{
    public GameObject[] themeImgs;
    private int index, previousInd = 0;

    private void OnEnable()
    {
        index = PlayerPrefs.GetInt("ThemeImgSelected");
        previousInd = index;

        foreach (GameObject item in themeImgs)
        {
            item.SetActive(false);
            themeImgs[index].SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject item in themeImgs)
        {
            item.SetActive(false);
        }
    }

    public void ButtonInput(string data11)
    {

        StartCoroutine(Input(data11));
    }
    IEnumerator Input(string data1)
    {
        GameHandler.Instance.ref_SoundController._BtnSound();
        yield return new WaitForSeconds(0.1f);

        switch (data1)
        {

            case "Left":
                index--;
                if (index < 0)
                    index = 0;
                ToggleLeft(index);
                break;

            case "Right":
                index++;
                if (index >= themeImgs.Length)
                    index = themeImgs.Length - 1;
                ToggleRight(index);
                break;

            case "Select":
                PlayerPrefs.SetInt("ThemeImgSelected", index);
                this.gameObject.SetActive(false);
                break;

        }
    }


    void ToggleLeft(int temp1)
    {

        themeImgs[previousInd].SetActive(false);
        themeImgs[temp1].SetActive(true);

        previousInd = index;
    }

    void ToggleRight(int temp2)
    {

        themeImgs[previousInd].SetActive(false);
        themeImgs[temp2].SetActive(true);

        previousInd = index;
    }

}
