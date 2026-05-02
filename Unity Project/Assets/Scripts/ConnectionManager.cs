using UnityEngine;
 
public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance;
 
    public SolarPanel selectedSolar { get; private set; }
 
    void Awake() => Instance = this;
 
    public void SelectSolar(SolarPanel panel)
    {
        if (selectedSolar != null)
            selectedSolar.Deselect();
        selectedSolar = panel;
    }
 
    public void ConnectToBuilding(Building building)
    {
        if (selectedSolar == null)
        {
            Debug.Log("No solar panel selected!");
            return;
        }
 
        building.SetPowered(true);
        DrawWire(selectedSolar.gameObject, building.gameObject);
 
        Debug.Log("Connected! Street light will glow at night.");
 
        selectedSolar.Deselect();
        selectedSolar = null;
    }
 
    void DrawWire(GameObject from, GameObject to)
    {
        GameObject wireObj = new GameObject("Wire");
        wireObj.transform.SetParent(transform);
 
        LineRenderer lr  = wireObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth    = 0.1f;
        lr.endWidth      = 0.1f;
        lr.useWorldSpace = true;
 
        lr.SetPosition(0, from.transform.position + Vector3.up * 3f);
        lr.SetPosition(1, to.transform.position   + Vector3.up * 5f);
 
        lr.material   = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = new Color(1f,  0.9f, 0.1f);
        lr.endColor   = new Color(0.2f,0.9f, 0.2f);
    }
}