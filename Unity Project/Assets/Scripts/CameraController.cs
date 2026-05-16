using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Distance")]
    public float distance = 5f;
    public float height   = 2f;

    [Header("Sensitivity")]
    public float sensitivityX = 2f;
    public float sensitivityY = 1.5f;

    [Header("Clamp")]
    public float minY = -20f;
    public float maxY =  60f;

    [Header("Smoothing")]
    public float smoothSpeed = 12f;

    float _rotX;
    float _rotY;

    void Start()
    {
        LockCursor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                UnlockCursor();
            else
                LockCursor();
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            _rotY += Input.GetAxis("Mouse X") * sensitivityX;
            _rotX -= Input.GetAxis("Mouse Y") * sensitivityY;
            _rotX  = Mathf.Clamp(_rotX, minY, maxY);
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        Quaternion rot    = Quaternion.Euler(_rotX, _rotY, 0f);
        Vector3 targetPos = player.position
                          - rot * Vector3.forward * distance
                          + Vector3.up * height;

        transform.position = Vector3.Lerp(
            transform.position, targetPos,
            smoothSpeed * Time.deltaTime);

        transform.LookAt(
            player.position + Vector3.up * (height * 0.5f));
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }
}