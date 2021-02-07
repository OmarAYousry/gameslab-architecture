using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TransformControl))]
public class InteractableObject : MonoBehaviour
{
    public static List<InteractableObject> interactables = new List<InteractableObject>();
    public static InteractableObject currentInteractable = null;

    public bool autoSetTag = true;
    public string tagName = "Interactable";

    public List<ObjectState> states { get; set; }
    int currentStateID = 0;

    List<Material> defaultMats = null;

    Color originalColor;
    List<Color> originalChildrenColors;
    TransformControl movement;
    public List<ObjectState> States
    {
        get { return states; }
    }

    public int CurrentStateID
    {
        get { return currentStateID; }
    }

    public ObjectState CurrentState
    {
        get { return states[currentStateID]; }
    }

    void Awake()
    {
        defaultMats = new List<Material>();
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            defaultMats.Add(renderer.material);
        }

        if (states == null)
        {
            states = new List<ObjectState>();
            ObjectState objState = ObjectState.GenerateObjectState(transform.localPosition, transform.localRotation, transform.localScale);
            states.Add(objState);
            currentStateID = 0;
        }

        ResetState();

        if (autoSetTag)
            transform.tag = tagName;

        movement = GetComponent<TransformControl>();
    }

    void Start()
    {
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

    public void ResetState()
    {
        UpdateTransformFromState();
    }

    public void UpdateTransformFromState()
    {
        transform.localPosition = CurrentState.position;
        transform.localRotation = CurrentState.rotation;
        transform.localScale = CurrentState.scale;
    }

    public void UpdateUI()
    {
        InteractableObjectUI.instance.Visualize(currentInteractable);
    }

    #region interactions
    public void SelectObject()
    {
        applySelectColors();
        currentInteractable = this;
        UpdateUI();
    }

    public void UnselectObject()
    {
        applyOriginalColors();
    }

    private void applyOriginalColors()
    {
        Color originalColorToApply = originalColor;
        originalColorToApply.a = GetComponent<MeshRenderer>().material.color.a;
        GetComponent<MeshRenderer>().material.color = originalColorToApply;
        int i = 0;
        foreach (Transform child in transform)
        {
            Color colorToApply = originalChildrenColors[i];
            colorToApply.a = child.GetComponent<MeshRenderer>().material.color.a;
            child.GetComponent<MeshRenderer>().material.color = colorToApply;
            i++;
        }
    }

    private void applySelectColors()
    {
        Color color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, GetComponent<MeshRenderer>().material.color.a);
        GetComponent<MeshRenderer>().material.color = color;
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    public static void Unselect()
    {
        if (currentInteractable != null)
        {
            currentInteractable.UnselectObject();
            currentInteractable = null;
            InteractableObjectUI.instance.Visualize(null);
            DataPersistence.SerializeObjectStates();
        }
    }

    void OnMouseUp()
    {
        if (currentInteractable != null)
            return;

        foreach (InteractableObject obj in interactables)
        {
            obj.UnselectObject();
        }
        SelectObject();
    }

    public void SetTempScale(Vector3 newScale)
    {
        transform.position = newScale;
    }
    #endregion

    #region back-end actions
    public void SetPosition(Vector3 newPos)
    {
        CurrentState.position = newPos;
    }

    public void SetRotation(Quaternion newRot)
    {
        CurrentState.rotation = newRot;
    }

    public void SetScale(Vector3 newScale)
    {
        CurrentState.scale = newScale;
    }

    public void AddComment(string commenter, string comment)
    {
        Comment newComment = new Comment();
        newComment.commenter = commenter;
        newComment.comment = comment;

        states[currentStateID].comments.Add(newComment);
        UpdateUI();
    }

    public void AddComment(Comment newComment)
    {
        states[currentStateID].comments.Add(newComment);
        UpdateUI();
    }

    public void RemoveComment(int commentIndex)
    {
        states[currentStateID].comments.RemoveAt(commentIndex);
        UpdateUI();
    }

    public void SetRating(float rating)
    {
        states[currentStateID].SetRating(rating);
        UpdateUI();
    }

    public void SetSemanticCertainty(Certainty certainty)
    {
        Debug.Log("Changed semantic certainty to " + certainty);
        states[currentStateID].semanticCertainty = certainty;
        UpdateUI();
        ToggleCertaintyMaterial(CertaintyMaterialization.isPreviewCertainty, CertaintyMaterialization.CertaintyMats);
    }

    public void SetGeometricCertainty(Certainty certainty)
    {
        Debug.Log("Changed geometric certainty to " + certainty);
        states[currentStateID].geometricCertainty = certainty;
        UpdateUI();
        //ToggleCertaintyMaterial(CertaintyMaterialization.isPreviewCertainty, CertaintyMaterialization.CertaintyMats);
    }

    public void SetState(int stateIndex)
    {
        currentStateID = stateIndex;
        ResetState();
        UpdateUI();
    }

    public void CreateNewState()
    {
        ObjectState newObjectState = ObjectState.GenerateObjectState(transform.localPosition, transform.localRotation, transform.localScale);
        states.Add(newObjectState);
        currentStateID = states.IndexOf(newObjectState);
        ResetState();
        UpdateUI();
    }

    public void CloneToNewState()
    {
        ObjectState newObjectState = ObjectState.CloneObjectState(CurrentState);
        states.Add(newObjectState);
        currentStateID = states.IndexOf(newObjectState);
        ResetState();
        UpdateUI();
    }

    public void RemoveState()
    {
        if (states.Count > 1)
        {
            states.RemoveAt(currentStateID);
            currentStateID = 0;
            ResetState();
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("[InteractableObject] Can not delete the only state that exists.");
        }
    }

    public void EnableMovement()
    {
        movement.enabled = true;
    }

    public void DisableMovementAndCancel()
    {
        movement.enabled = false;
        ResetState();
        UpdateUI();
    }

    public void DisableMovementAndSave()
    {
        movement.enabled = false;
        SetPosition(transform.localPosition);
        SetRotation(transform.localRotation);
        SetScale(transform.localScale);
        UpdateTransformFromState();
        UpdateUI();
    }
    #endregion

    public void Test()
    {

        Comment comm1; comm1.comment = "I really like this one."; comm1.commenter = "mister";
        Comment comm2; comm2.comment = "I hate this..."; comm2.commenter = "jeff";
        List<Comment> comms = new List<Comment> { comm1, comm2 };

        ObjectState testObject = ObjectState.GenerateObjectState(transform.localPosition, transform.localRotation, transform.localScale, comms, 3, true, Certainty.None, Certainty.None);
        states[currentStateID] = testObject;

        Comment comm3; comm3.comment = "well well well"; comm3.commenter = "wellman";
        Comment comm4; comm4.comment = "this is wrong"; comm4.commenter = "peter";
        List<Comment> comms2 = new List<Comment> { comm3, comm4 };

        ObjectState testObject2 = ObjectState.GenerateObjectState(transform.localPosition, transform.localRotation, transform.localScale, comms2, 2, true, Certainty.Medium, Certainty.Medium);
        states.Add(testObject2);

        Debug.Log("Testing...");
        ResetState();
        UpdateUI();
    }

    public void ToggleCertaintyMaterial(bool applyingCertainty, Material[] certaintyMats = null)
    {
        if (applyingCertainty)
        {
            switch (CurrentState.semanticCertainty)
            {
                case Certainty.None:
                    applyDefaultMats();
                    break;
                case Certainty.Low:
                case Certainty.Medium:
                case Certainty.High:
                    applyCertaintyMaterial(certaintyMats[((int) CurrentState.semanticCertainty) - 1]);
                    break;
                default:
                    break;
            }
        }
        else
            applyDefaultMats();

        if (currentInteractable == this)
            applySelectColors();
        else
            applyOriginalColors();
    }

    private void applyDefaultMats()
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = defaultMats[i];
        }
    }

    private void applyCertaintyMaterial(Material certaintyMaterial)
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material = certaintyMaterial;
        }
    }
}

