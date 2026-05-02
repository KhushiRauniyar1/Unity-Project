using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    // How much energy this panel supplies
    public float supplyAmount = 25f;

    // Small indicator above the solar panel
    private GameObject indicator;

    // ─────────────────────────────────────
    void Start()
    {
        // Create a YELLOW ball above the solar panel
        indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        indicator.name = "SolarIndicator";
        indicator.transform.SetParent(transform);
        indicator.transform.localPosition = new Vector3(0f, 3f, 0f);
        indicator.transform.localScale    = new Vector3(0.5f, 0.5f, 0.5f);
        Destroy(indicator.GetComponent<Collider>());

        // Start as white = idle
        indicator.GetComponent<Renderer>().material.color = Color.white;
    }

    // ─────────────────────────────────────
    // Player clicks the solar panel
    void OnMouseDown()
    {
        ConnectionManager.Instance.SelectSolar(this);

        // Turn ball YELLOW = selected and ready to connect
        indicator.GetComponent<Renderer>().material.color = Color.yellow;

        Debug.Log("Solar panel selected! Now click TestBuilding.");
    }

    // ─────────────────────────────────────
    // Called by ConnectionManager after connecting
    public void Deselect()
    {
        // Turn back to white after connecting
        indicator.GetComponent<Renderer>().material.color = Color.white;
    }
}