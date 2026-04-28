using UnityEngine;

/// <summary>
/// Attach this to ANY building model from SimplePoly City
/// It will track energy consumption and change material color
/// </summary>
public class Building : MonoBehaviour
{
    [Header("Energy Settings")]
    [Tooltip("Energy this building needs per second")]
    public float energyDemand = 1f;
    
    [Header("Visual Feedback")]
    public Color poweredColor = new Color(0.8f, 1f, 0.8f); // Light green tint
    public Color noPowerColor = new Color(1f, 0.3f, 0.3f); // Red tint
    
    private Renderer[] allRenderers; // Buildings have multiple mesh parts
    private Material[] originalMaterials;
    private Material[] runtimeMaterials;
    
    void Start()
    {
        // Get ALL renderers in this building (including children)
        allRenderers = GetComponentsInChildren<Renderer>();
        
        // Store original materials and create runtime copies
        int totalMaterials = 0;
        foreach (Renderer r in allRenderers)
        {
            totalMaterials += r.materials.Length;
        }
        
        originalMaterials = new Material[totalMaterials];
        runtimeMaterials = new Material[totalMaterials];
        
        int index = 0;
        foreach (Renderer r in allRenderers)
        {
            Material[] mats = r.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                originalMaterials[index] = mats[i];
                runtimeMaterials[index] = new Material(mats[i]); // Create copy
                index++;
            }
        }
        
        // Apply runtime materials
        index = 0;
        foreach (Renderer r in allRenderers)
        {
            Material[] mats = new Material[r.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = runtimeMaterials[index];
                index++;
            }
            r.materials = mats;
        }
        
        // Register with GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterBuilding(this);
            Debug.Log($"Building registered: {gameObject.name}");
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }
    }
    
    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterBuilding(this);
        }
    }
    
    public float GetEnergyDemand()
    {
        return energyDemand;
    }
    
    public void SetPowerStatus(bool powered)
    {
        Color tint = powered ? poweredColor : noPowerColor;
        
        // Apply color tint to all materials
        foreach (Material mat in runtimeMaterials)
        {
            if (mat != null)
            {
                mat.color = tint;
            }
        }
    }
}
