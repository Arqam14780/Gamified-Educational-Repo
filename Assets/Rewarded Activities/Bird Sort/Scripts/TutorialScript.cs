using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    BirdStack ref_BirdStack;
    public GameObject leftPointer;
    public GameObject rightPointer;
    public BoxCollider rightBoxCol;
    
    void Start()
    {
        ref_BirdStack = GetComponent<BirdStack>();
        rightBoxCol.enabled = false;
        leftPointer.SetActive(false);
        rightPointer.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameLogic.instance.isSelected)
        {
            leftPointer.SetActive(true);
            rightPointer.SetActive(false);
            rightBoxCol.enabled = true;
        }
        if(GameLogic.instance.selectedItem.Count <= 0)
            leftPointer.SetActive(false);
    }


}
