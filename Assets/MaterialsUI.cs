using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialsUI : MonoBehaviour
{
    [SerializeField]
    private MaterialsController matController = null;
    [SerializeField]
    private Dropdown materialsDropDown = null;
    [SerializeField]
    private bool interactable = false;

    private void Awake()
    {
        materialsDropDown.Select();
    }

    public void Visualize(string matName, System.Action<Material> onPickAction = null)
    {
        materialsDropDown.onValueChanged.RemoveAllListeners();
        materialsDropDown.onValueChanged.AddListener(getMaterialNameThenAssign);
        materialsDropDown.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<int>(i => onPickAction(matController.currentlyAssignedMat)));

        matController.AssignMaterialIfFound(matName);
        int dropDownIndex = Mathf.Max(0, matController.currentlyAssignedMatIndex);
        materialsDropDown.Select();
        materialsDropDown.SetValueWithoutNotify(dropDownIndex);
        materialsDropDown.RefreshShownValue();
    }

    public void getMaterialNameThenAssign(int matIndex)
    {
        matController.AssignMaterialIfFound(materialsDropDown.options[matIndex].text);
    }
}
