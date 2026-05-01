// PollutionManager.cs
// Attach to the GameManagers GameObject.
// Receives data from EnergyManager and WasteManager.
// Calculates total pollution. Sends all values to UIManager only.
using UnityEngine;

public class PollutionManager : MonoBehaviour
{
    public static PollutionManager Instance;

    public float totalPollution { get; private set; }
    public float maxPollution   = 500f;

    private float energyDeficit   = 0f;
    private float wastePollution  = 0f;
    private float staticPollution = 0f;

    private UIManager uiManager;

    void Awake() => Instance = this;

    void Start()
    {
        uiManager = Object.FindObjectOfType<UIManager>();
    }

    public void UpdateEnergyDeficit(float netEnergy)
    {
        energyDeficit = Mathf.Max(0f, -netEnergy);
    }

    public void UpdateWaste(float waste)
    {
        wastePollution = waste * 0.05f;
    }

    public void AddStaticPollution(float v)    => staticPollution += v;
    public void RemoveStaticPollution(float v) => staticPollution -= v;

    void Update()
    {
        float delta = (energyDeficit * 0.1f + staticPollution + wastePollution)
                       * Time.deltaTime;
        totalPollution = Mathf.Clamp(totalPollution + delta, 0f, maxPollution);
    }
}