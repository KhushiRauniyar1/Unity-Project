using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    public SpriteRenderer panelRenderer;

    public Color activeColor = new Color(1f, 0.85f, 0f, 1f);   // Bright yellow
    public Color inactiveColor = new Color(0.4f, 0.4f, 0.4f, 1f); // Grey

    private bool isDay = false;

    // Called by DayNightCycle — add this call inside ApplyTimeOfDay()
    public void SetDayMode(bool dayTime)
    {
        isDay = dayTime;

        if (panelRenderer != null)
            panelRenderer.color = isDay ? activeColor : inactiveColor;
    }
}