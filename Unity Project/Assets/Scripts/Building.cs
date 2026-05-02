using UnityEngine;

public class Building : MonoBehaviour
{
    public float energyDemand   = 10f;
    public float wastePerSecond =  1f;

    private bool       isPowered   = false;
    private GameObject indicator;    // power state ball
    private Light      cityLight;    // warm light inside building

    // ─────────────────────────────────────
    void Start()
    {
        SetPowered(false);
        CreateCityLight();
    }

    // ─────────────────────────────────────
    void Update()
    {
        if (DayNightCycle.Instance == null) return;

        if (DayNightCycle.Instance.isNight && isPowered)
        {
            // NIGHT + connected to solar → use battery → lights ON
            bool haspower = BatteryManager.Instance.UseCharge(
                energyDemand * 0.01f * Time.deltaTime);

            cityLight.enabled = haspower;

            // Indicator: green = powered, red = battery dead
            SetIndicatorColor(haspower ? Color.green : Color.red);
        }
        else if (DayNightCycle.Instance.isDay)
        {
            // DAY → turn off city lights (sun is enough)
            cityLight.enabled = false;
        }
        else
        {
            // Night but not connected → dark
            cityLight.enabled = false;
            SetIndicatorColor(Color.red);
        }
    }

    // ─────────────────────────────────────
    void OnMouseDown()
    {
        if (ConnectionManager.Instance == null) return;
        if (ConnectionManager.Instance.selectedSolar != null)
            ConnectionManager.Instance.ConnectToBuilding(this);
        else
            Debug.Log("Select a solar panel first.");
    }

    // ─────────────────────────────────────
    public void SetPowered(bool powered)
    {
        isPowered = powered;

        if (indicator == null)
        {
            indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            indicator.name = "PowerIndicator";
            indicator.transform.SetParent(transform);
            indicator.transform.localPosition = new Vector3(0f, 5f, 0f);
            indicator.transform.localScale    = new Vector3(0.6f, 0.6f, 0.6f);
            Destroy(indicator.GetComponent<Collider>());
        }

        SetIndicatorColor(powered ? Color.green : Color.red);
    }

    // ─────────────────────────────────────
    void CreateCityLight()
    {
        // Create a warm Point Light inside the building
        GameObject lightObj = new GameObject("CityLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = new Vector3(0f, 2f, 0f);

        cityLight = lightObj.AddComponent<Light>();
        cityLight.type      = LightType.Point;
        cityLight.color     = new Color(1f, 0.9f, 0.6f); // warm yellow
        cityLight.intensity = 2f;
        cityLight.range     = 8f;
        cityLight.enabled   = false; // OFF by default (day time)
    }

    // ─────────────────────────────────────
    void SetIndicatorColor(Color c)
    {
        if (indicator != null)
            indicator.GetComponent<Renderer>().material.color = c;
    }
}