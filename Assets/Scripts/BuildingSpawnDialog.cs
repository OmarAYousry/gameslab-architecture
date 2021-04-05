using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnDialog : MonoBehaviour
{
    private static BuildingSpawnDialog instance = null;

    [SerializeField]
    private UnityEngine.UI.Dropdown buildingDropdown;

    [SerializeField]
    private UnityEngine.UI.Button confirmButton;

    private static System.Action<GameObject> spawnAction = null;

    private static int currentBuildingIndex = 0;

    private static List<GameObject> spawnableBuildings = null;

    private void Awake()
    {
        instance = this;
        Debug.LogError("Wake me up lul");
        gameObject.SetActive(false);
    }

    public void UpdateChosenBuildingIndex(int newBuildingIndex)
    {
        currentBuildingIndex = newBuildingIndex;
    }

    public void OnConfirmButtonClicked()
    {
        string currentBuildingName = instance.buildingDropdown.options[currentBuildingIndex].text;
        GameObject buildingToSpawn = spawnableBuildings.Find(building => building.name == currentBuildingName);
        spawnAction(buildingToSpawn);
        instance.gameObject.SetActive(false);
        //instance.Invoke("disableMe", 3f);
    }

    private void disableMe()
    {
        gameObject.SetActive(false);
    }

    public static void DisplayBuildingSpawnDialog(List<GameObject> spawnableBuildings, System.Action<GameObject> spawnAction)
    {
        BuildingSpawnDialog.spawnableBuildings = spawnableBuildings;
        BuildingSpawnDialog.spawnAction = spawnAction;

        List<string> spawnableBuildingNames = new List<string>();
        foreach (GameObject building in spawnableBuildings)
        {
            spawnableBuildingNames.Add(building.name);
        }

        instance.buildingDropdown.ClearOptions();
        instance.buildingDropdown.AddOptions(spawnableBuildingNames);
        ////instance.buildingDropdown.onValueChanged = UpdateChosenBuildingIndex;
        //UnityEngine.UI.Dropdown.DropdownEvent d = new UnityEngine.UI.Dropdown.DropdownEvent();
        //d.AddListener(UpdateChosenBuildingIndex);
        //currentBuildingIndex = instance.buildingDropdown.optio

        instance.gameObject.SetActive(true);

    }

    public static bool isActive()
    {
        return instance.gameObject.activeInHierarchy;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
