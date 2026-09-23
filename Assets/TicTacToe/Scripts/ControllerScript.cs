using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript :  MonoBehaviour 
{
    public int AllBoxesLength;
    public int sequenceLength;
    public List<GameObject> AllBoxes = new List<GameObject>();
    GameObject activeObject;
    int activeSequenceLength = 0;
    bool isSequenceFormed = false;
    int multiplayerTurn = 1;
    public int firstTurnIndex = 1;
    public GameObject effectImagesContainer;
    public List<GameObject> ActiveIndexes;


    private void Awake()
    {
        GameController.Instance.ref_ControllerScript = this;
        for (int i = 0; i < AllBoxesLength; i++)
        {
            AllBoxes.Add(transform.GetChild(i).gameObject);
        }
    }

    public void PlayerFirstTurn()
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



        if (GameController.Instance.isMultiPlayerModeActive == false)
        {
            if (firstTurnIndex == 1)
            {
                for (int i = 0; i < AllBoxesLength; i++)
                {
                    AllBoxes[i].gameObject.GetComponent<Button>().interactable = true;
                    GameController.Instance.ref_SetSelectionInGameplay.player2.GetChild(0).gameObject.SetActive(false);
                    GameController.Instance.ref_SetSelectionInGameplay.player1.GetChild(0).gameObject.SetActive(true);
                }
            }
            else if (firstTurnIndex == 2)
            {
                for (int i = 0; i < AllBoxesLength; i++)
                {
                    AllBoxes[i].gameObject.GetComponent<Button>().interactable = false;
                    GameController.Instance.ref_SetSelectionInGameplay.player1.GetChild(0).gameObject.SetActive(false);
                    GameController.Instance.ref_SetSelectionInGameplay.player2.GetChild(0).gameObject.SetActive(true);
                }
                GameController.Instance.ref_AIScript.AITurn();
            }
        }
        else if (GameController.Instance.isMultiPlayerModeActive == true)
        {
            if (firstTurnIndex == 2)
            {
                multiplayerTurn = 2;
            }
        }
    }



    public void ImageTurnOn(int index)
    {
        if (GameController.Instance.isMultiPlayerModeActive == false)
        {
            isSequenceFormed = false;
            activeObject = AllBoxes[index].gameObject;
            activeObject.gameObject.GetComponent<SequenceElements>().isPressed = true;
            activeObject.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(CheckSequenceCreated(0));
        }
        else if (GameController.Instance.isMultiPlayerModeActive == true)
        {
            if (multiplayerTurn == 1)
            {
                MakeImageVisible(index, 0);
            }
            else if (multiplayerTurn == 2)
            {
                MakeImageVisible(index, 1);
            }
        }
    }

    void MakeImageVisible(int index, int childIndex)
    {
        isSequenceFormed = false;
        activeObject = AllBoxes[index].gameObject;
        activeObject.gameObject.GetComponent<SequenceElements>().isPressed = true;
        activeObject.transform.GetChild(childIndex).gameObject.SetActive(true);
        activeObject.GetComponent<Button>().interactable = false;
        StartCoroutine(CheckSequenceCreated(childIndex));
    }

    IEnumerator CheckSequenceCreated(int childIndex)
    {
      
        activeSequenceLength = 0;
        for (int i = 0; i < activeObject.GetComponent<SequenceElements>().SequencePosibilities.Length; i++)
        {
            for (int j = 0; j < activeObject.GetComponent<SequenceElements>().SequencePosibilities[i].SequenceBoxNumbers.Length; j++)
            {
                if (AllBoxes[activeObject.GetComponent<SequenceElements>().SequencePosibilities[i].SequenceBoxNumbers[j]].transform.GetChild(childIndex).gameObject.activeInHierarchy)
                {
                    
                    activeSequenceLength++;
                    if (activeSequenceLength == sequenceLength)
                    {
                        isSequenceFormed = true;
                        for (int k = 0; k < AllBoxes.Count; k++)
                        {
                            AllBoxes[k].GetComponent<Button>().interactable = false;
                        }
                        if (GameController.Instance.isMultiPlayerModeActive == false)
                        {
                            firstTurnIndex = 1;
                            
                            GameController.Instance.ref_GameplayUI.InputButton("LevelWin");
                        }
                        else if(GameController.Instance.isMultiPlayerModeActive == true)
                        {
                            if (multiplayerTurn == 1)
                            {
                                firstTurnIndex = 1;
                                GameController.Instance.ref_GameplayUI.InputButton("LevelWin");
                            }
                            else if (multiplayerTurn == 2)
                            {
                                firstTurnIndex = 2;
                                GameController.Instance.ref_GameplayUI.InputButton("LevelLost");
                            }
                        }
                        EnableEffects(activeObject, i);
                        
                    }
                }
                if (j == 1 && activeSequenceLength < sequenceLength)
                {
                    activeSequenceLength = 0;
                }

            }
        }
        if (!isSequenceFormed)
        {
            int _a = 0;
            for (int x = 0; x < AllBoxes.Count; x++)
            {            
                if (AllBoxes[x].GetComponent<SequenceElements>().isPressed)
                {
                    _a++;
                    if (_a == GameController.Instance.ref_ControllerScript.AllBoxesLength)
                    {
                        GameController.Instance.ref_GameplayUI.InputButton("LevelTie");
                    }
                }
            }

            if (GameController.Instance.isMultiPlayerModeActive == false)
            {              
                for (int i = 0; i < AllBoxes.Count; i++)
                {
                    AllBoxes[i].GetComponent<Button>().interactable = false;
                }
                GameController.Instance.ref_AIScript.AITurn();
            }
            else if(GameController.Instance.isMultiPlayerModeActive == true)
            {
                TurnChanged(multiplayerTurn);
            }
        }
        yield return null;
    }

    public void EnableEffects(GameObject activeObject,int sequencePosibilityIndex)
    {
        ActiveIndexes.Add(activeObject);
        
        for(int i=0; i< activeObject.GetComponent<SequenceElements>().SequencePosibilities[sequencePosibilityIndex].SequenceBoxNumbers.Length; i++)
        {
            ActiveIndexes.Add(AllBoxes[activeObject.GetComponent<SequenceElements>().SequencePosibilities[sequencePosibilityIndex].SequenceBoxNumbers[i]]);
        }

        GameController.Instance.ref_SoundController.PlayEffectSound();

        if (GameController.GridSize == 1)
        {
            FirstGridEffect();
        }
        else if (GameController.GridSize == 2)
        {
            SecondGridEffect();
        }

    }


    void FirstGridEffect()
    {
        if (ActiveIndexes.Contains(AllBoxes[0]) && ActiveIndexes.Contains(AllBoxes[1]) && ActiveIndexes.Contains(AllBoxes[2]))
        {
            effectImagesContainer.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[3]) && ActiveIndexes.Contains(AllBoxes[4]) && ActiveIndexes.Contains(AllBoxes[5]))
        {
            effectImagesContainer.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[6]) && ActiveIndexes.Contains(AllBoxes[7]) && ActiveIndexes.Contains(AllBoxes[8]))
        {
            effectImagesContainer.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[0]) && ActiveIndexes.Contains(AllBoxes[3]) && ActiveIndexes.Contains(AllBoxes[6]))
        {
            effectImagesContainer.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[1]) && ActiveIndexes.Contains(AllBoxes[4]) && ActiveIndexes.Contains(AllBoxes[7]))
        {
            effectImagesContainer.transform.GetChild(4).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[2]) && ActiveIndexes.Contains(AllBoxes[5]) && ActiveIndexes.Contains(AllBoxes[8]))
        {
            effectImagesContainer.transform.GetChild(5).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[0]) && ActiveIndexes.Contains(AllBoxes[4]) && ActiveIndexes.Contains(AllBoxes[8]))
        {
            effectImagesContainer.transform.GetChild(6).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[2]) && ActiveIndexes.Contains(AllBoxes[4]) && ActiveIndexes.Contains(AllBoxes[6]))
        {
            effectImagesContainer.transform.GetChild(7).gameObject.SetActive(true);
        }
    }

    void SecondGridEffect()
    {
        if (ActiveIndexes.Contains(AllBoxes[0]) && ActiveIndexes.Contains(AllBoxes[1]) && ActiveIndexes.Contains(AllBoxes[2]))
        {
            effectImagesContainer.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[1]) && ActiveIndexes.Contains(AllBoxes[2]) && ActiveIndexes.Contains(AllBoxes[3]))
        {
            effectImagesContainer.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[4]) && ActiveIndexes.Contains(AllBoxes[5]) && ActiveIndexes.Contains(AllBoxes[6]))
        {
            effectImagesContainer.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[5]) && ActiveIndexes.Contains(AllBoxes[6]) && ActiveIndexes.Contains(AllBoxes[7]))
        {
            effectImagesContainer.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[8]) && ActiveIndexes.Contains(AllBoxes[9]) && ActiveIndexes.Contains(AllBoxes[10]))
        {
            effectImagesContainer.transform.GetChild(4).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[9]) && ActiveIndexes.Contains(AllBoxes[10]) && ActiveIndexes.Contains(AllBoxes[11]))
        {
            effectImagesContainer.transform.GetChild(5).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[12]) && ActiveIndexes.Contains(AllBoxes[13]) && ActiveIndexes.Contains(AllBoxes[14]))
        {
            effectImagesContainer.transform.GetChild(6).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[13]) && ActiveIndexes.Contains(AllBoxes[14]) && ActiveIndexes.Contains(AllBoxes[15]))
        {
            effectImagesContainer.transform.GetChild(7).gameObject.SetActive(true);
        }

        else if (ActiveIndexes.Contains(AllBoxes[0]) && ActiveIndexes.Contains(AllBoxes[4]) && ActiveIndexes.Contains(AllBoxes[8]))
        {
            effectImagesContainer.transform.GetChild(8).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[4]) && ActiveIndexes.Contains(AllBoxes[8]) && ActiveIndexes.Contains(AllBoxes[12]))
        {
            effectImagesContainer.transform.GetChild(9).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[1]) && ActiveIndexes.Contains(AllBoxes[5]) && ActiveIndexes.Contains(AllBoxes[9]))
        {
            effectImagesContainer.transform.GetChild(10).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[5]) && ActiveIndexes.Contains(AllBoxes[9]) && ActiveIndexes.Contains(AllBoxes[13]))
        {
            effectImagesContainer.transform.GetChild(11).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[2]) && ActiveIndexes.Contains(AllBoxes[6]) && ActiveIndexes.Contains(AllBoxes[10]))
        {
            effectImagesContainer.transform.GetChild(12).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[6]) && ActiveIndexes.Contains(AllBoxes[10]) && ActiveIndexes.Contains(AllBoxes[14]))
        {
            effectImagesContainer.transform.GetChild(13).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[3]) && ActiveIndexes.Contains(AllBoxes[7]) && ActiveIndexes.Contains(AllBoxes[11]))
        {
            effectImagesContainer.transform.GetChild(14).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[7]) && ActiveIndexes.Contains(AllBoxes[11]) && ActiveIndexes.Contains(AllBoxes[15]))
        {
            effectImagesContainer.transform.GetChild(15).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[0]) && ActiveIndexes.Contains(AllBoxes[5]) && ActiveIndexes.Contains(AllBoxes[10]))
        {
            effectImagesContainer.transform.GetChild(16).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[1]) && ActiveIndexes.Contains(AllBoxes[6]) && ActiveIndexes.Contains(AllBoxes[11]))
        {
            effectImagesContainer.transform.GetChild(17).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[4]) && ActiveIndexes.Contains(AllBoxes[9]) && ActiveIndexes.Contains(AllBoxes[14]))
        {
            effectImagesContainer.transform.GetChild(18).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[5]) && ActiveIndexes.Contains(AllBoxes[10]) && ActiveIndexes.Contains(AllBoxes[15]))
        {
            effectImagesContainer.transform.GetChild(19).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[3]) && ActiveIndexes.Contains(AllBoxes[6]) && ActiveIndexes.Contains(AllBoxes[9]))
        {
            effectImagesContainer.transform.GetChild(20).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[2]) && ActiveIndexes.Contains(AllBoxes[5]) && ActiveIndexes.Contains(AllBoxes[8]))
        {
            effectImagesContainer.transform.GetChild(21).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[7]) && ActiveIndexes.Contains(AllBoxes[10]) && ActiveIndexes.Contains(AllBoxes[13]))
        {
            effectImagesContainer.transform.GetChild(22).gameObject.SetActive(true);
        }
        else if (ActiveIndexes.Contains(AllBoxes[6]) && ActiveIndexes.Contains(AllBoxes[9]) && ActiveIndexes.Contains(AllBoxes[12]))
        {
            effectImagesContainer.transform.GetChild(23).gameObject.SetActive(true);
        }
    }



    void TurnChanged(int playerTurn)
    {
        switch (playerTurn) {
            case 1:
                multiplayerTurn = 2;
                break;
            case 2:
                multiplayerTurn = 1;
                break;
        }
    }
}
