// ─────────────────────────────────────────────────────────────────
// SwitchController.cs
// Attach to: SwitchButton (the UI Button GameObject)
// Role: Listens for player click, toggles bulb state
// ─────────────────────────────────────────────────────────────────
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text buttonLabel;   // Drag the button's TextMeshPro text here
    public Button switchButton;    // Drag the SwitchButton itself here

    void Start()
    {
        // Register the button click listener
        switchButton.onClick.AddListener(OnSwitchClicked);
        UpdateButtonLabel();
    }

    // Called every time player clicks the switch button
    void OnSwitchClicked()
    {
        // Ignore clicks if game is over
        if (EnergyManager.Instance.isGameOver) return;

        // Toggle the bulb state in EnergyManager
        EnergyManager.Instance.isBulbOn = !EnergyManager.Instance.isBulbOn;

        // Update the button text to show current state
        UpdateButtonLabel();
    }

    void UpdateButtonLabel()
    {
        if (buttonLabel == null) return;

        bool isOn = EnergyManager.Instance.isBulbOn;
        buttonLabel.text = isOn ? "💡 Switch: ON" : "Switch: OFF";
        buttonLabel.color = isOn ?
            new Color(1f, 0.85f, 0.2f) :   // Yellow when ON
            new Color(0.7f, 0.7f, 0.7f);  // Grey when OFF
    }

    // Also refresh label each frame (handles restart state changes)
    void Update()
    {
        UpdateButtonLabel();
    }
}