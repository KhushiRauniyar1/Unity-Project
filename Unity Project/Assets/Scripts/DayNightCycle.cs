using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────────────────────
    public static DayNightCycle Instance;

    void Awake()
    {
        Instance = this;
    }

    // ── Inspector Fields ──────────────────────────────────────────────────
    [Header("Duration (seconds)")]
    [SerializeField] private float dayDuration   = 20f;
    [SerializeField] private float nightDuration = 15f;

    [Header("References")]
    [SerializeField] private Light     sunLight;
    [SerializeField] private Transform sunPivot;

    [Header("Sky Colors")]
    [SerializeField] private Color daySky    = new Color(0.53f, 0.81f, 0.98f, 1f);
    [SerializeField] private Color nightSky  = new Color(0.02f, 0.02f, 0.08f, 1f);

    [Header("Sun Colors")]
    [SerializeField] private Color sunDayColor   = new Color(1f,   0.95f, 0.8f,  1f);
    [SerializeField] private Color sunNightColor = new Color(0.05f,0.05f, 0.2f,  1f);

    [Header("Sun Intensity")]
    [SerializeField] private float maxSunIntensity = 1.5f;

    // ── Private ───────────────────────────────────────────────────────────
    private float  timer           = 0f;
    private bool   isDay           = true;
    private int    dayCount        = 1;
    private float  solarMultiplier = 1f;
    private Camera mainCamera;

    // ── Start ─────────────────────────────────────────────────────────────
    void Start()
    {
        // find camera automatically — no need to drag
        mainCamera = Camera.main;

        ApplyTimeOfDay(true);

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateDayCounter(dayCount);
    }

    // ── Update — runs every frame ──────────────────────────────────────────
    void Update()
    {
        timer += Time.deltaTime;

        float duration = isDay ? dayDuration : nightDuration;
        float t        = Mathf.Clamp01(timer / duration);

        if (isDay)
        {
            UpdateDaytime(t);
        }
        else
        {
            UpdateNighttime();
        }

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

            ApplyTimeOfDay(isDay);
        }
    }

    // ── Daytime: sun rises and sets ────────────────────────────────────────
    void UpdateDaytime(float t)
    {
        // sun moves from east to west
        if (sunPivot != null)
        {
            float angle = Mathf.Lerp(-90f, 90f, t);
            sunPivot.rotation = Quaternion.Euler(angle, 0f, 0f);
        }

        // sun intensity peaks at noon using sine curve
        solarMultiplier = Mathf.Sin(t * Mathf.PI);

        if (sunLight != null)
        {
            sunLight.intensity = solarMultiplier * maxSunIntensity;
            sunLight.color     = sunDayColor;
        }

        // sky color transitions
        if (mainCamera != null)
        {
            if (t < 0.2f)
                mainCamera.backgroundColor = Color.Lerp(
                    nightSky, daySky, t / 0.2f);
            else if (t > 0.8f)
                mainCamera.backgroundColor = Color.Lerp(
                    daySky, nightSky, (t - 0.8f) / 0.2f);
            else
                mainCamera.backgroundColor = daySky;
        }
    }

    // ── Nighttime: sun below horizon ───────────────────────────────────────
    void UpdateNighttime()
    {
        solarMultiplier = 0f;

        if (sunLight != null)
        {
            sunLight.intensity = 0f;
            sunLight.color     = sunNightColor;
        }

        if (mainCamera != null)
            mainCamera.backgroundColor = nightSky;
    }

    // ── Apply when phase first switches ───────────────────────────────────
    void ApplyTimeOfDay(bool dayTime)
    {
        isDay = dayTime;

        // tell EnergyManager
        if (EnergyManager.Instance != null)
            EnergyManager.Instance.SetDayMode(dayTime);

        // update UI text
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateTimeUI(dayTime);
    }

    // ── Public getters for other scripts ──────────────────────────────────
    public float GetSolarMultiplier()
    {
        return solarMultiplier;
    }

    public bool IsDay()
    {
        return isDay;
    }
}