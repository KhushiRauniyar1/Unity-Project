using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    // ── Singleton ────────────────────────────────────────────────────────
    public static EnergyManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // ── Inspector Fields ─────────────────────────────────────────────────
    [Header("Battery Settings")]
    [SerializeField] private float maxBattery = 100f;
    public float currentBattery = 80f;

    // Public getter so other scripts can READ maxBattery without errors
    public float MaxBattery => maxBattery;

    [Header("Energy Rates (per second)")]
    [SerializeField] private float solarChargeRate = 5f;
    [SerializeField] private float bulbDrainRate = 3f;

    [Header("Bulb Visual")]
    [SerializeField] private GameObject bulbGlowObject;

    // ── Private Tracking ─────────────────────────────────────────────────
    private bool isBulbOn = false;
    private bool isDay = true;
    private bool isGameOver = false;

    // ── Start ────────────────────────────────────────────────────────────
    void Start()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager not found! Attach UIManager script to GameManager.");
            return;
        }

        UIManager.Instance.UpdateBatteryUI(currentBattery, maxBattery);
        UIManager.Instance.UpdateTimeUI(true);
        UIManager.Instance.UpdateSwitchUI(false);

        SetBulb(false);
    }

    // ── Update ───────────────────────────────────────────────────────────
    void Update()
    {
        if (isGameOver) return;

        if (isDay)
            currentBattery += solarChargeRate * Time.deltaTime;

        if (isBulbOn)
            currentBattery -= bulbDrainRate * Time.deltaTime;

        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateBatteryUI(currentBattery, maxBattery);

        if (currentBattery <= 0f)
            TriggerGameOver();
    }

    // ── Called by DayNightCycle ──────────────────────────────────────────
    public void SetDayMode(bool dayTime)
    {
        isDay = dayTime;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateTimeUI(dayTime);
    }

    // ── Called by SwitchController ───────────────────────────────────────
    public void ToggleBulb()
    {
        isBulbOn = !isBulbOn;
        SetBulb(isBulbOn);

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateSwitchUI(isBulbOn);
    }

    // ── Private helpers ──────────────────────────────────────────────────
    private void SetBulb(bool on)
    {
        isBulbOn = on;
        if (bulbGlowObject != null)
            bulbGlowObject.SetActive(on);
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        SetBulb(false);

        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver();

        Time.timeScale = 0;
    }

    // ── Called by Try Again button ───────────────────────────────────────
    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}