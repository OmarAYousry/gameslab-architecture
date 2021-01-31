using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlane : MonoBehaviour
{
    void Awake()
    {
        Global.RegisterMovementPlane(this);
        gameObject.SetActive(false);
    }
}
