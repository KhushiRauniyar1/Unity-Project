using UnityEngine;

public class BuildingLamp : MonoBehaviour
{
    // ── Drag your lamp prefab here in the Inspector ──
    public GameObject lampPrefab;

    // ── How high above the building the lamp sits ────
    public float lampHeight = 8f;

    // ── Private ──────────────────────────────────────
    private GameObject spawnedLamp;
    private Light      lampLight;
    private bool       isPowered = false;

    // ── Every frame ──────────────────────────────────
    void Update()
    {
        if (DayNightCycle.Instance == null) return;
        if (!isPowered) return;
        if (lampLight == null) return;

        // Night + solar connected = lamp ON
        // Day  + solar connected = lamp OFF
        lampLight.enabled = DayNightCycle.Instance.isNight;
    }

    // ── Called by ConnectionManager when solar connects
    public void SetPowered(bool powered)
    {
        isPowered = powered;

        if (powered)
        {
            SpawnLamp();
            Debug.Log("Lamp spawned! Will light up at night.");
        }
        else
        {
            if (spawnedLamp != null)
                Destroy(spawnedLamp);
            lampLight = null;
        }
    }

    // ── Spawns the lamp asset on the building roof ───
    void SpawnLamp()
    {
        if (lampPrefab == null)
        {
            Debug.LogError("lampPrefab is empty! Drag the lamp prefab into the Inspector slot.");
            return;
        }

        // Destroy old lamp if already exists
        if (spawnedLamp != null)
            Destroy(spawnedLamp);

        // Spawn the lamp above the building
        Vector3 spawnPos = transform.position + Vector3.up * lampHeight;
        spawnedLamp = Instantiate(lampPrefab, spawnPos, Quaternion.identity);

        // Make it a child of this building so it moves with it
        spawnedLamp.transform.SetParent(transform);

        // Find the Light component inside the lamp prefab
        lampLight = spawnedLamp.GetComponentInChildren<Light>();

        if (lampLight == null)
        {
            // No light found inside prefab — create one manually
            Debug.Log("No Light found in lamp prefab — adding one manually.");
            GameObject lightObj = new GameObject("AddedLight");
            lightObj.transform.SetParent(spawnedLamp.transform);
            lightObj.transform.localPosition = Vector3.zero;

            lampLight           = lightObj.AddComponent<Light>();
            lampLight.type      = LightType.Point;
            lampLight.color     = new Color(1f, 0.88f, 0.45f);
            lampLight.intensity = 4f;
            lampLight.range     = 12f;
        }

        // Start with light OFF (day time)
        lampLight.enabled = false;
    }
}