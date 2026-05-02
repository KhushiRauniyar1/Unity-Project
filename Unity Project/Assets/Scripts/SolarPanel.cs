using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    public float supplyAmount    = 25f;
    public float chargePerSecond =  8f; // battery units added per second in sun

    private GameObject indicator;
    private bool isCharging = false;

    void Start()
    {
        // Indicator ball above solar panel
        indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        indicator.name = "SolarIndicator";
        indicator.transform.SetParent(transform);
        indicator.transform.localPosition = new Vector3(0f, 3f, 0f);
        indicator.transform.localScale    = new Vector3(0.5f, 0.5f, 0.5f);
        Destroy(indicator.GetComponent<Collider>());
        SetIndicatorColor(Color.white);
    }

    void Update()
    {
        if (DayNightCycle.Instance == null) return;

        if (DayNightCycle.Instance.isDay)
        {
            // DAY — charge the battery based on sun intensity
            float charge = chargePerSecond
                           * DayNightCycle.Instance.sunIntensity
                           * Time.deltaTime;
            BatteryManager.Instance?.AddCharge(charge);

            // Show yellow/orange depending on sun strength
            float t = DayNightCycle.Instance.sunIntensity;
            SetIndicatorColor(Color.Lerp(Color.white,
                              new Color(1f,0.85f,0f), t));
            isCharging = true;
        }
        else
        {
            // NIGHT — solar off
            if (isCharging)
            {
                SetIndicatorColor(Color.gray);
                isCharging = false;
            }
        }
    }

    void OnMouseDown()
    {
        ConnectionManager.Instance.SelectSolar(this);
        SetIndicatorColor(Color.yellow);
        Debug.Log("Solar panel selected! Now click a building.");
    }

    public void Deselect()
    {
        SetIndicatorColor(Color.white);
    }

    void SetIndicatorColor(Color c)
    {
        if (indicator != null)
            indicator.GetComponent<Renderer>().material.color = c;
    }
}