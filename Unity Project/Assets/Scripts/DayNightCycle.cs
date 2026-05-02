using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;

    [Header("Speed")]
    // How many seconds = 1 full day (default 120 = 2 minutes per day)
    public float dayDuration = 120f;

    [Header("Lighting")]
    public Color dayAmbient   = new Color(1f,   0.95f, 0.8f);  // warm white
    public Color nightAmbient = new Color(0.05f,0.05f, 0.15f); // dark blue

    // ── Public state ──────────────────────────────
    public bool isDay   { get; private set; } = true;
    public bool isNight => !isDay;

    // How bright the sun is right now (0=midnight, 1=noon)
    public float sunIntensity { get; private set; } = 1f;

    // ── Private ───────────────────────────────────
    private Light sunLight;
    private float timeOfDay = 0f;   // 0 to 1 (0=midnight, 0.5=noon)

    void Awake()
    {
        Instance = this;
        sunLight = GetComponent<Light>();
    }

    void Update()
    {
        // Advance time
        timeOfDay += Time.deltaTime / dayDuration;
        if (timeOfDay >= 1f) timeOfDay = 0f;

        // Rotate the sun  (0 = midnight below, 0.5 = noon above)
        float sunAngle = (timeOfDay * 360f) - 90f;
        transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // Sun intensity = how much above horizon
        // sin of angle: positive = day, negative = night
        float angle = timeOfDay * Mathf.PI * 2f;
        sunIntensity = Mathf.Clamp01(Mathf.Sin(angle));

        // Update directional light brightness
        if (sunLight != null)
            sunLight.intensity = sunIntensity;

        // Day = sun is above horizon  (timeOfDay 0.1 to 0.9)
        bool wasDay = isDay;
        isDay = timeOfDay > 0.1f && timeOfDay < 0.9f;

        // Smoothly blend ambient light
        RenderSettings.ambientLight = Color.Lerp(
            nightAmbient, dayAmbient, sunIntensity);

        // Log when day/night switches
        if (wasDay != isDay)
            Debug.Log(isDay ? "🌅 DAY — solar charging starts"
                            : "🌙 NIGHT — battery powering city");
    }
}