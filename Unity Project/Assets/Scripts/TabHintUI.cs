using UnityEngine;
using TMPro;

public class TabHintUI : MonoBehaviour
{
    [Header("Text Reference")]
    public TMP_Text hintText;

    void Start()
    {
        // Force show text immediately on start
        if (hintText == null)
        {
            Debug.LogError("TabHintUI: Drag your TMP Text into the Hint Text slot!");
            return;
        }

        hintText.text = "TAB — Switch to UI Mode";
        hintText.gameObject.SetActive(true);
        Debug.Log("TabHintUI working");
    }

    void Update()
    {
        if (hintText == null) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                hintText.text = "TAB — Back to Game";
            else
                hintText.text = "TAB — Switch to UI Mode";
        }
    }
}