// Building.cs — paste this complete script
using UnityEngine;

public class Building : MonoBehaviour
{
    public float energyDemand   = 10f;
    public float wastePerSecond =  1f;

    private bool isPowered = false;
    private Renderer[] rends;

    void Start()
    {
        rends = GetComponentsInChildren<Renderer>();

        // Give each building its OWN material copy
        // (stops all buildings changing color together)
        foreach (var r in rends)
        {
            Material[] mats = new Material[r.materials.Length];
            for (int i = 0; i < r.materials.Length; i++)
                mats[i] = new Material(r.materials[i]);
            r.materials = mats;
        }

        SetPowered(false); // Start RED — no power yet
    }

    void OnMouseDown()
    {
        // When player clicks this building,
        // tell ConnectionManager to connect it
     if (ConnectionManager.Instance != null &&
            ConnectionManager.Instance.selectedSolar != null)
        {
            ConnectionManager.Instance.ConnectToBuilding(this);
        }
        else
        {
            Debug.Log("Click a solar panel first, then click this building.");
        }
    }

    public void SetPowered(bool powered)
    {
        isPowered = powered;

        // RED = no power,  GREEN = solar connected
        Color c = powered
            ? new Color(0.47f, 0.80f, 0.23f)  // green
            : new Color(0.94f, 0.30f, 0.30f); // red

        foreach (var r in rends)
            foreach (var mat in r.materials)
                mat.color = c;
    }
}