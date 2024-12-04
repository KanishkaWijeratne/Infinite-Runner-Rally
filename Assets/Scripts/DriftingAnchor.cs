using UnityEngine;

public class DriftingAnchor : MonoBehaviour
{
    public Transform followPoint; // Point the back "drifts" toward
    public float followDistance = 2f; // Distance the follow point lags behind
    public float driftSpeed = 5f; // Speed of drifting motion
    public float rotationSpeed = 5f; // Speed of rotation adjustment

    private Vector3 velocity = Vector3.zero; // Used for smooth damping

    void Update()
    {
        if (followPoint == null) return;

        // Calculate the target position for the follow point
        Vector3 targetPosition = transform.position - transform.forward * followDistance;

        // Smoothly move the follow point toward the target position
        followPoint.position = Vector3.SmoothDamp(followPoint.position, targetPosition, ref velocity, 1 / driftSpeed);

        // Adjust the rotation of the object to align with the follow point
        Vector3 directionToFollowPoint = followPoint.position - transform.position;

        if (directionToFollowPoint.sqrMagnitude > 0.01f) // Ensure there's a meaningful direction
        {
            Quaternion targetRotation = Quaternion.LookRotation(-directionToFollowPoint);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
