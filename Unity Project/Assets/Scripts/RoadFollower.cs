using UnityEngine;

public class RoadDirectionFollower : MonoBehaviour
{
    public float speed = 8f;
    public float rotationSpeed = 5f;
    public float detectionRadius = 5f;

    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Find nearest direction marker
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("RoadDir"))
            {
                Vector3 dir = hit.transform.forward;

                Quaternion targetRot = Quaternion.LookRotation(dir);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime
                );

                break;
            }
        }
    }
}