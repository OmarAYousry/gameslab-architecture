using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CertaintyUI : MonoBehaviour
{
    Certainty certainty;
    System.Action<Certainty> certaintyAction;

    [SerializeField]
    string certaintyTitle = "Certainy";

    [Header("References:")]
    [SerializeField]
    Text certaintyText;
    [SerializeField]
    Slider certaintySlider;

    public void Visualize(Certainty cert, System.Action<Certainty> certaintyChangeAction)
    {
        certainty = cert;
        certaintyAction = certaintyChangeAction;

        certaintyText.text = certaintyTitle + ": " + certainty.ToString();
        
        switch (certainty)
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

    public void OnSliderRelease()
    {
        float f = certaintySlider.value;
        if (f == 0)
            return;
        if (f <= 0.05f)
            certaintyAction(Certainty.None);
        else
        if (f > 0.05f && f <= 0.33f)
            certaintyAction(Certainty.Low);
        else
        if (f > 0.33f && f <= 0.66f)
            certaintyAction(Certainty.Medium);
        else
        if (f > 0.66f)
            certaintyAction(Certainty.High);
    }
}
