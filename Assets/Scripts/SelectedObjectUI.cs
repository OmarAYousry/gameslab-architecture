using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SelectedObjectUI : MonoBehaviour
{
    private static SelectedObjectUI instance = null;

    [SerializeField]
    private Canvas parentCanvas = null;

    public static ObjectState CurrentObjectState { get; private set; }

    private static List<string> commentsList = null;

    private static int commentsIndex = 0;

    [SerializeField]
    private TMP_InputField commentsField = null;
    
    [SerializeField]
    private Button prevCommentButton = null;

    [SerializeField]
    private Button nextCommentButton = null;

    [SerializeField]
    private Button createCommentButton = null;

    [SerializeField]
    private Button deleteCommentButton = null;

    [SerializeField]
    private TMP_Text ratingText = null;
    
    [SerializeField]
    private Button changeRatingButton = null;

    [SerializeField]
    private TMP_Text positionText = null;

    [SerializeField]
    private TMP_Text rotationText = null;

    void Awake()
    {
        instance = this;
        instance.parentCanvas.enabled = false;
        instance.parentCanvas.gameObject.SetActive(false);
    }

    public static void DisplaySelectedObjectUI(ObjectState selectedObjectState)
    {
        CurrentObjectState = selectedObjectState;

        if (CurrentObjectState.comments != null)
            commentsList = new List<string>(CurrentObjectState.comments.Values);
        else
            commentsList = new List<string>();

        commentsIndex = 0;

        instance.commentsField.text = (commentsList.Count > 0) ? commentsList[commentsIndex] : string.Empty;

        instance.prevCommentButton.onClick.AddListener(goToPreviousComment);
        instance.nextCommentButton.onClick.AddListener(goToNextComment);
        instance.createCommentButton.onClick.AddListener(createComment);
        instance.deleteCommentButton.onClick.AddListener(deleteComment);

        instance.ratingText.text = (CurrentObjectState.isRated) ? CurrentObjectState.rating.ToString() : "Unrated";
        instance.changeRatingButton.onClick.AddListener(changeRating);

        instance.positionText.text = CurrentObjectState.position.ToString();
        instance.rotationText.text = CurrentObjectState.rotation.ToString();

        instance.parentCanvas.enabled = true;
        instance.parentCanvas.gameObject.SetActive(true);
    }

    public static void HideSelectedObjectUI()
    {
        // TODO:
        // Save state object to disk before resetting fields
        // 
        // code goes here
        // 
        // ---------

        commentsList = null;
        commentsIndex = 0;

        instance.commentsField.text = string.Empty;

        instance.prevCommentButton.onClick.RemoveAllListeners();
        instance.nextCommentButton.onClick.RemoveAllListeners();
        instance.createCommentButton.onClick.RemoveAllListeners();
        instance.deleteCommentButton.onClick.RemoveAllListeners();

        instance.ratingText.text = string.Empty;
        instance.changeRatingButton.onClick.RemoveAllListeners();

        instance.positionText.text = string.Empty;
        instance.rotationText.text = string.Empty;

        instance.parentCanvas.enabled = false;
        instance.parentCanvas.gameObject.SetActive(false);
    }

    private static void goToPreviousComment()
    {
        if (commentsList.Count == 0)
            return;

        commentsIndex = Math.Max(commentsIndex - 1, 0);

        instance.commentsField.text = commentsList[commentsIndex];

        if (commentsIndex == 0)
            instance.prevCommentButton.interactable = false;
    }

    private static void goToNextComment()
    {
        if (commentsList.Count == 0)
            return;

        commentsIndex = Math.Min(commentsIndex + 1, commentsList.Count - 1);

        instance.commentsField.text = commentsList[commentsIndex];

        if (commentsIndex == commentsList.Count - 1)
            instance.nextCommentButton.interactable = false;
    }

    private static void createComment()
    {
        commentsList.Add(string.Empty);

        commentsIndex = commentsList.Count - 1;

        instance.commentsField.text = commentsList[commentsIndex];

        if (commentsIndex == commentsList.Count - 1)
            instance.nextCommentButton.interactable = false;
        if (commentsIndex == 0)
            instance.prevCommentButton.interactable = false;
    }

    private static void deleteComment()
    {
        commentsList.RemoveAt(commentsIndex);

        commentsIndex = Math.Max(commentsIndex - 1, 0);

        if (commentsIndex == commentsList.Count - 1)
            instance.nextCommentButton.interactable = false;
        if (commentsIndex == 0)
            instance.prevCommentButton.interactable = false;
    }

    private static void changeRating()
    {
        commentsList.RemoveAt(commentsIndex);

        commentsIndex = Math.Max(commentsIndex - 1, 0);

        if (commentsIndex == commentsList.Count - 1)
            instance.nextCommentButton.interactable = false;
        if (commentsIndex == 0)
            instance.prevCommentButton.interactable = false;
    }
}

