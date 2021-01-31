using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentsViewUI : MonoBehaviour
{
    List<Comment> comments;

    [Header("Animation Settings: ")]
    [SerializeField]
    float offHeight = 200;
    [SerializeField]
    float onHeight = 750;
    [SerializeField]
    float animationSpeed = 0.25f;

    [Header("References:")]
    [SerializeField]
    CommentUI commentPrefab;
    [SerializeField]
    GameObject contentParent;
    [SerializeField]
    InputField newCommentInputField;
    [SerializeField]
    InputField commenterNameInputField;
    [SerializeField]
    Button submitCommentButton;
    [SerializeField]
    Text commentsCountText;

    bool commentsShown = false;
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Visualize(List<Comment> stateComments, System.Action<Comment> addCommentAction, System.Action<int> deleteCommentAction)
    {
        comments = stateComments;

        foreach (Transform child in contentParent.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < comments.Count; i++)
        {
            CommentUI commUI = Instantiate(commentPrefab, contentParent.transform);
            int commIndex = i;
            System.Action deleteAction = () =>
            {
                deleteCommentAction(commIndex);
            };
            commUI.Visualize(stateComments[i], deleteAction);
        }

        System.Action submitButtonAction = () =>
        {
            Comment comm = new Comment();
            comm.commenter = commenterNameInputField.text;
            comm.comment = newCommentInputField.text;
            if (!string.IsNullOrEmpty(comm.comment))
            {
                addCommentAction(comm);
                newCommentInputField.text = string.Empty;
            }
            else
                Debug.LogWarning("[CommentsViewUI] Can not add empty comment.");
        };

        submitCommentButton.onClick.RemoveAllListeners();
        submitCommentButton.onClick.AddListener(new UnityEngine.Events.UnityAction(submitButtonAction));
        commentsCountText.text = "(" + stateComments.Count + ")";

        if (commentsShown)
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, onHeight);
        else
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, offHeight);
    }

    public void ToggleShowComments()
    {
        if (commentsShown)
        {
            StartCoroutine(LerpHeight(rect.sizeDelta.y, offHeight, animationSpeed));
            commentsShown = false;
        } else
        {
            StartCoroutine(LerpHeight(rect.sizeDelta.y, onHeight, animationSpeed));
            commentsShown = true;
        }
    }

    IEnumerator LerpHeight(float start, float end, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            float y = Mathf.Lerp(start, end, timer / duration);
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, y);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, end);
    }
}
