using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    [Header("Battery Settings")]
    public float maxBattery = 100f;
    public float currentBattery = 80f;

    [Header("Energy Rates (per second)")]
    public float solarChargeRate = 5f;   // Solar fills battery this fast
    public float bulbDrainRate = 3f;     // Bulb drains battery this fast

    [Header("UI References")]
    public Slider batterySlider;
    public Text batteryText;
    public Text statusText;
    public GameObject warningPanel;
    public GameObject gameOverPanel;
    public GameObject bulbGlowObject;

    private bool isBulbOn = false;
    private bool isDay = true;
    private bool isGameOver = false;

    void Start()
    {
        batterySlider.maxValue = maxBattery;
        batterySlider.minValue = 0f;

        warningPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        SetBulb(false);
    }

    void Update()
    {
        if (isGameOver) return;

        if (isDay) // Solar charges battery during day
            currentBattery += solarChargeRate * Time.deltaTime;

        if (isBulbOn) // Bulb drains battery when ON
            currentBattery -= bulbDrainRate * Time.deltaTime;

        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

        UpdateUI();

        if (currentBattery <= 0f)
            TriggerGameOver();
    }

    void UpdateUI()
    {
        batterySlider.value = currentBattery;

        int pct = Mathf.RoundToInt((currentBattery / maxBattery) * 100);
        batteryText.text = pct + "%";

        warningPanel.SetActive(pct < 20);

        statusText.text = isDay ? "DAY" : "NIGHT";
    }

    public void SetDayMode(bool dayTime)
    {
        isDay = dayTime;
    }

    public void ToggleBulb()
    {
        isBulbOn = !isBulbOn;
        SetBulb(isBulbOn);
    }

    void SetBulb(bool on)
    {
        isBulbOn = on;

        if (bulbGlowObject != null)
            bulbGlowObject.SetActive(on);
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        gameOverPanel.SetActive(true);

        SetBulb(false);

        Time.timeScale = 0; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}