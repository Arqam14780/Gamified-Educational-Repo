using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIPlayer : MonoBehaviour
{
    public Color[] NormalSliderFilledColor;
    public Color[] AISliderFilledColor;
    public Color activeGridColor;
    public Color activeGridNormalColor;
    List<GameObject> AllSliders;
    List<GameObject> AllNodes;
    private int emptyBox = 0;
    int[] _connectedSliders;
    int[] _connectedNodes;
    bool isTwoJoiningBoxEmpty = false;
    bool isThreeSideEmpty = false;
    bool isTwoSideEmpty = false;
    List<int> AllNodeFourSideEmpty = new List<int>();
    List<int> AllNodeThreeSideEmpty = new List<int>();
    List<int> AllNodeTwoSideEmpty = new List<int>();

    public bool isMakeBox = false;

    bool isNewLineStart = false;


    private void Start()
    {
        GameHandler.Instance.ref_AIScript = this;
    }

    public void UpdateGridColor()
    {
        activeGridNormalColor = NormalSliderFilledColor[PlayerPrefs.GetInt("GridSelected")];
        activeGridColor = AISliderFilledColor[PlayerPrefs.GetInt("GridSelected")];
    }

    public IEnumerator AITurn()
    {
        isMakeBox = false;
        AllSliders = GameHandler.Instance.ref_SlidersAndNodes.AllSliders;
        AllNodes = GameHandler.Instance.ref_SlidersAndNodes.AllNodes;

        for (int i = 0; i < AllSliders.Count; i++)
        {
            AllSliders[i].transform.GetChild(3).gameObject.GetComponent<Button>().interactable = false;
            AllSliders[i].GetComponent<Slider>().interactable = false;
        }

        yield return new WaitForSeconds(0.3f);

        MakeABox();

        yield return null;
    }

    void MakeABox()
    {
            for (int i = 0; i < AllNodes.Count; i++)
            {
                if (AllNodes[i].GetComponent<GetConnectedSliders>().SlidersEmptySides == 1)                                      // this condition fill slider if 3 side fill already                       
                {
                    _connectedSliders = AllNodes[i].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber;

                    for (int j = 0; j < 4; j++)
                    {
                        if (AllSliders[_connectedSliders[j]].GetComponent<GetConnectedNodes>().isFilled == false)
                        {
                            AllSliders[_connectedSliders[j]].GetComponent<Slider>().transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = activeGridColor;
                            isMakeBox = true;
                            AllSliders[_connectedSliders[j]].GetComponent<GetConnectedNodes>().isFilled = true;
                            AllSliders[_connectedSliders[j]].GetComponent<Slider>().value = 1;
                        }
                    }
                }           
            }

            for(int j=0; j < AllNodes.Count; j++)
            {
                if (AllNodes[j].GetComponent<GetConnectedSliders>().SlidersEmptySides == 1)
                {
                 Invoke("MakeABox", 0.4f);
                return;
                }
            }
       
        isMakeBox = false;
        isNewLineStart = true;

        if (isNewLineStart == true)
        {
            Invoke("NewLine1stSenario", 0.7f);
            isNewLineStart = false;           
        }

    }




    void NewLine1stSenario()
    {
        for (int l = 0; l < AllSliders.Count; l++)                                                                          // fill slider if 2 joining box are empty
            {
                _connectedNodes = AllSliders[l].GetComponent<GetConnectedNodes>().GetConnectedNodeNumber;
                emptyBox = 0;

            for (int m = 0; m < _connectedNodes.Length; m++)
                {
                    if (AllNodes[_connectedNodes[m]].GetComponent<GetConnectedSliders>().SlidersEmptySides == 4)
                    {                        
                        emptyBox++;
                    
                    if (emptyBox == 2)
                        {
                            if (!AllNodeFourSideEmpty.Contains(_connectedNodes[m]))
                            {
                                AllNodeFourSideEmpty.Add(_connectedNodes[m]);                              
                            }
                            isTwoJoiningBoxEmpty = true;
                        }                       
                    }                    
                }
            }
            if (isTwoJoiningBoxEmpty == true )
            {
            MakeNewLine1();
            }
            else if (isTwoJoiningBoxEmpty == false)
            {
            NewLine2ndSenario();
            }
        }

        void MakeNewLine1()
        {    
        isTwoJoiningBoxEmpty = false;    

        int _randomNode = Random.Range(0, AllNodeFourSideEmpty.Count);

        int _randomIndex = Random.Range(0, 4);

        GameHandler.Instance.PlayerTurn = false;
        AllSliders[AllNodes[AllNodeFourSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[_randomIndex]].GetComponent<Slider>().transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = activeGridColor;
        AllSliders[AllNodes[AllNodeFourSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[_randomIndex]].GetComponent<GetConnectedNodes>().isFilled = true;
        AllSliders[AllNodes[AllNodeFourSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[_randomIndex]].GetComponent<Slider>().value = 1;

        AllNodeFourSideEmpty.Clear();
    }


    void NewLine2ndSenario()
    {

        for (int l = 0; l < AllSliders.Count; l++)                                                                          // fill slider if 1 side is filled
        {
            _connectedNodes = AllSliders[l].GetComponent<GetConnectedNodes>().GetConnectedNodeNumber;

            for (int m = 0; m < _connectedNodes.Length; m++)
            {
                if (AllNodes[_connectedNodes[m]].GetComponent<GetConnectedSliders>().SlidersEmptySides == 3)
                {
                    
                    if (!AllNodeThreeSideEmpty.Contains(_connectedNodes[m]))
                    {
                        AllNodeThreeSideEmpty.Add(_connectedNodes[m]);
                    }
                    isThreeSideEmpty = true;
                }            
            }
        }
        if (isThreeSideEmpty == true)
        {
            MakeNewLine2();
        }
        else if (isThreeSideEmpty == false)
        {
            NewLine3rdSenario();
        }
    }

    void MakeNewLine2()
    {
        if (isThreeSideEmpty == true)
        {
            isThreeSideEmpty = false;
            int _randomNode = Random.Range(0, AllNodeThreeSideEmpty.Count);

            int _randomIndex = 0;


            for (int i = 0; i < 4; i++)
            {
                if (AllSliders[AllNodes[AllNodeThreeSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[i]].GetComponent<GetConnectedNodes>().isFilled == false)
                {
                    _randomIndex = i;
                }

            }

            GameHandler.Instance.PlayerTurn = false;
            AllSliders[AllNodes[AllNodeThreeSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[_randomIndex]].GetComponent<Slider>().transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = activeGridColor;
            AllSliders[AllNodes[AllNodeThreeSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[_randomIndex]].GetComponent<GetConnectedNodes>().isFilled = true;
            AllSliders[AllNodes[AllNodeThreeSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[_randomIndex]].GetComponent<Slider>().value = 1;            

            AllNodeThreeSideEmpty.Clear();
        }
    }

    void NewLine3rdSenario()
    {

        for (int l = 0; l < AllSliders.Count; l++)                                                                          // fill slider if 2 side is filled
        {
            _connectedNodes = AllSliders[l].GetComponent<GetConnectedNodes>().GetConnectedNodeNumber;

            for (int m = 0; m < _connectedNodes.Length; m++)
            {
                if (AllNodes[_connectedNodes[m]].GetComponent<GetConnectedSliders>().SlidersEmptySides == 2)
                {
                    isTwoSideEmpty = true;
                    if (!AllNodeTwoSideEmpty.Contains(_connectedNodes[m]))
                    {
                        AllNodeTwoSideEmpty.Add(_connectedNodes[m]);
                    }
                }
            }
        }

        if (isTwoSideEmpty == true)
        {
            MakeNewLine3();
        }
    }

    void MakeNewLine3()
    {
        if (isTwoSideEmpty == true)
        {
            isTwoSideEmpty = false;
            int _randomNode = Random.Range(0, AllNodeTwoSideEmpty.Count);

            int _randomIndex = 0;

            for (int i = 0; i < 4; i++)
            {
                if (AllSliders[AllNodes[AllNodeTwoSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[i]].GetComponent<GetConnectedNodes>().isFilled == false)
                {
                    _randomIndex = i;
                }

            }

            GameHandler.Instance.PlayerTurn = false;

            AllSliders[AllNodes[AllNodeTwoSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[_randomIndex]].GetComponent<Slider>().transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = activeGridColor;
            AllSliders[AllNodes[AllNodeTwoSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[_randomIndex]].GetComponent<GetConnectedNodes>().isFilled = true;
            AllSliders[AllNodes[AllNodeTwoSideEmpty[_randomNode]].GetComponent<GetConnectedSliders>().GetConnectedSliderNumber[_randomIndex]].GetComponent<Slider>().value = 1;

            AllNodeTwoSideEmpty.Clear();
        }

    }
    
}