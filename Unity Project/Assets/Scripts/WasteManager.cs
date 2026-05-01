// WasteManager.cs
// Attach to the GameManagers GameObject.
// Tracks waste from all buildings.
// Sends total waste to PollutionManager every frame.

using System.Collections.Generic;
using UnityEngine;

public class WasteManager : MonoBehaviour
{
    public static WasteManager Instance;

    public float totalWaste { get; private set; }
    public float wasteReduction { get; private set; }
    public float maxWaste = 1000f;

    private List<Building> buildings = new List<Building>();
    private PollutionManager pollutionManager;

    void Awake() => Instance = this;

    void Start()
    {
        pollutionManager = Object.FindObjectOfType<PollutionManager>();
    }

    public void RegisterBuilding(Building b) => buildings.Add(b);
    public void UnregisterBuilding(Building b) => buildings.Remove(b);

    public void AddReduction(float v) => wasteReduction += v;
    public void RemoveReduction(float v) => wasteReduction -= v;

    void Update()
    {
        float generation = 0f;

        foreach (var b in buildings)
            generation += b.wastePerSecond;

        totalWaste += (generation - wasteReduction) * Time.deltaTime;
        totalWaste = Mathf.Max(0f, totalWaste);

        pollutionManager?.UpdateWaste(totalWaste);
    }
}