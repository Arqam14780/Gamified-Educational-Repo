using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderButton : MonoBehaviour
{
 
    public void OnValueChanged()
    {
        gameObject.transform.parent.GetComponent<GetConnectedNodes>().OnValueChangedFunction();      
    }
}
