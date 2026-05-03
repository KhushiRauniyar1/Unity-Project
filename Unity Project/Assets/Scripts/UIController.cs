/ ─────────────────────────────────────────────────────────────────
// UIController.cs
// Attach to: Canvas
// Role: Reads EnergyManager state, updates ALL UI visuals
// ─────────────────────────────────────────────────────────────────
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Battery UI")]
    public Slider batterySlider;           // Drag BatterySlider here
    public Image batteryFill;             // Drag Slider → Fill Area → Fill (Image)
    public TMP_Text batteryPercentText;   // Text showing "72%"

    [Header("Day/Night UI")]
    public TMP_Text dayNightText;          // Shows "☀️ DAY" or "🌙 NIGHT"

    [Header("Warning")]
    public TMP_Text warningText;           // Low battery warning message
    public float warningThreshold = 20f;  // Show warning below 20%

    [Header("Game Over")]
    public GameObject gameOverPanel;       // Drag GameOverPanel here

    // Colors for battery bar
    private Color colorFull  = new Color(0.18f, 0.80f, 0.44f);  // Green
    private Color colorMid   = new Color(1.00f, 0.76f, 0.03f);  // Yellow
    private Color colorLow   = new Color(0.93f, 0.26f, 0.21f);  // Red

    private bool warningBlink = false;
    private float blinkTimer = 0f;

    void Start()
    {
        // Set slider range to match battery max
        batterySlider.minValue = 0f;
        batterySlider.maxValue = EnergyManager.Instance.maxBattery;

        // Hide panels on start
        if (gameOverPanel != null)  gameOverPanel.SetActive(false);
        if (warningText != null)    warningText.gameObject.SetActive(false);
    }

    void Update()
    {
        EnergyManager em = EnergyManager.Instance;
        if (em == null) return;

        // ── 1. Update Battery Slider ──
        batterySlider.value = em.currentBattery;

        // ── 2. Update Battery % Text ──
        float pct = (em.currentBattery / em.maxBattery) * 100f;
        if (batteryPercentText != null)
            batteryPercentText.text = Mathf.RoundToInt(pct) + "%";

        // ── 3. Color the battery bar based on charge level ──
        if (batteryFill != null)
        {
            if (pct > 50f)      batteryFill.color = colorFull;
            else if (pct > 20f) batteryFill.color = colorMid;
            else                 batteryFill.color = colorLow;
        }

        // ── 4. Update Day/Night Indicator ──
        if (dayNightText != null)
        {
            dayNightText.text = em.isDay ?
                "☀️  DAY  — Solar Charging" :
                "🌙  NIGHT — Battery Draining";
            dayNightText.color = em.isDay ?
                new Color(1f, 0.85f, 0.1f) :
                new Color(0.6f, 0.8f, 1.0f);
        }

        // ── 5. Low Battery Warning (blinks when < threshold) ──
        if (warningText != null)
        {
            bool showWarning = pct < warningThreshold && !em.isGameOver;
            warningText.gameObject.SetActive(showWarning);

            if (showWarning)
            {
                blinkTimer += Time.deltaTime;
                if (blinkTimer > 0.5f)
                {
                    warningBlink = !warningBlink;
                    warningText.enabled = warningBlink;
                    blinkTimer = 0f;
                }
                warningText.text = "⚠️  LOW BATTERY! Turn off the bulb!";
            }
        }

        // ── 6. Game Over Panel ──
        if (gameOverPanel != null)
            gameOverPanel.SetActive(em.isGameOver);
    }

    // Called by RestartButton's OnClick event in Inspector
    public void OnRestartClicked()
    {
        EnergyManager.Instance.RestartGame();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (warningText != null)   warningText.gameObject.SetActive(false);
    }
}