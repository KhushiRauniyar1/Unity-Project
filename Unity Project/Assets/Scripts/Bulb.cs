using UnityEngine;

public class Bulb : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private Light bulbLight;

    [Header("Light Settings")]
    [SerializeField] private Color lightColor   = new Color(1f, 0.9f, 0.5f);
    [SerializeField] private float onIntensity  = 3f;
    [SerializeField] private float onRange      = 10f;

    [Header("Flicker Settings")]
    [SerializeField] private bool  enableFlicker       = true;
    [SerializeField] private float flickerSpeed        = 0.08f;
    [SerializeField] private float lowBatteryThreshold = 20f;

    private bool  isBulbOn     = false;
    private float flickerTimer = 0f;
    private bool  flickerState = true;

    void Start()
    {
        // make sure light is OFF at start
        if (bulbLight != null)
        {
            bulbLight.color     = lightColor;
            bulbLight.range     = onRange;
            bulbLight.intensity = 0f;
            bulbLight.enabled   = false;
        }
    }

    void Update()
    {
        // flicker when battery is low
        if (isBulbOn && enableFlicker && BatteryIsLow())
        {
            flickerTimer += Time.deltaTime;
            if (flickerTimer >= flickerSpeed)
            {
                flickerTimer = 0f;
                flickerState = !flickerState;
                ApplyLight(flickerState);
            }
        }
    }

    // called by SwitchController
    public void SetBulbState(bool on)
    {
        isBulbOn     = on;
        flickerTimer = 0f;
        flickerState = true;
        ApplyLight(on);
    }

    private void ApplyLight(bool on)
    {
        if (bulbLight == null) return;

        bulbLight.enabled   = on;
        bulbLight.intensity = on ? onIntensity : 0f;
    }

    private bool BatteryIsLow()
    {
        if (energyManager == null) return false;
        float pct = (energyManager.currentBattery / energyManager.MaxBattery) * 100f;
        return pct < lowBatteryThreshold;
    }
}