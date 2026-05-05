using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public SolarPanel solarpanel;

    [Header("Duration (seconds)")]
    public float dayDuration = 20f;
    public float nightDuration = 15f;

    [Header("References")]
    public EnergyManager energyManager;
    public Camera mainCamera;

    public Color dayColor = new Color(0.53f, 0.81f, 0.98f, 1f);
    public Color nightColor = new Color(0.05f, 0.05f, 0.15f, 1f);

    private float timer = 0f;
    private bool isDay = true;

    void Start()
    {
        ApplyTimeOfDay(true);
    }

    void Update()
    {
        timer += Time.deltaTime;

        float dur = isDay ? dayDuration : nightDuration;

        if (timer >= dur)
        {
            timer = 0f;
            isDay = !isDay;
            ApplyTimeOfDay(isDay);
        }
    }

    void ApplyTimeOfDay(bool dayTime)
    {
        isDay = dayTime;

        if (mainCamera != null)
            mainCamera.backgroundColor = dayTime ? dayColor : nightColor;

        if (energyManager != null)
            energyManager.SetDayMode(dayTime);

        if (solarpanel != null)
            solarpanel.SetDayMode(dayTime);
    }
}