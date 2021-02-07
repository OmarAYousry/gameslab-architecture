using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformControlUI : MonoBehaviour
{
    [Header("References:")]
    [SerializeField]
    Button acceptButton;
    [SerializeField]
    Button cancelButton;
    [SerializeField]
    Slider xSlider;
    [SerializeField]
    Slider ySlider;
    [SerializeField]
    Slider zSlider;
    [SerializeField]
    Text xScaleText;
    [SerializeField]
    Text yScaleText;
    [SerializeField]
    Text zScaleText;

    InteractableObject intObj;

    public void Visualize(InteractableObject iObj, System.Action acceptAction, System.Action cancelAction)
    {
        intObj = iObj;

        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(new UnityEngine.Events.UnityAction(acceptAction));

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(new UnityEngine.Events.UnityAction(cancelAction));

        xSlider.value = intObj.transform.localScale.x;
        ySlider.value = intObj.transform.localScale.y;
        zSlider.value = intObj.transform.localScale.z;

        xSlider.onValueChanged.RemoveAllListeners();
        xSlider.onValueChanged.AddListener(SetXScale);

        ySlider.onValueChanged.RemoveAllListeners();
        ySlider.onValueChanged.AddListener(SetYScale);

        zSlider.onValueChanged.RemoveAllListeners();
        zSlider.onValueChanged.AddListener(SetZScale);
    }

    void UpdateScaleTextValues()
    {
        xScaleText.text = xSlider.value.ToString();
        yScaleText.text = ySlider.value.ToString();
        zScaleText.text = zSlider.value.ToString();
    }

    public void SetXScale(float x)
    {
        if (intObj == null)
        {
            Debug.LogWarning("[TransformControlUI] Can not set scale. InteractableObject was not set.");
            return;
        }
        intObj.transform.localScale = new Vector3(x, intObj.transform.localScale.y, intObj.transform.localScale.z);
        UpdateScaleTextValues();
    }

    public void SetYScale(float y)
    {
        if (intObj == null)
        {
            Debug.LogWarning("[TransformControlUI] Can not set scale. InteractableObject was not set.");
            return;
        }
        intObj.transform.localScale = new Vector3(intObj.transform.localScale.x, y, intObj.transform.localScale.z);
        UpdateScaleTextValues();
    }

    public void SetZScale(float z)
    {
        if (intObj == null)
        {
            Debug.LogWarning("[TransformControlUI] Can not set scale. InteractableObject was not set.");
            return;
        }
        intObj.transform.localScale = new Vector3(intObj.transform.localScale.x, intObj.transform.localScale.y, z);
        UpdateScaleTextValues();
    }
}