[System.Serializable]
public class ObjectState
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public List<Comment> comments;
    public float rating; // 0.0 - 5.0
    public bool isRated;
    public Certainty semanticCertainty;
    public Certainty geometricCertainty;

    public static ObjectState GenerateObjectState(Vector3 pos, Quaternion rot, Vector3 scale, List<Comment> comms = null, float rate = 0, bool rated = false, Certainty semantic = Certainty.None, Certainty geometric = Certainty.None)
    {
        ObjectState objState = new ObjectState();
        objState.position = pos;
        objState.rotation = rot;
        objState.scale = scale;
        objState.rating = rate;
        objState.isRated = rated;
        objState.semanticCertainty = semantic;
        objState.semanticCertainty = geometric;
        objState.comments = (comms == null)? new List<Comment>() : comms;
        return objState;
    }

    public static ObjectState CloneObjectState(ObjectState objectState)
    {
        ObjectState objState = new ObjectState();
        objState.position = objectState.position;
        objState.rotation = objectState.rotation;
        objState.scale = objectState.scale;
        objState.comments = objectState.comments;
        objState.rating = objectState.rating;
        objState.isRated = objectState.isRated;
        objState.semanticCertainty = objectState.semanticCertainty;
        objState.geometricCertainty = objectState.geometricCertainty;
        return objState;
    }

    public void SetPosition(Vector3 newPos)
    {
        position = newPos;
    }

    public void SetRotation(Quaternion newRot)
    {
        rotation = newRot;
    }

    public void SetRating(float newRating)
    {
        rating = newRating;
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
