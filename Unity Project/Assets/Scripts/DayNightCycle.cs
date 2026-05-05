using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // ── Inspector Fields ─────────────────────────────────────────────────
    [Header("Duration (seconds)")]
    [SerializeField] private float dayDuration   = 20f;
    [SerializeField] private float nightDuration = 15f;

    [Header("References")]
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private SolarPanel solarPanel;
    [SerializeField] private Light sunLight;       // drag your Directional Light here
    [SerializeField] private Transform sunPivot;   // empty object that rotates — sun orbits around it

    [Header("Sun Rotation")]
    [SerializeField] private float sunriseAngle = -90f;  // sun below horizon (east)
    [SerializeField] private float sunsetAngle  =  90f;  // sun below horizon (west)
    [SerializeField] private float noonAngle    =   0f;  // sun directly above (noon)

    [Header("Sun Light Colors")]
    [SerializeField] private Color sunriseColor = new Color(1f,   0.5f,  0.2f);  // orange sunrise
    [SerializeField] private Color noonColor    = new Color(1f,   0.95f, 0.8f);  // warm white noon
    [SerializeField] private Color sunsetColor  = new Color(1f,   0.4f,  0.1f);  // deep orange sunset
    [SerializeField] private Color nightColor   = new Color(0.05f,0.05f, 0.2f);  // dark blue night

    [Header("Sun Intensity")]
    [SerializeField] private float maxIntensity  = 1.5f;  // brightest at noon
    [SerializeField] private float nightIntensity = 0f;   // off at night

    [Header("Sky Colors")]
    [SerializeField] private Color daySkyColor    = new Color(0.53f, 0.81f, 0.98f); // light blue
    [SerializeField] private Color sunriseSkyColor = new Color(1f,   0.6f,  0.3f);  // orange sky
    [SerializeField] private Color nightSkyColor   = new Color(0.02f,0.02f, 0.08f); // very dark

    // ── Private ───────────────────────────────────────────────────────────
    private float  timer    = 0f;
    private bool   isDay    = true;
    private int    dayCount = 1;
    private Camera mainCamera;

    // ── Start ─────────────────────────────────────────────────────────────
    void Start()
    {
        mainCamera = Camera.main;
        ApplyTimeOfDay(true);

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateDayCounter(dayCount);
    }

    // ── Update — runs every frame ─────────────────────────────────────────
    void Update()
    {
        timer += Time.deltaTime;

        float duration = isDay ? dayDuration : nightDuration;
        float progress = Mathf.Clamp01(timer / duration); // 0 to 1

        if (isDay)
            UpdateDaytime(progress);
        else
            UpdateNighttime(progress);

        // switch phase when timer runs out
        if (timer >= duration)
        {
            timer = 0f;
            isDay = !isDay;

            if (isDay)
            {
                dayCount++;
                if (UIManager.Instance != null)
                    UIManager.Instance.UpdateDayCounter(dayCount);
            }

            // notify other scripts
            if (energyManager != null)
                energyManager.SetDayMode(isDay);

            if (solarPanel != null)
                solarPanel.SetDayMode(isDay);

            if (UIManager.Instance != null)
                UIManager.Instance.UpdateTimeUI(isDay);
        }
    }

    // ── Daytime: sun rises from east, crosses sky, sets in west ──────────
    private void UpdateDaytime(float t)
    {
        // t = 0 is sunrise, t = 0.5 is noon, t = 1 is sunset

        // rotate the sun pivot — sun moves from -90 to +90 degrees
        if (sunPivot != null)
        {
            float angle = Mathf.Lerp(sunriseAngle, sunsetAngle, t);
            sunPivot.rotation = Quaternion.Euler(angle, 0f, 0f);
        }

        if (sunLight != null)
        {
            // intensity: low at sunrise, peak at noon, low at sunset
            // uses a sine curve so it feels natural
            float intensity = Mathf.Sin(t * Mathf.PI) * maxIntensity;
            sunLight.intensity = intensity;

            // color: orange sunrise → white noon → orange sunset
            if (t < 0.5f)
                sunLight.color = Color.Lerp(sunriseColor, noonColor, t * 2f);
            else
                sunLight.color = Color.Lerp(noonColor, sunsetColor, (t - 0.5f) * 2f);
        }

        // sky color: orange at sunrise → blue at noon → orange at sunset
        if (mainCamera != null)
        {
            Color skyColor;
            if (t < 0.2f)
                skyColor = Color.Lerp(nightSkyColor, sunriseSkyColor, t / 0.2f);
            else if (t < 0.5f)
                skyColor = Color.Lerp(sunriseSkyColor, daySkyColor, (t - 0.2f) / 0.3f);
            else if (t < 0.8f)
                skyColor = Color.Lerp(daySkyColor, sunriseSkyColor, (t - 0.5f) / 0.3f);
            else
                skyColor = Color.Lerp(sunriseSkyColor, nightSkyColor, (t - 0.8f) / 0.2f);

            mainCamera.backgroundColor = skyColor;
        }
    }

    // ── Nighttime: sun stays below horizon, sky stays dark ────────────────
    private void UpdateNighttime(float t)
    {
        // keep sun below horizon during night
        if (sunPivot != null)
        {
            float angle = Mathf.Lerp(sunsetAngle, sunriseAngle + 360f, t);
            sunPivot.rotation = Quaternion.Euler(angle, 0f, 0f);
        }

        // sun light off at night
        if (sunLight != null)
        {
            sunLight.intensity = nightIntensity;
            sunLight.color     = nightColor;
        }

        // sky stays dark
        if (mainCamera != null)
            mainCamera.backgroundColor = nightSkyColor;
    }

    // ── Helper called when phase first starts ─────────────────────────────
    private void ApplyTimeOfDay(bool dayTime)
    {
        isDay = dayTime;

        if (energyManager != null) energyManager.SetDayMode(dayTime);
        if (solarPanel    != null) solarPanel.SetDayMode(dayTime);
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimeUI(dayTime);
            UIManager.Instance.UpdateDayCounter(dayCount);
        }
    }
}