// CameraController.cs
// Attach to the Main Camera.
// WASD / Arrow keys to pan. Scroll wheel to zoom.
// Does not communicate with any other script.

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float zoomSpeed = 5f;
    public float minZoom = 10f;
    public float maxZoom = 60f;

    void Update()
    {
        // Pan left / right / forward / backward
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Translate(
            new Vector3(h, 0, v) * panSpeed * Time.deltaTime,
            Space.World
        );

        // Zoom by moving the camera up or down
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        float newY = transform.position.y - scroll * zoomSpeed * 10f;
        newY = Mathf.Clamp(newY, minZoom, maxZoom);

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }
}