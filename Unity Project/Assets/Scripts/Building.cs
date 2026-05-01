// Building.cs
// Attach to every building prefab.
// Sends energyDemand and wastePerSecond to managers.
// NEVER communicates with the UI directly.

using UnityEngine;

public class Building : MonoBehaviour
{
    public float energyDemand = 10f;   // Set per building in Inspector
    public float wastePerSecond = 1f;  // Waste generated every second

    private EnergyManager energyManager;
    private WasteManager wasteManager;

    void Start()
    {
        energyManager = Object.FindObjectOfType<EnergyManager>();
        wasteManager = Object.FindObjectOfType<WasteManager>();

        if (energyManager != null) energyManager.RegisterBuilding(this);
        if (wasteManager != null) wasteManager.RegisterBuilding(this);
    }

    void OnDestroy()
    {
        if (energyManager != null) energyManager.UnregisterBuilding(this);
        if (wasteManager != null) wasteManager.UnregisterBuilding(this);
    }
}
