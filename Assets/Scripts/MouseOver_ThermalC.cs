﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver_ThermalC : MonoBehaviour
{

    GameObject img;

    // Use this for initialization
    void Start()
    {

        img = GameObject.Find("Image_ThermalC");
        img.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        img.SetActive(true);
    }

    void OnMouseExit()
    {
        img.SetActive(false);
    }
}
