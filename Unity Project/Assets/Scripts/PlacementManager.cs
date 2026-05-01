// PlacementManager.cs
// Attach to an empty GameObject named PlacementSystem.
// Press 1 = Solar, 2 = Wind, 3 = Coal, 4 = Waste Processor.
// Left-click to place. Escape to cancel.

using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    // Drag your 4 prefabs into these slots in the Inspector:
    // [0]=Solar [1]=Wind [2]=Coal [3]=Waste
    public GameObject[] buildingPrefabs;

    private int selectedIndex = -1;
    private GameObject previewObj;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Select building with keys 1–4
        for (int i = 0; i < buildingPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectBuilding(i);
        }

        if (selectedIndex < 0) return;

        // Raycast from mouse
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Move preview object
            if (previewObj != null)
                previewObj.transform.position = hit.point;

            // Place building
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(
                    buildingPrefabs[selectedIndex],
                    hit.point,
                    Quaternion.identity
                );
            }
        }

        // Cancel selection
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelSelection();
    }

    void SelectBuilding(int index)
    {
        CancelSelection();

        selectedIndex = index;

        if (buildingPrefabs[index] != null)
            previewObj = Instantiate(buildingPrefabs[index]);
    }

    void CancelSelection()
    {
        selectedIndex = -1;

        if (previewObj != null)
            Destroy(previewObj);
    }
}