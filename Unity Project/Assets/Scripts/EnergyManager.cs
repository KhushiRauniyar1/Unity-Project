// EnergyManager.cs
// Attach to the GameManagers GameObject.
// Collects demand from buildings, supply from energy sources.
// Sends net energy to PollutionManager every frame.

using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;
    public float totalDemand { get; private set; }
    public float totalSupply { get; private set; }
    public float netEnergy { get; private set; }

    private List<Building> buildings = new List<Building>();
    private PollutionManager pollutionManager;

    void Awake() => Instance = this;

    void Start()
    {
        pollutionManager = Object.FindObjectOfType<PollutionManager>();
    }

    public void RegisterBuilding(Building b) => buildings.Add(b);
    public void UnregisterBuilding(Building b) => buildings.Remove(b);

    public void AddSupply(float amount) => totalSupply += amount;
    public void RemoveSupply(float amount) => totalSupply -= amount;

    void Update()
    {
        totalDemand = 0f;

        foreach (var b in buildings)
            totalDemand += b.energyDemand;

        netEnergy = totalSupply - totalDemand;

        pollutionManager?.UpdateEnergyDeficit(netEnergy);
    }
}