using UnityEngine;

public class Bulb : MonoBehaviour
{
    public EnergyManager energyManager;

    public SpriteRenderer bulbRenderer;
    public SpriteRenderer glowRenderer;

    public Color bulbOnColor = new Color(1f, 1f, 0.4f, 1f);
    public Color bulbOffColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    public Color glowOnColor = new Color(1f, 1f, 0f, 0.6f);
    public Color glowOffColor = new Color(0f, 0f, 0f, 0f);

    public bool enableFlicker = true;
    public float flickerSpeed = 0.1f;
    public float lowBatteryThreshold = 20f;

    private bool isBulbOn = false;

    private float flickerTimer = 0f;
    private bool flickerState = true;

    void Update()
    {
        if (isBulbOn && enableFlicker && BatteryIsLow())
        {
            flickerTimer += Time.deltaTime;

            if (flickerTimer >= flickerSpeed)
            {
                flickerTimer = 0f;
                flickerState = !flickerState;
                ApplyVisual(flickerState);
            }
        }
    }

    public void SetBulbState(bool on)
    {
        isBulbOn = on;
        flickerTimer = 0f;
        flickerState = true;

        ApplyVisual(on);
    }

    void ApplyVisual(bool on)
    {
        if (bulbRenderer != null)
            bulbRenderer.color = on ? bulbOnColor : bulbOffColor;

        if (glowRenderer != null)
            glowRenderer.color = on ? glowOnColor : glowOffColor;
    }

    bool BatteryIsLow()
    {
        if (energyManager == null) return false;

        float pct = (energyManager.currentBattery / energyManager.maxBattery) * 100f;

        return pct < lowBatteryThreshold;
    }
}