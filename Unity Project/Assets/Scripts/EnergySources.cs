// EnergySource.cs
// Attach to Solar Panel, Wind Turbine, and Coal Plant prefabs.
// Solar Panel: supplyAmount = 10, pollutionPerSec = 0
// Wind Turbine: supplyAmount = 25, pollutionPerSec = 0
// Coal Plant: supplyAmount = 60, pollutionPerSec = 5

using UnityEngine;

public class EnergySource : MonoBehaviour
{
    public float supplyAmount = 20f;
    public float pollutionPerSec = 0f;

    void Start()
    {
        EnergyManager.Instance?.AddSupply(supplyAmount);
        PollutionManager.Instance?.AddStaticPollution(pollutionPerSec);
    }

    void OnDestroy()
    {
        EnergyManager.Instance?.RemoveSupply(supplyAmount);
        PollutionManager.Instance?.RemoveStaticPollution(pollutionPerSec);
    }
}