using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData e)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }

    public void OnPointerExit(PointerEventData e)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }
}