using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIScript : MonoBehaviour
{
    public List<GameObject> AllBoxes;
    public List<int> EmptyBoxes;
    public List<int> ActivePlayerBoxes;
    public List<int> ActiveAIBoxes;
    int index = 0;

    private void Start()
    {
        GameController.Instance.ref_AIScript = this;
    }
    public void AITurn()
    {
        GameController.Instance.ref_SetSelectionInGameplay.player1.GetChild(0).gameObject.SetActive(false);
        GameController.Instance.ref_SetSelectionInGameplay.player2.GetChild(0).gameObject.SetActive(true);
        EmptyBoxes.Clear();
        ActivePlayerBoxes.Clear();
        ActiveAIBoxes.Clear();
        AllBoxes = GameController.Instance.ref_ControllerScript.AllBoxes;
        for (int a = 0; a < AllBoxes.Count; a++)
        {
            if (AllBoxes[a].GetComponent<SequenceElements>().isPressed == false)
            {
                EmptyBoxes.Add(a);
            }
        }
        Invoke("Scenario1",1f);
    }
    void Scenario1()
    {
        index = 0;
        for (int i = 0; i < AllBoxes.Count; i++)
        {
            if (AllBoxes[i].transform.GetChild(1).gameObject.activeInHierarchy == false)
            {
                index++;
                if (index == AllBoxes.Count)
                {
                    int _randomNumber = Random.Range(0, EmptyBoxes.Count);
                    AllBoxes[EmptyBoxes[_randomNumber]].transform.GetChild(1).gameObject.SetActive(true);
                    AllBoxes[EmptyBoxes[_randomNumber]].GetComponent<SequenceElements>().isPressed = true;
                    PlayerTurn();
                }
                else if (i == AllBoxes.Count - 1 && index < AllBoxes.Count)
                {
                    Scenario2();
                }
            }
            else if (i == AllBoxes.Count - 1 && index < AllBoxes.Count)
            {
                Scenario2();
            }
        }
    }

    void Scenario2()
    {
        bool _isAITurn = true;

        for (int i = 0; i < AllBoxes.Count; i++)
        {
            if (AllBoxes[i].transform.GetChild(1).gameObject.activeInHierarchy)
            {
                ActiveAIBoxes.Add(i);
            }
        }

        for (int j = 0; j < ActiveAIBoxes.Count; j++)
        {

            SequenceElements _currentBox = AllBoxes[ActiveAIBoxes[j]].GetComponent<SequenceElements>();


            for (int k = 0; k < _currentBox.SequencePosibilities.Length; k++)
            {
                for (int l = 0; l < _currentBox.SequencePosibilities[k].SequenceBoxNumbers.Length; l++)
                {
                    if (AllBoxes[_currentBox.SequencePosibilities[k].SequenceBoxNumbers[l]].transform.GetChild(1).gameObject.activeInHierarchy)
                    {
                        for (int m = 0; m < GameController.Instance.ref_ControllerScript.sequenceLength; m++)
                        {
                            int _boxFilled = 0;

                            GameObject _SequenceBoxNumber = AllBoxes[_currentBox.SequencePosibilities[k].SequenceBoxNumbers[m]];
                            if (_SequenceBoxNumber.transform.GetChild(1).gameObject.activeInHierarchy == false && _isAITurn == true && EmptyBoxes.Contains(_currentBox.SequencePosibilities[k].SequenceBoxNumbers[m]))
                            {
                                _boxFilled++;
                                _isAITurn = false;
                                _SequenceBoxNumber.transform.GetChild(1).gameObject.SetActive(true);
                                _SequenceBoxNumber.GetComponent<SequenceElements>().isPressed = true;
                                GameController.Instance.ref_ControllerScript.firstTurnIndex = 2;
                                GameController.Instance.ref_GameplayUI.InputButton("LevelLost");

                                GameController.Instance.ref_ControllerScript.EnableEffects(AllBoxes[ActiveAIBoxes[j]], k);
                            }
                            else if (j == ActiveAIBoxes.Count - 1 && _isAITurn == true)
                            {
                                _isAITurn = false;
                                Scenario3();
                            }
                        }
                    }
                    else if (j == ActiveAIBoxes.Count - 1 && _isAITurn == true)
                    {
                        _isAITurn = false;
                        Scenario3();
                    }
                }
            }
        }
    }

    void Scenario3()
    {
        bool _isAITurn = true;

        for (int i = 0; i < AllBoxes.Count; i++)
        {
            if (AllBoxes[i].transform.GetChild(0).gameObject.activeInHierarchy)
            {
                ActivePlayerBoxes.Add(i);
            }
        }
        for (int j = 0; j < ActivePlayerBoxes.Count; j++)
        {
            SequenceElements _currentBox = AllBoxes[ActivePlayerBoxes[j]].GetComponent<SequenceElements>();

            for (int k = 0; k < _currentBox.SequencePosibilities.Length; k++)
            {
                for (int l = 0; l < _currentBox.SequencePosibilities[k].SequenceBoxNumbers.Length; l++)
                {

                    if (AllBoxes[_currentBox.SequencePosibilities[k].SequenceBoxNumbers[l]].transform.GetChild(0).gameObject.activeInHierarchy)
                    {

                        for (int m = 0; m < GameController.Instance.ref_ControllerScript.sequenceLength; m++)
                        {
                            GameObject _SequenceBoxNumber = AllBoxes[_currentBox.SequencePosibilities[k].SequenceBoxNumbers[m]];
                            if (_SequenceBoxNumber.transform.GetChild(0).gameObject.activeInHierarchy == false && _isAITurn == true && EmptyBoxes.Contains(_currentBox.SequencePosibilities[k].SequenceBoxNumbers[m]))
                            {
                                _isAITurn = false;
                                _SequenceBoxNumber.transform.GetChild(1).gameObject.SetActive(true);
                                _SequenceBoxNumber.GetComponent<SequenceElements>().isPressed = true;
                                PlayerTurn();
                            }
                            else if (j == ActivePlayerBoxes.Count - 1 && _isAITurn == true)
                            {
                                _isAITurn = false;
                                Scenario4();
                            }
                        }
                    }
                    else if (j == ActivePlayerBoxes.Count - 1 && _isAITurn == true)
                    {
                        _isAITurn = false;
                        Scenario4();
                    }
                }
            }
        }
    }
    void Scenario4()
    {
        bool _isAITurn = true;

        for (int i = 0; i < AllBoxes.Count; i++)
        {
            if (AllBoxes[i].transform.GetChild(1).gameObject.activeInHierarchy)
            {
                ActiveAIBoxes.Add(i);
            }
        }

        for (int j = 0; j < ActiveAIBoxes.Count; j++)
        {

            SequenceElements _currentBox = AllBoxes[ActiveAIBoxes[j]].GetComponent<SequenceElements>();

            for (int k = 0; k < _currentBox.SequencePosibilities.Length; k++)
            {
                for (int l = 0; l < _currentBox.SequencePosibilities[k].SequenceBoxNumbers.Length; l++)
                {
                    for (int m = 0; m < GameController.Instance.ref_ControllerScript.sequenceLength; m++)
                    {
                        GameObject _SequenceBoxNumber = AllBoxes[_currentBox.SequencePosibilities[k].SequenceBoxNumbers[m]];
                        if (_SequenceBoxNumber.transform.GetChild(1).gameObject.activeInHierarchy == false && _isAITurn == true && EmptyBoxes.Contains(_currentBox.SequencePosibilities[k].SequenceBoxNumbers[m]))
                        {
                            _isAITurn = false;
                            _SequenceBoxNumber.transform.GetChild(1).gameObject.SetActive(true);
                            _SequenceBoxNumber.GetComponent<SequenceElements>().isPressed = true;
                            PlayerTurn();
                        }
                        else if (j == ActiveAIBoxes.Count - 1 && _isAITurn == true)
                        {
                            _isAITurn = false;
                        }
                    }
                }
            }
        }
    }

    void PlayerTurn()
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

        for (int i = 0; i < AllBoxes.Count; i++)
        {
            AllBoxes[i].GetComponent<Button>().interactable = true;
        }

        for (int j = 0; j < AllBoxes.Count; j++)
        {

            if (AllBoxes[j].GetComponent<SequenceElements>().isPressed)
            {
                AllBoxes[j].GetComponent<Button>().interactable = false;
            }
        }
        GameController.Instance.ref_SetSelectionInGameplay.player2.GetChild(0).gameObject.SetActive(false);
        GameController.Instance.ref_SetSelectionInGameplay.player1.GetChild(0).gameObject.SetActive(true);
    }

}