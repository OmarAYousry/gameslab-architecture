using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementUI : MonoBehaviour
{
    [Header("References:")]
    [SerializeField]
    Button acceptButton;
    [SerializeField]
    Button cancelButton;

    public void Visualize(System.Action acceptAction, System.Action cancelAction)
    {
        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(new UnityEngine.Events.UnityAction(acceptAction));

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(new UnityEngine.Events.UnityAction(cancelAction));
    }
}
