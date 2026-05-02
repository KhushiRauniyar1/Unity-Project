// ConnectionManager.cs
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance;

    public SolarPanel selectedSolar { get; private set; }

    void Awake() => Instance = this;

    public void SelectSolar(SolarPanel panel)
    {
        // Deselect the previous one if any
        if (selectedSolar != null)
            selectedSolar.Deselect();

        selectedSolar = panel;
        Debug.Log("Solar selected. Now click a building.");
    }

    public void ConnectToBuilding(Building building)
    {
        if (selectedSolar == null) return;

        // Power on the building
        building.SetPowered(true);
     // Draw the wire between solar and building
        DrawWire(selectedSolar.gameObject, building.gameObject);

        Debug.Log("Connected! Building is now GREEN.");

        // Deselect after connecting
        selectedSolar.Deselect();
        selectedSolar = null;
    }

    void DrawWire(GameObject from, GameObject to)
    {
        GameObject wireObj = new GameObject("Wire");
        LineRenderer lr = wireObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.startWidth    = 0.08f;
        lr.endWidth      = 0.08f;
        lr.useWorldSpace = true;

        lr.SetPosition(0, from.transform.position + Vector3.up * 2f);
        lr.SetPosition(1, to.transform.position   + Vector3.up * 2f);

        lr.material        = new Material(Shader.Find("Sprites/Default"));
        lr.startColor      = new Color(1f, 0.85f, 0f);
        lr.endColor        = new Color(1f, 0.5f,  0f);
    }
}   