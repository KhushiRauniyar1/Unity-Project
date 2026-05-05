using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // ── Singleton so any script can call UIManager.Instance ──────────────
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // ── UI References — drag all of these in Inspector ───────────────────
    [Header("Battery UI")]
    public Slider batterySlider;
    public Text batteryText;

    [Header("Status UI")]
    public Text statusText;         // Shows DAY or NIGHT
    public Text switchStatusText;   // Shows BULB ON / BULB OFF

    [Header("Panels")]
    public GameObject warningPanel;   // Red low battery panel
    public GameObject gameOverPanel;  // Game over screen

    [Header("Day Counter (optional)")]
    public Text dayCounterText;       // Shows Day 1, Day 2...

    // ── Called by EnergyManager every frame ──────────────────────────────
    public void UpdateBatteryUI(float current, float max)
    {
        // Update the slider bar
        batterySlider.value = current;

        // Calculate and show percentage
        int pct = Mathf.RoundToInt((current / max) * 100);
        batteryText.text = pct + "%";

        // Change slider fill color based on level
        UpdateBatteryColor(pct);

        // Show red warning panel when battery is low
        warningPanel.SetActive(pct < 20);
    }

    // ── Called by DayNightCycle when time changes ─────────────────────────
    public void UpdateTimeUI(bool isDay)
    {
        statusText.text = isDay ? "DAY  ☀" : "NIGHT  🌙";
    }

    // ── Called by SwitchController when player clicks button ─────────────
    public void UpdateSwitchUI(bool isBulbOn)
    {
        if (switchStatusText != null)
            switchStatusText.text = isBulbOn ? "Bulb: ON" : "Bulb: OFF";
    }

    // ── Called by EnergyManager when battery hits 0 ───────────────────────
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    // ── Called when a new day starts (optional feature) ───────────────────
    public void UpdateDayCounter(int day)
    {
        if (dayCounterText != null)
            dayCounterText.text = "Day " + day;
    }

    // ── Changes battery bar color: green → yellow → red ──────────────────
    void UpdateBatteryColor(int pct)
    {
        // Get the fill image of the slider
        Image fillImage = batterySlider.fillRect.GetComponent<Image>();
        if (fillImage == null) return;

        if (pct > 50)
            fillImage.color = new Color(0.15f, 0.68f, 0.38f); // Green
        else if (pct > 20)
            fillImage.color = new Color(0.95f, 0.62f, 0.07f); // Yellow/Orange
        else
            fillImage.color = new Color(0.91f, 0.30f, 0.24f); // Red
    }
}