using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiPlayerModeController : MonoBehaviour
{
    public int playerNumberTurn = 1;
    int boxSideFill = 0;
    bool isBoxFilled = false;
    List<GameObject> AllSliders;
    List<GameObject> AllNodes;


    private void Start()
    {
        GameHandler.Instance.ref_MultiPlayerModeController = this;
    }
    public void CheckIsBoxCreated(int sliderNumber, GameObject[] imageFilled)
    {
        AllSliders = GameHandler.Instance.ref_SlidersAndNodes.AllSliders;
        AllNodes = GameHandler.Instance.ref_SlidersAndNodes.AllNodes;

        int[] _attachedSliders = AllSliders[sliderNumber].GetComponent<GetConnectedNodes>().makeBoxNumber;

        for (int i = 0; i < _attachedSliders.Length; i++)
        {
            if (i <= 3)
            {
                if (AllSliders[_attachedSliders[i]].GetComponent<GetConnectedNodes>().isFilled == true)
                {
                    boxSideFill++;
                    if (boxSideFill == 4)
                    {
                        isBoxFilled = true;
                        imageFilled[0].SetActive(true);
                        BoxFilled();
                    }
                    if (i == 3 && boxSideFill < 4)
                    {
                        boxSideFill = 0;                       
                    }
                }
                else
                {
                    boxSideFill = 0;
                }
            }
            else if (i > 3)
            {
                if (AllSliders[_attachedSliders[i]].GetComponent<GetConnectedNodes>().isFilled == true)  
                {
                    boxSideFill++;
                    if (boxSideFill == 3)
                    {
                        isBoxFilled = true;
                        imageFilled[1].SetActive(true);
                        BoxFilled();
                    }
                    if (i == _attachedSliders.Length - 1 && boxSideFill < 3)
                    {
                        boxSideFill = 0;
                    }
                }
                else
                {
                    boxSideFill = 0;
                }
            }
        }
        if (isBoxFilled == false)
        {
            PlayerTurnChanged();
        }
        isBoxFilled = false;

    }

    private void PlayerTurnChanged()
    {
        switch (playerNumberTurn)
        {
            case 1:
                GameHandler.Instance.ref_LevelManager.AITurnText.GetComponent<Text>().text= "Player2 Turn";
                playerNumberTurn = 2;
                break;
            case 2:
                GameHandler.Instance.ref_LevelManager.AITurnText.GetComponent<Text>().text = "Player1 Turn";
                playerNumberTurn = 1;
                break;
        }      
    }

    private void BoxFilled()
    {
        boxSideFill = 0;
        StartCoroutine(GameHandler.Instance.ref_LevelManager.LevelComplete());
    }
}
