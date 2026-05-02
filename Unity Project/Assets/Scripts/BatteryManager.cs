using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    public static BatteryManager Instance;

    [Header("Battery")]
    public float maxCharge     = 100f;  // full battery = 100
    public float currentCharge = 0f;    // starts empty

    // Drain per building per second at night
    public float drainPerBuilding = 0.5f;

    void Awake() => Instance = this;

    // Called by solar panels during day
    public void AddCharge(float amount)
    {
        currentCharge = Mathf.Clamp(currentCharge + amount,
                                    0f, maxCharge);
    }

    // Called by buildings at night
    // Returns true if there was enough charge
    public bool UseCharge(float amount)
    {
        if (currentCharge <= 0f) return false;
        currentCharge = Mathf.Max(0f, currentCharge - amount);
        return true;
    }

    // Is there enough power for the city at night?
    public bool HasCharge => currentCharge > 0f;
}