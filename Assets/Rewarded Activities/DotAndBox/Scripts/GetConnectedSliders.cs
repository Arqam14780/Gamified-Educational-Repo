using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetConnectedSliders : MonoBehaviour
{
    public int[] GetConnectedSliderNumber;
    public int SlidersEmptySides = 4;

    private void Start()
    {
        GameHandler.Instance.ref_GetConnectedSliders = this;
    }

}
