using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // ── Singleton ────────────────────────────────────────────────────────
    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // ── Inspector Fields ─────────────────────────────────────────────────
    [Header("Battery UI")]
    [SerializeField] private Slider batterySlider;
    [SerializeField] private TMP_Text batteryText;

    [Header("Status UI")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text switchStatusText;
    [SerializeField] private TMP_Text dayCounterText;

    [Header("Panels")]
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Canvas")]
    [SerializeField] private Canvas UICanvas;

    // ── Start ────────────────────────────────────────────────────────────
    void Start()
    {
        if (warningPanel)  warningPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    // ── Called by EnergyManager every frame ──────────────────────────────
    public void UpdateBatteryUI(float current, float max)
    {
        if (batterySlider)
            batterySlider.value = current;

        if (batteryText)
        {
            int pct = Mathf.RoundToInt((current / max) * 100);
            batteryText.text = pct + "%";
        }

        if (warningPanel)
            warningPanel.SetActive((current / max) * 100f < 20f);

        UpdateBatteryColor(current, max);
    }

    // ── Called by DayNightCycle ──────────────────────────────────────────
    public void UpdateTimeUI(bool isDay)
    {
        if (statusText)
            statusText.text = isDay ? "DAY" : "NIGHT";
    }

    // ── Called by SwitchController ───────────────────────────────────────
    public void UpdateSwitchUI(bool isBulbOn)
    {
        if (switchStatusText)
            switchStatusText.text = isBulbOn ? "Bulb: ON" : "Bulb: OFF";
    }

    // ── Called by DayNightCycle on each new day ───────────────────────────
    public void UpdateDayCounter(int day)
    {
        if (dayCounterText)
            dayCounterText.text = "Day " + day;
    }

    // ── Called by EnergyManager on game over ─────────────────────────────
    public void ShowGameOver()
    {
        if (gameOverPanel)
            gameOverPanel.SetActive(true);
    }

    // ── Toggle battery text visibility ───────────────────────────────────
    public void ToggleText()
    {
        if (batteryText)
        {
            bool current = batteryText.gameObject.activeSelf;
            batteryText.gameObject.SetActive(!current);
        }
    }

    // ── Toggle whole canvas ───────────────────────────────────────────────
    public void ToggleUI()
    {
        if (UICanvas)
        {
            bool current = UICanvas.gameObject.activeSelf;
            UICanvas.gameObject.SetActive(!current);
        }
    }

    // ── Battery bar color: green → orange → red ──────────────────────────
    private void UpdateBatteryColor(float current, float max)
    {
        if (batterySlider == null) return;

        Image fillImage = batterySlider.fillRect.GetComponent<Image>();
        if (fillImage == null) return;

        float pct = (current / max) * 100f;

        if (pct > 50f)
            fillImage.color = new Color(0.15f, 0.68f, 0.38f); // Green
        else if (pct > 20f)
            fillImage.color = new Color(0.95f, 0.62f, 0.07f); // Orange
        else
            fillImage.color = new Color(0.91f, 0.30f, 0.24f); // Red
    }
}