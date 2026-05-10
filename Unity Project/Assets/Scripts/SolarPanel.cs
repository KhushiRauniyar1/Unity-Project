using UnityEngine;
using TMPro;

public class SolarPanelInspect : MonoBehaviour
{
    // ── Panel Stats ───────────────────────────────────────────────────────
    [Header("Panel Stats")]
    public float maxOutput    = 100f;
    public float currentOutput = 0f;
    public float panelHealth  = 100f;
    public bool  isBroken     = false;

    // ── Floating UI ───────────────────────────────────────────────────────
    [Header("Floating UI — World Space Canvas above panel")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text   outputText;
    [SerializeField] private TMP_Text   healthText;
    [SerializeField] private TMP_Text   statusText;

    // ── Visuals ───────────────────────────────────────────────────────────
    [Header("Visuals")]
    [SerializeField] private Renderer panelRenderer;
    [SerializeField] private Light    panelGlow;
    [SerializeField] private Color    activeColor = new Color(0f,  0.6f, 1f,  1f);
    [SerializeField] private Color    brokenColor = new Color(1f,  0.2f, 0f,  1f);

    // ── Repair ────────────────────────────────────────────────────────────
    [Header("Repair Settings")]
    [SerializeField] private float repairDuration = 3f; // renamed to fix warning

    // ── Private ───────────────────────────────────────────────────────────
    private float repairProgress = 0f;
    private bool  isRepairing   = false;
    private bool  isInspecting  = false;

    // ── Start ─────────────────────────────────────────────────────────────
    void Start()
    {
        // hide floating UI at start
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    // ── Update ────────────────────────────────────────────────────────────
    void Update()
    {
        UpdateOutput();
        UpdateVisuals();
        UpdateFloatingUI();
        HandleRepair();
    }

    // ── Power output calculation ───────────────────────────────────────────
    void UpdateOutput()
    {
        if (isBroken)
        {
            currentOutput = 0f;
            return;
        }

        // get solar multiplier from DayNightCycle (0 at night, 1 at noon)
        float dayMult = 1f;
        if (DayNightCycle.Instance != null)
            dayMult = DayNightCycle.Instance.GetSolarMultiplier();

        // output depends on health and time of day
        currentOutput = maxOutput * (panelHealth / 100f) * dayMult;

        // send power to EnergyManager every frame
        if (EnergyManager.Instance != null)
            EnergyManager.Instance.AddPower(
                currentOutput * Time.deltaTime * 0.01f);
    }

    // ── Panel color and glow ───────────────────────────────────────────────
    void UpdateVisuals()
    {
        if (panelRenderer == null) return;

        Color c = isBroken ? brokenColor : activeColor;
        panelRenderer.material.color = c;

        if (panelGlow != null)
        {
            panelGlow.color = c;
            panelGlow.intensity = isBroken
                ? 0f
                : (currentOutput / maxOutput) * 2f;
        }
    }

    // ── Floating UI above panel ────────────────────────────────────────────
    void UpdateFloatingUI()
    {
        if (!isInspecting) return;

        // output in watts
        if (outputText != null)
            outputText.text = Mathf.RoundToInt(currentOutput) + "W";

        // health percentage
        if (healthText != null)
            healthText.text = "Health: " +
                Mathf.RoundToInt(panelHealth) + "%";

        // status with color
        if (statusText != null)
        {
            if (isBroken)
            {
                statusText.text  = "BROKEN";
                statusText.color = new Color(1f, 0.2f, 0f); // red
            }
            else
            {
                statusText.text  = "ACTIVE";
                statusText.color = new Color(0f, 1f, 0.4f); // green
            }
        }
    }

    // ── Called by InteractController when player looks at this panel ───────
    public void StartInspect()
    {
        isInspecting = true;

        if (infoPanel != null)
            infoPanel.SetActive(true);
    }

    // ── Called by InteractController when player looks away ────────────────
    public void StopInspect()
    {
        isInspecting   = false;
        isRepairing    = false;
        repairProgress = 0f;

        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    // ── Called by InteractController when player presses E ─────────────────
    public void StartRepair()
    {
        if (!isBroken) return;
        isRepairing = true;
    }

    // ── Hold E to repair — progress fills over repairDuration seconds ──────
    void HandleRepair()
    {
        if (!isRepairing) return;

        if (Input.GetKey(KeyCode.E))
        {
            repairProgress += Time.deltaTime;

            // health fills as repair progresses
            panelHealth = Mathf.Lerp(
                0f, 100f, repairProgress / repairDuration);

            // fully repaired
            if (repairProgress >= repairDuration)
            {
                isBroken       = false;
                panelHealth    = 100f;
                isRepairing    = false;
                repairProgress = 0f;

                Debug.Log("Panel fully repaired!");
            }
        }
        else
        {
            // player released E — stop repair
            isRepairing    = false;
            repairProgress = 0f;
        }
    }

    // ── Public getters for InteractController ──────────────────────────────
    public float GetOutput()
    {
        return currentOutput;
    }

    public bool IsBroken()
    {
        return isBroken;
    }

    // ── Break this panel (can call from Inspector test) ────────────────────
    public void BreakPanel()
    {
        isBroken    = true;
        panelHealth = 0f;
    }
}