using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SequenceElement
{
    public int[] SequenceBoxNumbers;
}

public class SequenceElements : MonoBehaviour
{

    private void Start()
    {
        GameController.Instance.ref_SequenceElements = this;
    }
    public SequenceElement[] SequencePosibilities;
    public bool isPressed = false;
}
