// SolarPanel.cs
using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    public float supplyAmount = 25f;

    private Renderer[] rends;

    void Start()
    {
        rends = GetComponentsInChildren<Renderer>();
    }

    void OnMouseDown()
    {
        // Tell ConnectionManager this panel is selected
        ConnectionManager.Instance.SelectSolar(this);

        // Turn it yellow so player sees it is selected
        foreach (var r in rends)
            foreach (var mat in r.materials)
                mat.color = Color.yellow;

        Debug.Log("Solar panel selected! Now click TestBuilding.");
    }

    public void Deselect()
   {
        foreach (var r in rends)
            foreach (var mat in r.materials)
                mat.color = Color.white;
    }
}
  