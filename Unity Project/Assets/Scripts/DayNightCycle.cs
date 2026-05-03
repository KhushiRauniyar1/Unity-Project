// ─────────────────────────────────────────────────────────────────
// DayNightCycle.cs
// Attach to: GameManager (same object as EnergyManager)
// Role: Timer that flips day/night and updates visual sky color
// ─────────────────────────────────────────────────────────────────
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Timing")]
    public float dayDuration = 30f;        // Seconds the day phase lasts
    public float nightDuration = 20f;      // Seconds the night phase lasts

    [Header("Visual References")]
    public Light sunLight;                  // Drag your SunLight here
    public Camera mainCamera;              // Drag Main Camera here

    [Header("Sky Colors")]
    public Color dayColor = new Color(0.52f, 0.80f, 0.98f);  // Blue sky
    public Color nightColor = new Color(0.05f, 0.05f, 0.15f); // Dark navy

    // Internal timer
    private float timer = 0f;

    void Update()
    {
        // Pause cycle if game is over
        if (EnergyManager.Instance != null && EnergyManager.Instance.isGameOver)
            return;

        timer += Time.deltaTime;

        // Determine current phase duration
        bool isCurrentlyDay = EnergyManager.Instance.isDay;
        float phaseDuration = isCurrentlyDay ? dayDuration : nightDuration;

        // When timer exceeds phase duration, flip day/night
        if (timer >= phaseDuration)
        {
            timer = 0f;                                           // Reset timer
            EnergyManager.Instance.isDay = !isCurrentlyDay;     // Toggle day/night
        }

        // ── Smooth visual transition ──
        float t = Mathf.Clamp01(timer / phaseDuration);
        Color targetColor = EnergyManager.Instance.isDay ? dayColor : nightColor;
        Color fromColor = EnergyManager.Instance.isDay ? nightColor : dayColor;

        // Update camera background (sky color)
        if (mainCamera != null)
            mainCamera.backgroundColor = Color.Lerp(fromColor, targetColor, t);

        // Update sun brightness
        if (sunLight != null)
            sunLight.intensity = EnergyManager.Instance.isDay ?
                Mathf.Lerp(0.2f, 1.2f, t) : Mathf.Lerp(1.2f, 0.1f, t);
    }

    // Called by EnergyManager.RestartGame()
    public void ResetCycle()
    {
        timer = 0f;
        EnergyManager.Instance.isDay = true;
    }
}
