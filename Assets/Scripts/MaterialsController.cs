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

    public static MaterialsController instance { get; private set; } = null;

    public Material currentlyAssignedMat { get; private set; }
    public int currentlyAssignedMatIndex { get; private set; }

    public static void AddMaterialIfNotExisting(Material matParam)
    {
        if (instance.selectableMaterials.Find(mat => mat.name == matParam.name.Replace(" (Instance)", "")) == null)
        {
            Material newSelectableMat = new Material(matParam);
            newSelectableMat.name = newSelectableMat.name.Replace(" (Instance)", "");
            instance.selectableMaterials.Add(newSelectableMat);
            instance.materialsDropDown.AddOptions(new List<Dropdown.OptionData> { new Dropdown.OptionData(newSelectableMat.name) });
        }
    }

    void Awake()
    {
        instance = this;
        materialsDropDown.ClearOptions();
        currentlyAssignedMat = null;
        currentlyAssignedMatIndex = -1;

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
        {
            currentlyAssignedMat = null;
            currentlyAssignedMatIndex = -1;
            return;
        }
        if (selectedMat == currentlyAssignedMat)
            return;

        currentlyAssignedMat = selectedMat;
        currentlyAssignedMatIndex = selectableMaterials.IndexOf(selectedMat);
    }
}
