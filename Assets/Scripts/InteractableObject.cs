﻿using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public static List<InteractableObject> interactables = new List<InteractableObject>();
    public static InteractableObject currentInteractable = null;

    public bool autoSetTag = true;
    public string tagName = "Interactable";

    List<ObjectState> states;
    int currentStateID = 0;
    
    Color originalColor;
    List<Color> originalChildrenColors;

    public ObjectState currentState
    {
        get { return states[currentStateID]; }
    }

    void Awake()
    {
        if (states == null)
        {
            states = new List<ObjectState>();
            ObjectState objState = ObjectState.GenerateObjectState(transform.position, transform.rotation);
            states.Add(objState);
            currentStateID = 0;
        }

        if (autoSetTag)
            transform.tag = tagName;

        originalColor = GetComponent<MeshRenderer>().material.color;
        originalChildrenColors = new List<Color>();
        foreach (Transform child in transform) 
        {
            originalChildrenColors.Add(child.GetComponent<MeshRenderer>().material.color);
        }
    }

    void OnEnable()
    {
        interactables.Add(this);
    }

    void OnDisable()
    {
        interactables.Remove(this);
    }

    public void UpdateState()
    {
        transform.position = currentState.position;
        transform.rotation = currentState.rotation;
    }

    public void SelectObject()
    {
        Color color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, GetComponent<MeshRenderer>().material.color.a);
        GetComponent<MeshRenderer>().material.color = color;
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    public void UnselectObject()
    {
        GetComponent<MeshRenderer>().material.color = originalColor;
        int i = 0;
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().material.color = originalChildrenColors[i];
            i++;
        }
    }

    void OnMouseDown()
    {
        foreach (InteractableObject obj in interactables)
        {
            obj.UnselectObject();
        }

        SelectedObjectUI.HideSelectedObjectUI();

        SelectObject();
        SelectedObjectUI.DisplaySelectedObjectUI(currentState);
    }
}

[System.Serializable]
public struct ObjectState
{
    public Vector3 position;
    public Quaternion rotation;
    public List<Comment> comments;
    public float rating; // 0.0 - 5.0
    public bool isRated;
    public Certainty degreeOfCertainty;

    public static ObjectState GenerateObjectState(Vector3 pos, Quaternion rot, List<Comment> comms = null, float rate = 0, bool rated = false, Certainty certainty = Certainty.None)
    {
        ObjectState objState = new ObjectState();
        objState.position = pos;
        objState.rotation = rot;
        objState.comments = comms;
        objState.rating = rate;
        objState.isRated = rated;
        objState.degreeOfCertainty = certainty;
        return objState;
    }

    public static ObjectState CloneObjectState(ObjectState objectState)
    {
        ObjectState objState = new ObjectState();
        objState.position = objectState.position;
        objState.rotation = objectState.rotation;
        objState.comments = objectState.comments;
        objState.rating = objectState.rating;
        objState.isRated = objectState.isRated;
        objState.degreeOfCertainty = objectState.degreeOfCertainty;
        return objState;
    }
}

[System.Serializable]
public enum Certainty
{
    None,
    Low,
    Medium,
    High,
}

[System.Serializable]
public struct Comment
{
    public string commenter;
    public string comment;
}
