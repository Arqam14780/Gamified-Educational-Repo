using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSizeSelected : MonoBehaviour
{
    public GameObject[] tick;

    private void OnEnable()
    {
        int temp = PlayerPrefs.GetInt("GridSelected");
        tick[temp].SetActive(true);
    }
    public void ButtonClicked(int Id)
    {
        StartCoroutine(butttonClick(Id));
    }
    IEnumerator butttonClick(int Id)
    {
        GameHandler.Instance.ref_SoundController._BtnSound();

        yield return new WaitForSeconds(0.2f);

        foreach (GameObject item in tick)
        {
            item.SetActive(false);
        }
        tick[Id].SetActive(true);
        PlayerPrefs.SetInt("GridSelected", Id);
    }

    public void BackPressed()
    {
        GameHandler.Instance.ref_SoundController._BtnSound();
        gameObject.SetActive(false);
    }

}
