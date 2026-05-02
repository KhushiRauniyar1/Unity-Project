using UnityEngine;
 
public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;
 
    // How many seconds = 1 full day + night cycle
    // 30  = very fast (good for testing)
    // 120 = normal speed
    // 300 = slow and realistic
   public float dayDuration = 30f;   // slow day
public float nightDuration = 120f;  // fast night
    // Public state read by Building.cs every frame
    public bool  isDay      { get; private set; } = true;
    public bool  isNight    => !isDay;
    public float sunIntensity { get; private set; } = 1f;
 
    private Light sunLight;
    private float timeOfDay = 0.25f; // start at morning
 
    void Awake()
    {
        Instance = this;
        sunLight = GetComponent<Light>();
    }
 
    void Update()
    {
        // Advance the time of day
        timeOfDay += Time.deltaTime / dayDuration;
        if (timeOfDay >= 1f) timeOfDay = 0f;
 
        // Rotate sun across the sky
        float sunAngle = (timeOfDay * 360f) - 90f;
        transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
 
        // Sun brightness = sine curve (0 at midnight, 1 at noon)
        sunIntensity = Mathf.Clamp01(
            Mathf.Sin(timeOfDay * Mathf.PI * 2f));
 
        // Set directional light brightness
        if (sunLight != null)
            sunLight.intensity = sunIntensity * 1.5f;
 
        // Day = sun above horizon
        bool wasDay = isDay;
        isDay = timeOfDay > 0.05f && timeOfDay < 0.95f;
 
        // Blend sky color from very dark blue (night) to bright (day)
        RenderSettings.ambientLight = Color.Lerp(
            new Color(0.02f, 0.02f, 0.08f),  // very dark night
            new Color(0.8f,  0.85f, 1.0f),   // bright day
            sunIntensity);
 
        // Log when cycle switches
        if (wasDay != isDay)
            Debug.Log(isDay
                ? "DAY  — solar charging, building lights OFF"
                : "NIGHT — building lights ON if solar connected");
    }
}