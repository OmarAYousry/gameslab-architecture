using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver_UIImage : MonoBehaviour {

    GameObject img2;

	// Use this for initialization
	void Start () {

        img2 = GameObject.Find("Image_FireResistance");
        img2.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver()
    {
        img2.SetActive(true);
    }

    void OnMouseExit()
    {
        img2.SetActive(false);
    }
}
