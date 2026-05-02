using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance;

    // The solar panel the player has clicked
    public SolarPanel selectedSolar { get; private set; }

    // ─────────────────────────────────────
    void Awake()
    {
        Instance = this;
    }

    // ─────────────────────────────────────
    // Called when player clicks a solar panel
    public void SelectSolar(SolarPanel panel)
    {
        // Deselect previous if any
        if (selectedSolar != null)
            selectedSolar.Deselect();

        selectedSolar = panel;
        Debug.Log("Solar selected. Now click a building.");
    }

    // ─────────────────────────────────────
    // Called when player clicks TestBuilding
    public void ConnectToBuilding(Building building)
    {
        if (selectedSolar == null)
        {
            Debug.Log("No solar panel selected!");
            return;
        }

        // Power ON the building → ball turns GREEN
        building.SetPowered(true);

        // Draw a wire between solar panel and building
        DrawWire(selectedSolar.gameObject, building.gameObject);

        Debug.Log("Connected! TestBuilding is now powered.");

        // Deselect the solar panel after connecting
        selectedSolar.Deselect();
        selectedSolar = null;
    }

    // ─────────────────────────────────────
    // Draws a visible yellow wire between two objects
    void DrawWire(GameObject from, GameObject to)
    {
        GameObject wireObj = new GameObject("Wire_Solar_To_Building");
        LineRenderer lr    = wireObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.startWidth    = 0.1f;
        lr.endWidth      = 0.1f;
        lr.useWorldSpace = true;

        // Start above the solar panel, end above the building
        lr.SetPosition(0, from.transform.position + Vector3.up * 3f);
        lr.SetPosition(1, to.transform.position   + Vector3.up * 5f);

        lr.material   = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = new Color(1f, 0.85f, 0f);   // yellow
        lr.endColor   = new Color(0.2f, 0.9f, 0.2f); // green
    }
}