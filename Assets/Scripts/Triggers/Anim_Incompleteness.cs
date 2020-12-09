using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Incompleteness : MonoBehaviour {

    public Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        //anim.enabled = false;
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            //anim.enabled = true;
            //GetComponent<Animator>().Rebind();
            anim.Play("Incompleteness_Wall North");
        }
        if (Input.GetKeyDown("o"))
        {
            //anim.enabled = false;
            anim.Play("New State");
        }

    }
}
