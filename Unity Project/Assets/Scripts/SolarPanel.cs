using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    [Header("Solar Panel Light")]
    [SerializeField] private Light panelLight;        // drag SolarPanelLight here

    [Header("Light Settings")]
    [SerializeField] private Color dayColor   = new Color(1f, 0.95f, 0.5f);  // warm yellow
    [SerializeField] private Color nightColor = new Color(0.2f, 0.2f, 0.5f); // dim blue
    [SerializeField] private float dayIntensity   = 2f;  // bright during day
    [SerializeField] private float nightIntensity = 0f;  // off at night

    void Start()
    {
        // start as daytime
        SetDayMode(true);
    }

    // called by DayNightCycle
    public void SetDayMode(bool isDay)
    {
        if (panelLight == null) return;

        panelLight.enabled   = true;
        panelLight.color     = isDay ? dayColor   : nightColor;
        panelLight.intensity = isDay ? dayIntensity : nightIntensity;
    }
}