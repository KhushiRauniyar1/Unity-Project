using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────────────────────
    public static EnergyManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // ── Inspector Fields ──────────────────────────────────────────────────
    [Header("Battery Settings")]
    [SerializeField] private float maxBattery = 100f;
    public float currentBattery = 80f;

    // getter works in all Unity versions
    public float MaxBattery
    {
        get { return maxBattery; }
    }

    [Header("Energy Rates (per second)")]
    [SerializeField] private float solarChargeRate = 5f;
    [SerializeField] private float bulbDrainRate   = 3f;

    [Header("Bulb Visual")]
    [SerializeField] private GameObject bulbGlowObject;

    // ── Private ───────────────────────────────────────────────────────────
    private bool isBulbOn   = false;
    private bool isDay      = true;
    private bool isGameOver = false;

    // ── Start ─────────────────────────────────────────────────────────────
    void Start()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateBatteryUI(currentBattery, maxBattery);
            UIManager.Instance.UpdateTimeUI(true);
            UIManager.Instance.UpdateSwitchUI(false);
        }

        SetBulb(false);
    }

    // ── Update ────────────────────────────────────────────────────────────
    void Update()
    {
        if (isGameOver) return;

        // solar charges battery during day
        if (isDay)
            currentBattery += solarChargeRate * Time.deltaTime;

        // bulb drains battery when ON
        if (isBulbOn)
            currentBattery -= bulbDrainRate * Time.deltaTime;

        // clamp between 0 and max
        currentBattery = Mathf.Clamp(
            currentBattery, 0f, maxBattery);

        // update UI
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateBatteryUI(
                currentBattery, maxBattery);

        // check game over
        if (currentBattery <= 0f)
            TriggerGameOver();
    }

    // ── Called by SolarPanelInspect every frame ────────────────────────────
    // This is the method that was MISSING — now added
    public void AddPower(float amount)
    {
        if (isGameOver) return;

        currentBattery = Mathf.Clamp(
            currentBattery + amount, 0f, maxBattery);
    }

    // ── Called by DayNightCycle ────────────────────────────────────────────
    public void SetDayMode(bool dayTime)
    {
        isDay = dayTime;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateTimeUI(dayTime);
    }

    // ── Called by SwitchController or SwitchInteract ───────────────────────
    public void ToggleBulb()
    {
        isBulbOn = !isBulbOn;
        SetBulb(isBulbOn);

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateSwitchUI(isBulbOn);
    }

    // ── Called by ScoreManager and Bulb.cs ────────────────────────────────
    public float GetPowerPercent()
    {
        return (currentBattery / maxBattery) * 100f;
    }

    // ── Turns bulb glow on or off ──────────────────────────────────────────
    private void SetBulb(bool on)
    {
        isBulbOn = on;

        if (bulbGlowObject != null)
            bulbGlowObject.SetActive(on);
    }

    // ── Game Over ──────────────────────────────────────────────────────────
    private void TriggerGameOver()
    {
        isGameOver = true;
        SetBulb(false);

        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver();

        Time.timeScale = 0;
    }

    // ── Called by Try Again button ─────────────────────────────────────────
    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager
                .GetActiveScene().name);
    }
}