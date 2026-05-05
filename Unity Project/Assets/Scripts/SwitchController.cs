using UnityEngine;
using TMPro;

public class SwitchController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnergyManager energyManager;

    [Header("All Bulbs and Street Lights")]
    [SerializeField] private Bulb[] allBulbs; // drag ALL bulb objects here

    [Header("Button")]
    [SerializeField] private TMP_Text buttonLabel;
    [SerializeField] private UnityEngine.UI.Image buttonBackground;

    [Header("Colors")]
    [SerializeField] private Color onColor  = new Color(0.64f, 0.18f, 0.18f); // red
    [SerializeField] private Color offColor = new Color(0.18f, 0.49f, 0.20f); // green

    private bool isBulbOn = false;

    void Start()
    {
        UpdateButton();
    }

    // connect to Button OnClick in Inspector
    public void OnSwitchPressed()
    {
        isBulbOn = !isBulbOn;

        // tell EnergyManager
        if (energyManager != null)
            energyManager.ToggleBulb();

        // turn ON or OFF every single bulb and street light
        foreach (Bulb b in allBulbs)
        {
            if (b != null)
                b.SetBulbState(isBulbOn);
        }

        UpdateButton();

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateSwitchUI(isBulbOn);
    }

    private void UpdateButton()
    {
        if (buttonLabel != null)
            buttonLabel.text = isBulbOn ? "SWITCH OFF" : "SWITCH ON";

        if (buttonBackground != null)
            buttonBackground.color = isBulbOn ? onColor : offColor;
    }
}