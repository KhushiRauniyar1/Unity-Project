// WasteProcessingPlant.cs
// Attach to the WasteProcessor prefab.
// Reduces waste in WasteManager.
// Adds slight pollution from the processing operation.

using UnityEngine;

public class WasteProcessingPlant : MonoBehaviour
{
    public float wasteReduction = 3f;     // Units reduced per second
    public float operatePollution = 0.5f; // Small pollution from running

    void Start()
    {
        WasteManager.Instance?.AddReduction(wasteReduction);
        PollutionManager.Instance?.AddStaticPollution(operatePollution);
    }

    void OnDestroy()
    {
        WasteManager.Instance?.RemoveReduction(wasteReduction);
        PollutionManager.Instance?.RemoveStaticPollution(operatePollution);
    }
}