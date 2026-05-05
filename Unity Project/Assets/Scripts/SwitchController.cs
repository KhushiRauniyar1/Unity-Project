using UnityEngine;
using UnityEngine.UI;

public class SwitchController : MonoBehaviour
{
    public Bulb bulb;
    public EnergyManager energyManager;
    public Text switchLabel;

    private bool isBulbOn = false;

    void Start()
    {
        UpdateLabel();
    }

    // Connect this to Button's OnClick() event in Inspector
    public void OnSwitchPressed()
    {
        isBulbOn = !isBulbOn;

        energyManager.ToggleBulb();

        UpdateLabel();
        if (bulb != null) bulb.SetBulbState (isBulbOn);    }

    void UpdateLabel()
    {
        if (switchLabel != null)
            switchLabel.text = isBulbOn ? "SWITCH OFF" : "SWITCH ON";
    }
}