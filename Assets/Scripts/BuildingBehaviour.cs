using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour
{
    public static string BuildingName { get; private set; } = null;

    void Start()
    {
        BuildingName = gameObject.name;
        DataPersistence.InitObjectStatesFromFile();
    }
}
