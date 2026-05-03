// ─────────────────────────────────────────────────────────────────
// EnergyManager.cs
// Attach to: GameManager (empty GameObject)
// Role: Central controller for all energy logic
// ─────────────────────────────────────────────────────────────────
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    // ── Singleton so other scripts can find us easily ──
    public static EnergyManager Instance;

    // ── Inspector-tunable values ──
    [Header("Battery Settings")]
    public float maxBattery = 100f;       // Max battery capacity
    public float startBattery = 50f;      // Starting charge (50%)

    [Header("Energy Rates (per second)")]
    public float solarChargeRate = 5f;     // How fast panel charges battery
    public float bulbDrainRate = 8f;       // How fast bulb drains battery

    // ── Game State (read by other scripts) ──
    [HideInInspector] public float currentBattery;
    [HideInInspector] public bool isDay = true;
    [HideInInspector] public bool isBulbOn = false;
    [HideInInspector] public bool isGameOver = false;

    // ── Unity Light reference (the actual bulb) ──
    [Header("Scene References")]
    public Light bulbLight;                // Drag the Bulb light here in Inspector

    void Awake()
    {
        // Singleton setup: ensures only one EnergyManager exists
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentBattery = startBattery;    // Set initial battery level
        if (bulbLight != null)
            bulbLight.enabled = false;   // Bulb starts OFF
    }

    void Update()
    {
        // Don't process anything if game is over
        if (isGameOver) return;

        // ── DAYTIME: Solar panel charges the battery ──
        if (isDay)
        {
            currentBattery += solarChargeRate * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
        }

        // ── BULB ON: Drain battery regardless of time of day ──
        if (isBulbOn)
        {
            currentBattery -= bulbDrainRate * Time.deltaTime;
        }

        // ── Sync the actual Unity light with game state ──
        if (bulbLight != null)
            bulbLight.enabled = isBulbOn;

        // ── Check for game over condition ──
        if (currentBattery <= 0f)
        {
            currentBattery = 0f;
            TriggerGameOver();
        }
    }

    // Called when battery hits zero
    public void TriggerGameOver()
    {
        isGameOver = true;
        isBulbOn = false;
        if (bulbLight != null)
            bulbLight.enabled = false;
        Debug.Log("GAME OVER: Battery depleted!");
        // UIController listens for isGameOver and shows the panel
    }

    // Called by RestartButton
    public void RestartGame()
    {
        currentBattery = startBattery;
        isGameOver = false;
        isBulbOn = false;
        isDay = true;
        // Tell DayNightCycle to reset too
        DayNightCycle cycle = FindObjectOfType<DayNightCycle>();
        if (cycle != null) cycle.ResetCycle();
    }
}