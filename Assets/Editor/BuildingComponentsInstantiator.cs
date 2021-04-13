using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildingComponentsInstantiator : Editor
{
    public static string buildsFolderName = "Buildings";

    [MenuItem("Assets/Create Building Prefab")]
    static void createBuildingPrefab()
    {
        var selected = Selection.activeObject;

        if (!AssetDatabase.IsValidFolder("Assets/" + buildsFolderName))
            AssetDatabase.CreateFolder("Assets", buildsFolderName);
        string localPath = "Assets/" + buildsFolderName + "/" + selected.name + ".prefab";

        var instanceRoot = PrefabUtility.InstantiatePrefab(selected);
        GameObject prefabRoot = PrefabUtility.SaveAsPrefabAsset((GameObject)instanceRoot, localPath);
        DestroyImmediate((GameObject)instanceRoot);

        prefabRoot.AddComponent<MultiTouchBehaviour>();
        prefabRoot.AddComponent<BuildingBehaviour>();

        Camera cam = prefabRoot.GetComponentInChildren<Camera>();
        if (cam != null)
            cam.enabled = false;

        foreach (Transform child in prefabRoot.transform)
        {
            MeshFilter meshFiler = child.gameObject.GetComponent<MeshFilter>();
            if (meshFiler != null)
            {
                BoxCollider collider = child.gameObject.AddComponent<BoxCollider>();
                InteractableObject interactibleObj = child.gameObject.AddComponent<InteractableObject>();
                TransformControl transformControl = child.gameObject.GetComponent<TransformControl>();

                collider.isTrigger = true;
                transformControl.enabled = false;
            }
        }
    }
}
