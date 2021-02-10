using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CertaintyUI : MonoBehaviour
{
    [System.Obsolete]
    Certainty certaintyEnum;
    [System.Obsolete]
    System.Action<Certainty> certaintyEnumAction;

    float certaintyValue;
    System.Action<float> certaintyAction;

    [SerializeField]
    string certaintyTitle = "Certainy";

    [Header("References:")]
    [SerializeField]
    Text certaintyText;
    [SerializeField]
    Slider certaintySlider;

    public void Visualize(float cert, System.Action<float> certaintyChangeAction)
    {
        certaintyValue = cert;
        certaintyAction = certaintyChangeAction;

        certaintyText.text = certaintyTitle + ": " + ((int)(certaintyValue * 100)).ToString() + "%";

        certaintySlider.value = certaintyValue;

        certaintySlider.onValueChanged.RemoveAllListeners();
        certaintySlider.onValueChanged.AddListener(SetPercentageText);
    }

    [System.Obsolete]
    public void Visualize(Certainty cert, System.Action<Certainty> certaintyChangeAction)
    {
        certaintyEnum = cert;
        certaintyEnumAction = certaintyChangeAction;

        certaintyText.text = certaintyTitle + ": " + certaintyEnum.ToString();
        
        switch (certaintyEnum)
        {
            case Certainty.None:
                certaintySlider.value = 0;
                break;
            case Certainty.Low:
                certaintySlider.value = 0.33f;
                break;
            case Certainty.Medium:
                certaintySlider.value = 0.66f;
                break;
            case Certainty.High:
                certaintySlider.value = 0.99f;
                break;
            default:
                break;
        }

        System.Action<float> sliderAction = (float f) =>
        {
            if (f == 0)
                return;
            if (f <= 0.05f)
                certaintyChangeAction(Certainty.None);
            else
            if (f > 0.05f && f <= 0.33f)
                certaintyChangeAction(Certainty.Low);
            else
            if (f > 0.33f && f <= 0.66f)
                certaintyChangeAction(Certainty.Medium);
            else
            if (f > 0.66f)
                certaintyChangeAction(Certainty.High);
        };

        //certaintySlider.onValueChanged.RemoveAllListeners();
        //certaintySlider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(sliderAction));
    }

    public void SetPercentageText(float value)
    {
        certaintyText.text = certaintyTitle + ": " + ((int)(value * 100)).ToString() + "%";
    }

    public void OnSliderRelease()
    {
        float f = certaintySlider.value;
        certaintyAction(f);
    }

    [System.Obsolete]
    public void OnSliderEnumRelease()
    {
        float f = certaintySlider.value;
        if (f == 0)
            return;
        if (f <= 0.05f)
            certaintyEnumAction(Certainty.None);
        else
        if (f > 0.05f && f <= 0.33f)
            certaintyEnumAction(Certainty.Low);
        else
        if (f > 0.33f && f <= 0.66f)
            certaintyEnumAction(Certainty.Medium);
        else
        if (f > 0.66f)
            certaintyEnumAction(Certainty.High);
    }
}
