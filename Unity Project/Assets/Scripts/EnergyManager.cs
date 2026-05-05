using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [Header("Battery Settings")]
    public float maxBattery = 100f;
    public float currentBattery = 80f;

    [Header("Energy Rates (per second)")]
    public float solarChargeRate = 5f;
    public float bulbDrainRate = 3f;

    [Header("Bulb Visual")]
    public GameObject bulbGlowObject;

    // Private tracking
    private bool isBulbOn = false;
    private bool isDay = true;
    private bool isGameOver = false;

    void Start()
    {
        // Tell UIManager the starting battery values
        UIManager.Instance.UpdateBatteryUI(currentBattery, maxBattery);
        UIManager.Instance.UpdateTimeUI(true);
        UIManager.Instance.UpdateSwitchUI(false);

        SetBulb(false);
    }

    void Update()
    {
        if (isGameOver) return;

        if (isDay)
            currentBattery += solarChargeRate * Time.deltaTime;

        if (isBulbOn)
            currentBattery -= bulbDrainRate * Time.deltaTime;

        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

        // UIManager handles ALL the display now
        UIManager.Instance.UpdateBatteryUI(currentBattery, maxBattery);

        if (currentBattery <= 0f)
            TriggerGameOver();
    }

    public void SetDayMode(bool dayTime)
    {
        isDay = dayTime;
        UIManager.Instance.UpdateTimeUI(dayTime);
    }

    public void ToggleBulb()
    {
        isBulbOn = !isBulbOn;
        SetBulb(isBulbOn);
        UIManager.Instance.UpdateSwitchUI(isBulbOn);
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
        UIManager.Instance.ShowGameOver();
        SetBulb(false);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}