using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableDisableScript : MonoBehaviour
{
    public UnityEvent whenIEnable;
    public UnityEvent whenIDisable;
    // Start is called before the first frame update
    private void OnEnable()
    {
        whenIEnable.Invoke();
    }

    private void OnDisable()
    {
        whenIDisable.Invoke();
    }

}
