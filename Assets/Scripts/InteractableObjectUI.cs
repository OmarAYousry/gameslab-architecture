using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjectUI : MonoBehaviour
{
    public static InteractableObjectUI instance = null;

    InteractableObject intObj;

    [Header("References:")]
    [SerializeField]
    StatesViewUI statesView;
    [SerializeField]
    CommentsViewUI commentsView;
    [SerializeField]
    Button closeButton;

    [Header("Info View References:")]
    [SerializeField]
    Text elementName;
    [SerializeField]
    Text stateNumber;
    [SerializeField]
    CertaintyUI certaintyView;
    [SerializeField]
    RatingUI ratingView;

    [Header("Action Buttons")]
    [SerializeField]
    Button createNewState;
    [SerializeField]
    Button cloneToNewState;
    [SerializeField]
    Button resetStateButton;
    [SerializeField]
    Button deleteStateButton;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        closeButton.onClick.AddListener(InteractableObject.Unselect);
    }

    void OnDisable()
    {
        closeButton.onClick.RemoveListener(InteractableObject.Unselect);
    }

    public void Visualize(InteractableObject iObj)
    {
        if (iObj == null)
        {
            intObj = null;
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        intObj = iObj;

        statesView.Visualize(intObj.States, intObj.CurrentStateID, intObj.SetState);
        commentsView.Visualize(intObj.CurrentState.comments, intObj.AddComment, intObj.RemoveComment);
        certaintyView.Visualize(intObj.CurrentState.degreeOfCertainty, intObj.SetCertainty);
        ratingView.Visualize(intObj.CurrentState.rating, intObj.SetRating);

        elementName.text = intObj.name;
        stateNumber.text = "State: " + (intObj.CurrentStateID + 1).ToString();

        createNewState.onClick.RemoveAllListeners();
        createNewState.onClick.AddListener(intObj.CreateNewState);

        cloneToNewState.onClick.RemoveAllListeners();
        cloneToNewState.onClick.AddListener(intObj.CloneToNewState);

        deleteStateButton.onClick.RemoveAllListeners();
        deleteStateButton.onClick.AddListener(intObj.RemoveState);
    }
}
