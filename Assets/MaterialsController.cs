﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialsController : MonoBehaviour
{
    [SerializeField]
    private Dropdown materialsDropDown = null;

    [SerializeField]
    private List<Material> selectableMaterials = new List<Material>();

    public List<Material> SelectableMaterials { get { return selectableMaterials; } }

    public Material currentlyAssignedMat { get; private set; }
    public int currentlyAssignedMatIndex { get; private set; }

    void Awake()
    {
        materialsDropDown.ClearOptions();
        currentlyAssignedMat = null;
        currentlyAssignedMatIndex = -1;
    }

    void Start()
    {
        List<Dropdown.OptionData> materialNames = new List<Dropdown.OptionData>();
        foreach (Material mat in selectableMaterials)
        {
            // TODO: Maybe add sprite here, else leave as is
            materialNames.Add(new Dropdown.OptionData(text: mat.name, image: null));
        }

        materialsDropDown.AddOptions(materialNames);
    }

    public void AssignMaterialIfFound(string materialName)
    {
        Material selectedMat = selectableMaterials.Find(mat => mat.name == materialName);
        if (selectedMat == null)
            return;
        if (selectedMat == currentlyAssignedMat)
            return;

        currentlyAssignedMat = selectedMat;
        currentlyAssignedMatIndex = selectableMaterials.IndexOf(selectedMat);
    }
}
