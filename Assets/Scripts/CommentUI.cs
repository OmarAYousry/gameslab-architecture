using UnityEngine;
using UnityEngine.UI;

public class CommentUI : MonoBehaviour
{
    Comment comment;

    [Header("References:")]
    [SerializeField]
    Text commenterName;
    [SerializeField]
    Text commentText;
    [SerializeField]
    Button deleteButton;

    public void Visualize(Comment comm, System.Action deleteButtonAction = null)
    {
        comment = comm;

        commenterName.text = (string.IsNullOrEmpty(comm.commenter)) ? "Anonymus" : comm.commenter;
        commentText.text = comm.comment;

        if (deleteButtonAction != null)
            deleteButton.onClick.AddListener(new UnityEngine.Events.UnityAction(deleteButtonAction));
    }
}
