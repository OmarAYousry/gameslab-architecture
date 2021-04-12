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

    public void Visualize(string matName, System.Action<Material> onPickAction = null)
    {
        materialsDropDown.onValueChanged.RemoveAllListeners();
        materialsDropDown.onValueChanged.AddListener(getMaterialNameThenAssign);
        materialsDropDown.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<int>(i => onPickAction(matController.currentlyAssignedMat)));

        matController.AssignMaterialIfFound(matName);
        Debug.LogWarning(matController.currentlyAssignedMat);
        Debug.LogError(materialsDropDown.value);
        Debug.LogError(matController.currentlyAssignedMatIndex);
        materialsDropDown.value = matController.currentlyAssignedMatIndex;
        materialsDropDown.RefreshShownValue();
        Debug.LogError(materialsDropDown.value);
    }

    public void getMaterialNameThenAssign(int matIndex)
    {
        matController.AssignMaterialIfFound(materialsDropDown.options[matIndex].text);
    }
}
