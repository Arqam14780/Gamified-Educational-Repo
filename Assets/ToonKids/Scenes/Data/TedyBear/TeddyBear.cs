using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeddyBear
{
    public class TeddyBear : MonoBehaviour
    {
        Animator anim;
        float looktime;
        float when;

        void Start()
        {
            looktime = 0f;
            anim = transform.GetComponent<Animator>();
            when = 2f;
        }


        void Update()
        {
            looktime += Time.deltaTime;
            if (looktime > when)
            {
                anim.CrossFade("look", 0.125f);
                when = Random.Range(4f, 12f);
                looktime = -1f;
            }
        }
    }
}