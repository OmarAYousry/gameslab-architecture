using UnityEngine;

public class CertaintyMaterialization : MonoBehaviour
{
    private static CertaintyMaterialization instance = null;

    [Tooltip("Sort Ascendingly")]
    [SerializeField]
    private Material[] certaintyMatsSorted = null;

    public static Material[] CertaintyMats { get { return instance.certaintyMatsSorted; } }

    public static bool isPreviewCertainty { get; private set; } = false;

    void Awake()
    {
        instance = this;        
    }

    public void ToggleCertaintyMats(bool applyingCertainty)
    {
        isPreviewCertainty = applyingCertainty;

        foreach (InteractableObject interObj in InteractableObject.interactables)
        {
            interObj.ToggleCertaintyMaterial(applyingCertainty, certaintyMatsSorted);
        }
    }
}
