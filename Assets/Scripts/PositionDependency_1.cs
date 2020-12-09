using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionDependency_1 : MonoBehaviour {

    GameObject wall;
    GameObject dep;
    Vector3 posWall = new Vector3();
    Vector3 posDep = new Vector3();

    // Use this for initialization
    void Start () {
        wall = GameObject.Find("Inner Wall Seperation");
        dep = GameObject.Find("Vague Range Symbol 2 (1)");
    }
	
	// Update is called once per frame
	void Update () {
        posWall = wall.transform.position;
        posDep = dep.transform.position;

        posDep.z = posWall.z;

        dep.transform.position = posDep;

    }
}
