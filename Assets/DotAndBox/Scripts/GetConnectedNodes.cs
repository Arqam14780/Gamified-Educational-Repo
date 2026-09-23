using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetConnectedNodes : MonoBehaviour
{
    List<GameObject> AllSliders;
    public bool isFilled;
    public int[] makeBoxNumber;
    public GameObject[] filledImage;
    public GameObject[] filledImagePlayer;
    public int sliderNumber;
    public int[] GetConnectedNodeNumber;
    bool isCall = true;

    public void OnValueChangedFunction()
    {
        if (isCall)
        {
            AllSliders = GameHandler.Instance.ref_SlidersAndNodes.AllSliders;

            isCall = false;
            GameHandler.Instance.ref_SoundController._SliderFillSound();
            GetComponent<Slider>().interactable = false;
            GetComponent<Slider>().value = 1;
            if (GetComponent<Slider>().value == 1)
            {

                isFilled = true;

                for (int i = 0; i < GetConnectedNodeNumber.Length; i++)
                {
                    GameHandler.Instance.ref_SlidersAndNodes.AllNodes[GetConnectedNodeNumber[i]].GetComponent<GetConnectedSliders>().SlidersEmptySides--;

                }
                if (GameHandler.Instance.isMultiPlayerModeActive)
                {
                    if (GameHandler.Instance.ref_MultiPlayerModeController.playerNumberTurn == 1)
                    {

                        GameHandler.Instance.ref_MultiPlayerModeController.CheckIsBoxCreated(sliderNumber, filledImagePlayer);
                    }
                    else if (GameHandler.Instance.ref_MultiPlayerModeController.playerNumberTurn == 2)
                    {
                        gameObject.GetComponent<Slider>().transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = GameHandler.Instance.ref_AIScript.activeGridColor;
                        GameHandler.Instance.ref_MultiPlayerModeController.CheckIsBoxCreated(sliderNumber, filledImage);
                    }

                }
                else
                {
                    if (GameHandler.Instance.PlayerTurn == false || (GameHandler.Instance.PlayerTurn == true && GameHandler.Instance.ref_AIScript.isMakeBox == false))
                    {
                        GameHandler.Instance.ref_SlidersAndNodes.checkBoxCreated(sliderNumber, filledImagePlayer);
                    }
                    else if (GameHandler.Instance.PlayerTurn == true && GameHandler.Instance.ref_AIScript.isMakeBox == true)
                    {
                        GameHandler.Instance.ref_SlidersAndNodes.checkBoxCreated(sliderNumber, filledImage);
                    }
                }
            }
        }
    }
}