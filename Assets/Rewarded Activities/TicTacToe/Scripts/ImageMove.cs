using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMove : MonoBehaviour
{
    public Transform imageEndingPoint;
    public int speed = 100;
    Vector3 positionToSave;
    
    private void Start()
    {
        positionToSave = transform.position;
    }
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, imageEndingPoint.position, step);
        if (transform.position == imageEndingPoint.position)
        {          
            transform.position = positionToSave;
        }
    }
}
