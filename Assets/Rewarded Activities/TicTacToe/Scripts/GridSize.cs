using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSize : MonoBehaviour
{
    public Sprite[] clickedSprites;
    public void ButtonClicked(string btnName)
    {
        GameController.Instance.ref_SoundController._BtnSound();
        switch (btnName)
        {
            case "3*3":
                transform.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().sprite = clickedSprites[0];
                GameController.GridSize = 1;
                break;
            case "4*4":
                transform.GetChild(2).GetChild(1).gameObject.GetComponent<Image>().sprite = clickedSprites[1];
                GameController.GridSize = 2;
                break;               
        }
        
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.3f);
        GameController.Instance.gamePlayCanvas.SetActive(true);
        GameController.Instance.menuCanvas.SetActive(false);
        GameController.Instance.ref_SetSelectionInGameplay.DrawDesignGrid();
    }


}
