using UnityEngine;
 
public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;
 
    public float dayDuration = 30f;
 
    public bool  isDay      { get; private set; } = true;
    public bool  isNight    => !isDay;
    public float sunIntensity { get; private set; } = 1f;
 
    private Light sunLight;
    private float timeOfDay = 0.25f;
 
    void Awake()
    {
        Instance = this;
        sunLight = GetComponent<Light>();
    }
 
    void Update()
    {
        timeOfDay += Time.deltaTime / dayDuration;
        if (timeOfDay >= 1f) timeOfDay = 0f;
 
        float sunAngle = (timeOfDay * 360f) - 90f;
        transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
 
        sunIntensity = Mathf.Clamp01(
            Mathf.Sin(timeOfDay * Mathf.PI * 2f));
 
        if (sunLight != null)
            sunLight.intensity = sunIntensity * 1.5f;
 
        bool wasDay = isDay;
        isDay = timeOfDay > 0.05f && timeOfDay < 0.95f;
 
        RenderSettings.ambientLight = Color.Lerp(
            new Color(0.02f, 0.02f, 0.08f),
            new Color(0.8f,  0.85f, 1.0f),
            sunIntensity);
 
        if (wasDay != isDay)
            Debug.Log(isDay
                ? "DAY  — lights OFF"
                : "NIGHT — street lights ON");
    }
}
 