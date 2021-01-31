using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingUI : MonoBehaviour
{
    float rating = 0.0f;
    System.Action<float> onRatingPick;

    [SerializeField]
    bool interactable = false;

    [Header("References:")]
    [SerializeField]
    List<Button> stars;

    public void Visualize(float rate, System.Action<float> onPickAction = null)
    {
        rating = rate;
        int count = (int)rating;

        for (int i = 0; i < stars.Count; i++)
        {
            if (i < rating)
                stars[i].image.color = Color.white;
            else
                stars[i].image.color = Color.grey;

            if (!interactable)
                stars[i].enabled = false;
            else
            {
                if (onPickAction != null)
                {
                    int buttonIndex = i;
                    System.Action buttonAction = () => { onPickAction((float)buttonIndex + 1); };
                    stars[i].onClick.RemoveAllListeners();
                    stars[i].onClick.AddListener(new UnityEngine.Events.UnityAction(buttonAction));
                }
            }
        }
    }
}
