using UnityEngine;
 
public class Building : MonoBehaviour
{
    public float energyDemand   = 10f;
    public float wastePerSecond =  1f;
 
    private bool       isPowered = false;
    private Light      streetLight;
    private GameObject lightPole;
    private GameObject lightBulb;
 
    void Start()
    {
        // Do NOT create anything at start
        // Pole + bulb only appear when solar connects
    }
 
    void Update()
    {
        if (!isPowered) return;
        if (DayNightCycle.Instance == null) return;
 
        // Night = bulb ON and glowing
        // Day   = bulb OFF (dim)
        if (DayNightCycle.Instance.isNight)
        {
            if (streetLight != null)
                streetLight.enabled = true;
            if (lightBulb != null)
                lightBulb.GetComponent<Renderer>().material
                    .SetColor("_EmissionColor",
                        new Color(1f, 0.88f, 0.3f) * 3f);
        }
        else
        {
            if (streetLight != null)
                streetLight.enabled = false;
            if (lightBulb != null)
                lightBulb.GetComponent<Renderer>().material
                    .SetColor("_EmissionColor", Color.black);
        }
    }
 
    void OnMouseDown()
    {
        if (ConnectionManager.Instance == null) return;
        if (ConnectionManager.Instance.selectedSolar != null)
            ConnectionManager.Instance.ConnectToBuilding(this);
        else
            Debug.Log("Click a solar panel first.");
    }
 
    public void SetPowered(bool powered)
    {
        isPowered = powered;
        if (powered)
        {
            CreateStreetLight();
            Debug.Log("Solar connected! Street light will glow at night.");
        }
    }
 
    void CreateStreetLight()
    {
        // ── 1. POLE BASE ─────────────────────────────
        // Thick base at bottom of pole
        GameObject poleBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        poleBase.name = "PoleBase";
        poleBase.transform.SetParent(transform);
        poleBase.transform.localPosition = new Vector3(1.5f, 0.3f, 0f);
        poleBase.transform.localScale    = new Vector3(0.3f, 0.3f, 0.3f);
        Destroy(poleBase.GetComponent<Collider>());
        poleBase.GetComponent<Renderer>().material.color
            = new Color(0.2f, 0.2f, 0.2f); // dark gray
 
        // ── 2. MAIN POLE ─────────────────────────────
        lightPole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        lightPole.name = "LightPole";
        lightPole.transform.SetParent(transform);
        lightPole.transform.localPosition = new Vector3(1.5f, 4f, 0f);
        lightPole.transform.localScale    = new Vector3(0.12f, 4f, 0.12f);
        Destroy(lightPole.GetComponent<Collider>());
        lightPole.GetComponent<Renderer>().material.color
            = new Color(0.25f, 0.25f, 0.25f); // dark gray
 
        // ── 3. HORIZONTAL ARM ────────────────────────
        // The arm sticking out at the top of the pole
        GameObject arm = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        arm.name = "PoleArm";
        arm.transform.SetParent(transform);
        arm.transform.localPosition = new Vector3(1.0f, 8.1f, 0f);
        arm.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        arm.transform.localScale    = new Vector3(0.1f, 0.6f, 0.1f);
        Destroy(arm.GetComponent<Collider>());
        arm.GetComponent<Renderer>().material.color
            = new Color(0.25f, 0.25f, 0.25f);
 
        // ── 4. LAMP HOOD (flattened sphere on top) ───
        GameObject hood = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hood.name = "LampHood";
        hood.transform.SetParent(transform);
        hood.transform.localPosition = new Vector3(0.4f, 8.0f, 0f);
        hood.transform.localScale    = new Vector3(0.5f, 0.25f, 0.5f);
        Destroy(hood.GetComponent<Collider>());
        hood.GetComponent<Renderer>().material.color
            = new Color(0.2f, 0.2f, 0.2f); // dark hood
 
        // ── 5. BULB SPHERE (the glowing part) ────────
        lightBulb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        lightBulb.name = "LightBulb";
        lightBulb.transform.SetParent(transform);
        lightBulb.transform.localPosition = new Vector3(0.4f, 7.7f, 0f);
        lightBulb.transform.localScale    = new Vector3(0.35f, 0.35f, 0.35f);
        Destroy(lightBulb.GetComponent<Collider>());
 
        // Use Standard shader with emission so bulb actually glows
        Material bulbMat = new Material(Shader.Find("Standard"));
        bulbMat.color = new Color(1f, 0.95f, 0.6f);
        bulbMat.EnableKeyword("_EMISSION");
        bulbMat.SetColor("_EmissionColor", Color.black); // OFF at start
        lightBulb.GetComponent<Renderer>().material = bulbMat;
 
        // ── 6. POINT LIGHT inside the bulb ───────────
        GameObject lightObj = new GameObject("StreetLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = new Vector3(0.4f, 7.7f, 0f);
 
        streetLight           = lightObj.AddComponent<Light>();
        streetLight.type      = LightType.Point;
        streetLight.color     = new Color(1f, 0.88f, 0.45f);
        streetLight.intensity = 3f;
        streetLight.range     = 15f;
        streetLight.enabled   = false; // OFF until night
    }
}
 