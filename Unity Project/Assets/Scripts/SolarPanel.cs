using UnityEngine;
 
public class SolarPanel : MonoBehaviour
{
    public float supplyAmount = 25f;
 
    private GameObject indicator;
 
    void Start()
    {
        // Small white sphere above solar panel = idle
        indicator = MakeSphere(new Vector3(0f, 2f, 0f), 0.4f, Color.white);
    }
 
    void OnMouseDown()
    {
        ConnectionManager.Instance.SelectSolar(this);
        SetIndicator(Color.yellow); // YELLOW = selected
        Debug.Log("Solar selected! Now click the building.");
    }
 
    public void Deselect()
    {
        SetIndicator(Color.white); // back to white
    }
 
    void SetIndicator(Color c)
    {
        if (indicator != null)
            indicator.GetComponent<Renderer>().material.color = c;
    }
 
    GameObject MakeSphere(Vector3 pos, float size, Color color)
    {
        GameObject s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        s.transform.SetParent(transform);
        s.transform.localPosition = pos;
        s.transform.localScale    = new Vector3(size, size, size);
        Destroy(s.GetComponent<Collider>());
        s.GetComponent<Renderer>().material.color = color;
        return s;
    }
}
