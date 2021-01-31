using UnityEngine;
using UnityEngine.UI;

public class StateUI : MonoBehaviour
{
    ObjectState state;
    int index;

    [Header("References")]
    [SerializeField]
    Button stateButton;
    [SerializeField]
    Text stateLabel;
    [SerializeField]
    RatingUI ratingUI;

    public void Visualize(ObjectState objState, int stateIndex, System.Action statePickAction = null, bool selected = false)
    {
        state = objState;
        index = stateIndex;

        stateLabel.text = (index + 1).ToString();
        stateButton.onClick.RemoveAllListeners();

        if (selected)
        {
            stateButton.image.color = Color.green;
            stateButton.enabled = false;
        }
        else
        {
            stateButton.image.color = Color.white;
            if (statePickAction != null)
                stateButton.onClick.AddListener(new UnityEngine.Events.UnityAction(statePickAction));
        }

        ratingUI.Visualize(state.rating);
    }
}
