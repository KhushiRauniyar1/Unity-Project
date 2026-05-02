using UnityEngine;

public class Building : MonoBehaviour
{
    // --- Inspector values ---
    public float energyDemand   = 10f;
    public float wastePerSecond =  1f;

    // --- Private ---
    private bool       isPowered = false;
    private GameObject indicator;       // coloured ball above building

    // ─────────────────────────────────────
    void Start()
    {
        // Start with RED ball = no power
        SetPowered(false);
    }

    // ─────────────────────────────────────
    // Player clicks TestBuilding
    void OnMouseDown()
    {
        if (ConnectionManager.Instance == null) return;

        if (ConnectionManager.Instance.selectedSolar != null)
        {
            // A solar panel is selected — connect it
            ConnectionManager.Instance.ConnectToBuilding(this);
        }
        else
        {
            Debug.Log("Select a solar panel first, then click TestBuilding.");
        }
    }

    // ─────────────────────────────────────
    // Called by ConnectionManager
    // powered = true  → GREEN ball (solar connected)
    // powered = false → RED   ball (no power)
    public void SetPowered(bool powered)
    {
        isPowered = powered;

        // Create the indicator ball if it doesn't exist yet
        if (indicator == null)
        {
            indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            indicator.name = "PowerIndicator";
            indicator.transform.SetParent(transform);

            // Float above the building
            indicator.transform.localPosition = new Vector3(0f, 5f, 0f);
            indicator.transform.localScale    = new Vector3(0.6f, 0.6f, 0.6f);

            // Remove the collider so clicks go through to the building
            Destroy(indicator.GetComponent<Collider>());
        }

        // RED = no power     GREEN = solar connected
        indicator.GetComponent<Renderer>().material.color =
            powered ? Color.green : Color.red;
    }
}