using UnityEngine;
using TMPro;

public class SwitchController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnergyManager energyManager;

    [Header("ALL Bulbs and Street Lights — drag every one here")]
    [SerializeField] private Bulb[] allBulbs;

    [Header("Button")]
    [SerializeField] private TMP_Text      buttonLabel;
    [SerializeField] private UnityEngine.UI.Image buttonBackground;

    [Header("Colors")]
    [SerializeField] private Color onColor  = new Color(0.64f, 0.18f, 0.18f);
    [SerializeField] private Color offColor = new Color(0.18f, 0.49f, 0.20f);

    private bool isBulbOn = false;

    void Start()
    {
        UpdateButton();
    }

    public void OnSwitchPressed()
    {
        isBulbOn = !isBulbOn;

        // toggle every single bulb and street light
        foreach (Bulb b in allBulbs)
        {
            if (b != null)
                b.SetBulbState(isBulbOn);
        }

        // tell energy manager
        if (energyManager != null)
            energyManager.ToggleBulb();

        UpdateButton();

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateSwitchUI(isBulbOn);
    }

    private void UpdateButton()
    {
        if (buttonLabel != null)
            buttonLabel.text = isBulbOn
                ? "SWITCH OFF"
                : "SWITCH ON";

        if (buttonBackground != null)
            buttonBackground.color = isBulbOn
                ? onColor
                : offColor;
    }
}