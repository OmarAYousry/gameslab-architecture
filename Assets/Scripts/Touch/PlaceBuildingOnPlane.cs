using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefabs"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceBuildingOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates the prefab from the list on a plane at the touch location.")]
    List<GameObject> m_PlacedPrefabs = new List<GameObject>();

    bool m_spawned = false;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public List<GameObject> placedPrefabs
    {
        get { return m_PlacedPrefabs; }
        set { m_PlacedPrefabs = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

#if UNITY_EDITOR
    void OnValidate()
    {
        m_PlacedPrefabs = new List<GameObject>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:GameObject"));
        //string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(GameObject)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            BuildingBehaviour asset = AssetDatabase.LoadAssetAtPath<BuildingBehaviour>(assetPath);
            if (asset != null)
            {
                m_PlacedPrefabs.Add(asset.gameObject);
            }
        }
    }
#endif

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    Pose hitPose = default;

    void Update()
    {
        // only spawn the building once (through this script at least)
        if (m_spawned)
            return;
        if (BuildingSpawnDialog.isActive())
            return;
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            //var hitPose = s_Hits[0].pose;
            hitPose = s_Hits[0].pose;

            if (spawnedObject == null)
            {
                BuildingSpawnDialog.DisplayBuildingSpawnDialog(m_PlacedPrefabs, setAsSpawned);
                //spawnedObject = Instantiate(m_PlacedPrefabs, hitPose.position, hitPose.rotation);
            }
            //else
            //{
            //    spawnedObject.transform.position = hitPose.position;
            //}
        }
    }

    private void setAsSpawned(GameObject spawnedObject)
    {
        spawnedObject = Instantiate(spawnedObject, hitPose.position, hitPose.rotation);
        this.spawnedObject = spawnedObject;
        m_spawned = true;
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}
