using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // The object to follow
    public Vector3 offset;        // The offset position of the camera relative to the target
    public float followSpeed = 5f; // The speed at which the camera follows the target

    void Start()
    {
        // Set the initial offset if not set
        if (offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        // Calculate the desired position based on the target position and offset
        Vector3 desiredPosition = target.position + offset;
        
        // Smoothly move the camera towards the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        
        // Optionally, make the camera always look at the target
        transform.LookAt(target);
    }
}
