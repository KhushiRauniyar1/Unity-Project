using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Energy Tracking")]
    public float totalEnergyDemand = 0f;
    public float totalEnergySupply = 0f;
    
    [Header("Pollution Tracking")]
    public float totalPollution = 0f;
    public float maxPollution = 100f;
    
    [Header("Money System")]
    public float currentMoney = 500f;
    
    [Header("Game State")]
    public bool hasEnoughEnergy = false;
    public float stablePowerTime = 0f;
    public float blackoutTime = 0f;
    
    [Header("Debug Info")]
    public int buildingCount = 0;
    public int energySourceCount = 0;
    
    private List<Building> allBuildings = new List<Building>();
    private List<EnergySource> allEnergySources = new List<EnergySource>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("GameManager initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        CalculateTotals();
        UpdateBuildingPowerStatus();
        UpdateTimers();
        
        // Update debug info
        buildingCount = allBuildings.Count;
        energySourceCount = allEnergySources.Count;
    }
    
    public void RegisterBuilding(Building building)
    {
        if (!allBuildings.Contains(building))
        {
            allBuildings.Add(building);
        }
    }
    
    public void UnregisterBuilding(Building building)
    {
        allBuildings.Remove(building);
    }
    
    public void RegisterEnergySource(EnergySource source)
    {
        if (!allEnergySources.Contains(source))
        {
            allEnergySources.Add(source);
            Debug.Log($"Energy source added: {source.name}");
        }
    }
    
    public void UnregisterEnergySource(EnergySource source)
    {
        allEnergySources.Remove(source);
    }
    
    void CalculateTotals()
    {
        totalEnergyDemand = 0f;
        totalEnergySupply = 0f;
        totalPollution = 0f;
        
        foreach (Building building in allBuildings)
        {
            totalEnergyDemand += building.GetEnergyDemand();
        }
        
        foreach (EnergySource source in allEnergySources)
        {
            totalEnergySupply += source.GetEnergyOutput();
            totalPollution += source.GetPollutionOutput() * Time.deltaTime;
        }
        
        // Natural pollution decay
        totalPollution = Mathf.Max(0, totalPollution - 0.5f * Time.deltaTime);
        
        hasEnoughEnergy = totalEnergySupply >= totalEnergyDemand;
    }
    
    void UpdateBuildingPowerStatus()
    {
        foreach (Building building in allBuildings)
        {
            building.SetPowerStatus(hasEnoughEnergy);
        }
    }
    
    void UpdateTimers()
    {
        if (hasEnoughEnergy)
        {
            stablePowerTime += Time.deltaTime;
            blackoutTime = 0f;
        }
        else
        {
            blackoutTime += Time.deltaTime;
            stablePowerTime = 0f;
        }
    }
    
    public bool CanAfford(float cost)
    {
        return currentMoney >= cost;
    }
    
    public bool SpendMoney(float amount)
    {
        if (CanAfford(amount))
        {
            currentMoney -= amount;
            Debug.Log($"Spent ${amount}. Remaining: ${currentMoney}");
            return true;
        }
        Debug.LogWarning($"Not enough money! Need ${amount}, have ${currentMoney}");
        return false;
    }
    
    public float GetRenewablePercentage()
    {
        if (totalEnergySupply == 0) return 0f;
        
        float renewableEnergy = 0f;
        foreach (EnergySource source in allEnergySources)
        {
            if (source.GetPollutionOutput() == 0)
            {
                renewableEnergy += source.GetEnergyOutput();
            }
        }
        
        return (renewableEnergy / totalEnergySupply) * 100f;
    }
}