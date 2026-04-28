using UnityEngine;

public class EnergySource : MonoBehaviour
{
    [Header("Energy Production")]
    public float energyOutput = 0f;
    
    [Header("Pollution Production")]
    public float pollutionOutput = 0f;
    
    [Header("Cost")]
    public float buildCost = 0f;
    
    [Header("Visual Info")]
    public string sourceName = "Energy Source";
    
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterEnergySource(this);
        }
    }
    
    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterEnergySource(this);
        }
    }
    
    public float GetEnergyOutput()
    {
        return energyOutput;
    }
    
    public float GetPollutionOutput()
    {
        return pollutionOutput;
    }
    
    public float GetBuildCost()
    {
        return buildCost;
    }
}