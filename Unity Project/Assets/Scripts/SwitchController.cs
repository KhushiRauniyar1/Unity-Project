using UnityEngine;
using TMPro;

public class SwitchController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private Bulb bulb;

    [Header("Button Text")]
    [SerializeField] private TMP_Text buttonLabel;

    [Header("Button Colors")]
    [SerializeField] private UnityEngine.UI.Image buttonBackground;
    [SerializeField] private Color onColor  = new Color(0.91f, 0.30f, 0.24f); // Red when ON
    [SerializeField] private Color offColor = new Color(0.18f, 0.80f, 0.44f); // Green when OFF

    private bool isBulbOn = false;

    void Start()
    {
        UpdateButton();
    }

    // Connect this to Button OnClick() in Inspector
    public void OnSwitchPressed()
    {
        isBulbOn = !isBulbOn;

        if (energyManager != null)
            energyManager.ToggleBulb();

        if (bulb != null)
            bulb.SetBulbState(isBulbOn);

        UpdateButton();

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateSwitchUI(isBulbOn);
    }

    private void UpdateButton()
    {
        // Change button text
        if (buttonLabel != null)
            buttonLabel.text = isBulbOn ? "SWITCH OFF" : "SWITCH ON";

        // Change button color
        if (buttonBackground != null)
            buttonBackground.color = isBulbOn ? onColor : offColor;
    }
}