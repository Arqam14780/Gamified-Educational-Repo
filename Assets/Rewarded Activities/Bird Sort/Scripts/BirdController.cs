using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public BirdStack ref_BirdStack;
    public int birdId = 1;
    public GameObject meshComponent;
    public float duration = 2f;
    public float rotationSpeed = 100f;
    public Transform destination;

    private Animator birdAc;
    [HideInInspector]
    public AudioSource audioComponent;
    private void Awake()
    {
        birdAc = this.gameObject.GetComponent<Animator>();
        audioComponent = this.gameObject.GetComponent<AudioSource>();
    }
    
    void Start(){
        GameLogic.instance.birdsContainer.Add(this.gameObject);
    }
    private void Update()
    {
        if (destination)
        {
            float angle = Vector3.Angle(transform.position, destination.position);
            transform.position = Vector3.MoveTowards(transform.position, destination.position, Time.deltaTime * duration);

            var step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, destination.rotation, step);

            if (angle <= 0)
            {
                birdAc.SetBool("fly", false);
                destination = null;
            }
            else
                birdAc.SetBool("fly", true);
        }
    }



}
