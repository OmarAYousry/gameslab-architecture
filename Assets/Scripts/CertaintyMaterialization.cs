using UnityEngine;

public class CertaintyMaterialization : MonoBehaviour
{
    public static CertaintyMaterialization instance = null;

    [Tooltip("Sort Ascendingly")]
    [SerializeField]
    private Material[] certaintyMatsSorted = null;

    public static Material[] CertaintyMats { get { return instance.certaintyMatsSorted; } }

    [SerializeField]
    private Shader geometricCertaintyShader = null;

    public static Shader GeometricCertaintyShader { get { return instance.geometricCertaintyShader; } }

    public static bool isPreviewSemanticCertainty { get; private set; } = false;

    public static bool isPreviewGeometricCertainty { get; private set; } = false;

    void Awake()
    {
        instance = this;        
    }

    public void ToggleSemanticCertainty(bool applyingCertainty)
    {
        isPreviewSemanticCertainty = applyingCertainty;

        foreach (InteractableObject interObj in InteractableObject.interactables)
        {
            interObj.ToggleSemanticCertaity(applyingCertainty, certaintyMatsSorted);
        }
    }

    public void ToggleGeometricCertainty(bool applyingCertainty)
    {
        isPreviewGeometricCertainty = applyingCertainty;

        foreach (InteractableObject interObj in InteractableObject.interactables)
        {
            interObj.ToggleGeometricCertainty(applyingCertainty, geometricCertaintyShader); ;
        }
    }
}
