using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersAndNodes : MonoBehaviour
{
    public GameObject AllSlider;
    public GameObject AllNode;
    public List<GameObject> AllSliders = new List<GameObject>();
    public List<GameObject> AllNodes = new List<GameObject>();
    GameObject currentSlider;
    GameObject newNode;
    int boxSideFill = 0;
    bool isPlayerBoxFilled = false;


    private void Start()
    {
        GameHandler.Instance.ref_SlidersAndNodes = this;

        for (int i = 0; i < AllSlider.transform.childCount; i++)
        {
            AllSliders.Add(AllSlider.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < AllNode.transform.childCount; i++)
        {
            AllNodes.Add(AllNode.transform.GetChild(i).gameObject);
        }
    }



    public void checkBoxCreated(int sliderNumber, GameObject[] imageFilled)
    {
        int[] _attachedSliders = AllSliders[sliderNumber].GetComponent<GetConnectedNodes>().makeBoxNumber;

        for (int i = 0; i < _attachedSliders.Length; i++)
        {

            if (i <= 3)
            {
                if (AllSliders[_attachedSliders[i]].GetComponent<GetConnectedNodes>().isFilled == true)      //<Slider>().value == 1)
                {
                    boxSideFill++;
                    if (boxSideFill == 4)
                    {
                        if (GameHandler.Instance.PlayerTurn == true)
                        {
                            isPlayerBoxFilled = true;
                        }
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
                if (AllSliders[_attachedSliders[i]].GetComponent<GetConnectedNodes>().isFilled == true)           //< Slider>().value == 1)
                {
                    boxSideFill++;
                    if (boxSideFill == 3)
                    {
                        if (GameHandler.Instance.PlayerTurn == true)
                        {
                            isPlayerBoxFilled = true;
                        }
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
        if (GameHandler.Instance.PlayerTurn == true && isPlayerBoxFilled == false)
        {
            AITurnStart();
        }
        else if (GameHandler.Instance.PlayerTurn == false)
        {
            PlayerTurn();
        }

        if (isPlayerBoxFilled == true)
        {
            isPlayerBoxFilled = false;
        }
    }


    public void AITurnStart()
    {
        for (int i = 0; i < AllSliders.Count; i++)
        {
            AllSliders[i].GetComponent<Slider>().transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = GameHandler.Instance.ref_AIScript.activeGridNormalColor;
        }

        Time.timeScale = 0.5f;
        GameHandler.Instance.ref_LevelManager.PlayerTurnText.SetActive(false);
        GameHandler.Instance.ref_LevelManager.AITurnText.SetActive(true);
        StartCoroutine(GameHandler.Instance.ref_AIScript.AITurn());
    }

    public void PlayerTurn()
    {
        Time.timeScale = 1f;
        GameHandler.Instance.ref_LevelManager.AITurnText.SetActive(false);
        GameHandler.Instance.ref_LevelManager.PlayerTurnText.SetActive(true);
        for (int i = 0; i < AllSliders.Count; i++)
        {
            AllSliders[i].GetComponent<Slider>().interactable = true;

            AllSliders[i].transform.GetChild(3).gameObject.GetComponent<Button>().interactable = true;
            if (AllSliders[i].GetComponent<GetConnectedNodes>().isFilled == true)
            {
                AllSliders[i].GetComponent<Slider>().interactable = false;
                AllSliders[i].transform.GetChild(3).gameObject.GetComponent<Button>().interactable = false;
            }
        }

        GameHandler.Instance.PlayerTurn = true;
    }


    private void BoxFilled()
    {
        boxSideFill = 0;

        StartCoroutine(GameHandler.Instance.ref_LevelManager.LevelComplete());        
    }


}