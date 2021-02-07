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
    GameObject infoView;
    [SerializeField]
    StatesViewUI statesView;
    [SerializeField]
    CommentsViewUI commentsView;
    [SerializeField]
    TransformControlUI movementView;
    [SerializeField]
    Button closeButton;

    [Header("Info View References:")]
    [SerializeField]
    Text elementName;
    [SerializeField]
    Text stateNumber;
    [SerializeField]
    CertaintyUI semanticCertainty;
    [SerializeField]
    CertaintyUI geometricCertainty;
    [SerializeField]
    RatingUI ratingView;

    [Header("Action Buttons")]
    [SerializeField]
    Button movementButton;
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

        if (intObj.States.Count <= 0)
        {
            statesView.gameObject.SetActive(false);
            commentsView.gameObject.SetActive(false);
            cloneToNewState.gameObject.SetActive(false);
            deleteStateButton.gameObject.SetActive(false);
            movementView.gameObject.SetActive(false);
            infoView.SetActive(true);
            elementName.text = intObj.name;
            stateNumber.text = "No States";
            return;
        }

        statesView.gameObject.SetActive(true);
        commentsView.gameObject.SetActive(true);
        cloneToNewState.gameObject.SetActive(true);
        deleteStateButton.gameObject.SetActive(true);
        infoView.SetActive(true);
        movementView.gameObject.SetActive(false);

        statesView.Visualize(intObj.States, intObj.CurrentStateID, intObj.SetState);
        commentsView.Visualize(intObj.CurrentState.comments, intObj.AddComment, intObj.RemoveComment);
        semanticCertainty.Visualize(intObj.CurrentState.semanticCertainty, intObj.SetSemanticCertainty);
        geometricCertainty.Visualize(intObj.CurrentState.geometricCertainty, intObj.SetGeometricCertainty);
        ratingView.Visualize(intObj.CurrentState.rating, intObj.SetRating);
        movementView.Visualize(intObj, intObj.DisableMovementAndSave, intObj.DisableMovementAndCancel);

        elementName.text = intObj.name;
        stateNumber.text = "State: " + (intObj.CurrentStateID + 1).ToString();

        movementButton.onClick.RemoveAllListeners();
        movementButton.onClick.AddListener(intObj.EnableMovement);
        movementButton.onClick.AddListener(StartMovementMode);

        createNewState.onClick.RemoveAllListeners();
        createNewState.onClick.AddListener(intObj.CreateNewState);

        cloneToNewState.onClick.RemoveAllListeners();
        cloneToNewState.onClick.AddListener(intObj.CloneToNewState);

        deleteStateButton.onClick.RemoveAllListeners();
        deleteStateButton.onClick.AddListener(intObj.RemoveState);
    }

    void StartMovementMode()
    {
        statesView.gameObject.SetActive(false);
        commentsView.gameObject.SetActive(false);
        cloneToNewState.gameObject.SetActive(false);
        deleteStateButton.gameObject.SetActive(false);
        infoView.SetActive(false);
        movementView.gameObject.SetActive(true);
    }
}
