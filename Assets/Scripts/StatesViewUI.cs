using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesViewUI : MonoBehaviour
{
    List<ObjectState> states;

    [Header("References:")]
    [SerializeField]
    StateUI stateButtonPrefab;
    [SerializeField]
    GameObject contentParent;

    public void Visualize(List<ObjectState> objStates, int currentStateIndex = 0, System.Action<int> onStatePick = null)
    {
        states = objStates;
        foreach (Transform child in contentParent.transform)
            Destroy(child.gameObject);

        for(int i = 0; i < objStates.Count; i++)
        {
            StateUI newState = Instantiate(stateButtonPrefab, contentParent.transform);
            int stateIndex = i;
            System.Action statePickAction = () =>
            {
                onStatePick(stateIndex);
            };

            bool isSelected = (i == currentStateIndex);
            newState.Visualize(states[i], i, statePickAction, isSelected);
        }
    }
}
