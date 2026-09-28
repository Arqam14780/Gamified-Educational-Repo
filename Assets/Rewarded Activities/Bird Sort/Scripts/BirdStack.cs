using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdStack : MonoBehaviour
{
    public int birdStackId = 1;
    public Animator branchAnimator;
    public Transform flyTarget;
    public List<GameObject> birdList;
    [Space(5)]
    public Transform[] sitPoints;
    [Space(5)]
    public int totalSlot = 4;
    public int freeSlot = 4;
    public int mergeCound = 0;

    public bool isReturn;
    public int posInd = 0;
    public int stackLengthCounter = 0;


    private void Start()
    {
        int reverse = sitPoints.Length - 1;       // my change
        int temp = birdList.Count - 1;

        for (int i = 0; i < birdList.Count; i++)
        {
            birdList[temp].GetComponent<BirdController>().destination = sitPoints[reverse];  // i
            birdList[i].GetComponent<BirdController>().audioComponent.Play();
            birdList[i].GetComponent<BirdController>().ref_BirdStack = this;
            if (i + 1 < birdList.Count && !isReturn)
            {
                if (birdList[0].GetComponent<BirdController>().birdId == birdList[i + 1].GetComponent<BirdController>().birdId)
                    mergeCound++;
                else
                    isReturn = true;
            }
            reverse--;
            temp--;
        }
        if (birdList.Count > 0)
            freeSlot = totalSlot - birdList.Count;

        posInd = freeSlot - 1; // totalSlot - 1;

        StartCoroutine(ShakeBranch(1f));

    }

    private void OnMouseDown()
    {
        if (GameLogic.instance.isSelected)
        {
            // if item selected then check combination
            if (birdList.Count <= 0)
            {
                UpdateItemPos();
            }
            else if (birdList.Count > 0 && GameLogic.instance.selectedItem[0].GetComponent<BirdController>().ref_BirdStack.birdStackId
                != birdStackId)
            {
                if (GameLogic.instance.selectedItem[0].GetComponent<BirdController>().birdId == birdList[0].GetComponent<BirdController>().birdId
                    && freeSlot >= GameLogic.instance.selectedItem.Count)           // chang this condition if selected item id is same then change pos
                {
                    //  Debug.Log("Chage Bird Pos");
                    UpdateItemPos();
                }
                else
                {
                    // select current item and deselect already selected
                    ModifiedSelection();
                }
                // match same id and check how much slot is free
            }
        }
        else
        {
            // select current item and deselect already selected
            ModifiedSelection();
        }
    }

    void UpdateItemPos()
    {
        for (int i = GameLogic.instance.selectedItem.Count - 1; i >= 0; i--)
        {
            GameLogic.instance.selectedItem[i].GetComponent<BirdController>().destination = sitPoints[posInd];
            GameLogic.instance.selectedItem[i].GetComponent<BirdController>().audioComponent.Play();
            posInd--;
            if (posInd < 0) posInd = totalSlot - 1;
        }
        for (int i = 0; i < GameLogic.instance.selectedItem.Count; i++)
        {
            birdList.Add(GameLogic.instance.selectedItem[i]);
        }
        stackLengthCounter = birdList.Count;
        DeselectPreviousItems();
        UpdatePreviousStack();
        isReturn = false;                                          // my changes
        //Invoke("UpdateSelectedStack", 1f);
        UpdateSelectedStack();
        GameLogic.instance.isSelected = false;

    }

    void UpdatePreviousStack()
    {
        BirdStack ref_stack = birdList[birdList.Count - 1].GetComponent<BirdController>().ref_BirdStack;
        ref_stack.branchAnimator.SetTrigger("Shake");
        ref_stack.isReturn = false;


        //  Debug.Log("<color=red>" + ref_stack.gameObject.name + "</color>");
        for (int i = 0; i <= ref_stack.mergeCound; i++)
        {
            ref_stack.birdList.RemoveAt(0);
        }

        int sitPoint = ref_stack.sitPoints.Length - 1;
        for (int j = ref_stack.birdList.Count - 1; j >= 0; j--)
        {
            ref_stack.birdList[j].GetComponent<BirdController>().destination = ref_stack.sitPoints[sitPoint];
            ref_stack.birdList[j].GetComponent<BirdController>().audioComponent.Play();
            sitPoint--;
        }
        ref_stack.posInd = (ref_stack.totalSlot - ref_stack.birdList.Count) - 1;

        ref_stack.mergeCound = 0;
        for (int i = 0; i < ref_stack.birdList.Count; i++)
        {
            if (i + 1 < ref_stack.birdList.Count && !ref_stack.isReturn)
            {
                if (ref_stack.birdList[0].GetComponent<BirdController>().birdId == ref_stack.birdList[i + 1].GetComponent<BirdController>().birdId)
                    ref_stack.mergeCound++;
                else
                    ref_stack.isReturn = true;
            }
        }
        ref_stack.freeSlot = ref_stack.totalSlot - ref_stack.birdList.Count;
    }

    void UpdateSelectedStack()
    {
        mergeCound = 0;
        if (birdList.Count <= 0)
        {
            Debug.Log("Return");
            return;
        }
        int getSimilarItemId = 0;    // my changes
        for (int i = 0; i < birdList.Count; i++)
        {

            birdList[i].GetComponent<BirdController>().ref_BirdStack = this;
            // my changes           
            for (int j = i + 1; j < birdList.Count; j++)
            {
                if (!isReturn)
                {
                    if (birdList[i].GetComponent<BirdController>().birdId == birdList[j].GetComponent<BirdController>().birdId)
                    {
                        getSimilarItemId = birdList[i].GetComponent<BirdController>().birdId;
                        mergeCound++;
                    }
                }
            }
        }
        if (mergeCound > sitPoints.Length) mergeCound = sitPoints.Length;
        if (mergeCound > 0)
        {
            List<GameObject> similarItemList = new List<GameObject>();
            List<GameObject> otherItemList = new List<GameObject>();
            for (int k = 0; k < birdList.Count; k++)
            {
                if (birdList[k].GetComponent<BirdController>().birdId == getSimilarItemId)
                {
                    //      Debug.Log("Similar");
                    similarItemList.Add(birdList[k]);
                }
                else
                {
                    //     Debug.Log("Not Similar");
                    otherItemList.Add(birdList[k]);
                }
            }
            if (similarItemList.Count > 0)
                mergeCound = similarItemList.Count - 1;
            else
                mergeCound = 0;
            for (int i = 0; i < similarItemList.Count; i++)
            {
                if (similarItemList.Count > 0)
                {
                    //      Debug.Log("Similar2");
                    birdList[i] = similarItemList[i];
                }
            }
            int temp = 0;
            for (int i = similarItemList.Count; i < birdList.Count; i++)
            {
                if (otherItemList.Count > 0)
                {
                    //      Debug.Log("Not Similar2:" + i);
                    birdList[i] = otherItemList[temp];
                    temp++;
                }
            }
        }
        isReturn = true;

        freeSlot = totalSlot - birdList.Count;

        if (freeSlot >= totalSlot)
        {
            isReturn = false;
            posInd = totalSlot - 1;
        }

        StartCoroutine(ShakeBranch(0.5f));
        if (mergeCound >= 1)
        {
            int id = birdList[0].GetComponent<BirdController>().birdId;
            int countCombination = 0;
            for (int i = 0; i < GameLogic.instance.birdsContainer.Count; i++)
            {
                if (GameLogic.instance.birdsContainer[i].GetComponent<BirdController>().birdId == id)
                    countCombination++;
            }
            //    Debug.Log("countCombination>>>>" + countCombination);
            if (mergeCound >= countCombination - 1)
                StartCoroutine(UpdateCombination());
        }
    }

    IEnumerator ShakeBranch(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        branchAnimator.SetTrigger("Shake");
    }

    void DeselectPreviousItems()
    {
        for (int i = 0; i < GameLogic.instance.selectedItem.Count; i++)
            GameLogic.instance.selectedItem[i].GetComponent<BirdController>().meshComponent.GetComponent<HighlightPlus.HighlightEffect>().enabled = false;
        GameLogic.instance.selectedItem.Clear();
    }

    void ModifiedSelection()
    {
        if (birdList.Count > 0)
        {
            if (GameLogic.instance.selectedItem.Count > 0)
                DeselectPreviousItems();

            for (int i = 0; i <= mergeCound; i++)
            {
                birdList[i].GetComponent<BirdController>().meshComponent.GetComponent<HighlightPlus.HighlightEffect>().enabled = true;
                GameLogic.instance.selectedItem.Add(birdList[i]);
            }
            GameLogic.instance.isSelected = true;
        }
    }

    IEnumerator UpdateCombination()
    {
        yield return new WaitForSecondsRealtime(1f);

        for (int i = 0; i <= mergeCound; i++)
        {
            birdList[i].GetComponent<BirdController>().destination = flyTarget;
            birdList[i].GetComponent<BirdController>().audioComponent.Play();
            //  posInd--;                   // my changes
        }

        for (int i = 0; i <= mergeCound; i++)
            birdList.RemoveAt(0);

        freeSlot = totalSlot - birdList.Count;
        if (freeSlot >= totalSlot)
        {
            isReturn = false;
            posInd = (totalSlot - posInd) - 1;
            //  posInd = totalSlot - 1;
        }
        mergeCound = 0;

        // my changes start
        if (birdList.Count <= 0)
            posInd = -1;
        else
            posInd--;

        if (posInd < 0)
        {
            posInd = birdList.Count;
            posInd = (totalSlot - posInd) - 1;
        }   // my changes end

        // check For lvl Complete
        GameController2.instance.ref_LevelManager.combinationCounter++;
        if (PlayerPrefs.GetInt("CurrentLevel", 0) <= 9)
        {
            if (GameController2.instance.ref_LevelManager.combinationCounter >= GameController2.instance.ref_LevelManager.Levels[PlayerPrefs.GetInt("CurrentLevel", 0)].totalCombination)
            {
                StartCoroutine(GameController2.instance.ref_LevelManager.LevelCompleted());
            }
        }
        else
        {
            if (GameController2.instance.ref_LevelManager.combinationCounter >= GameController2.instance.ref_LevelManager.Levels[GameController2.instance.ref_LevelManager.currentLevel].totalCombination)
            {
                StartCoroutine(GameController2.instance.ref_LevelManager.LevelCompleted());
            }
        }
    }


}


