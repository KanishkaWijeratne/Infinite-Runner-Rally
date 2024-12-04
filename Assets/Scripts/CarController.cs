using UnityEngine;

public class CarController : MonoBehaviour
{  
     public float moveSpeed = 5f;
    public float horizontalSpeed = 3f; // Speed of horizontal movement (left/right)
    public float rotationSpeed = 300f; // Rotation speed for drifting
    public float maxRotationAngle = 30f; // Max rotation angle for drifting
    public float rotationReturnSpeed = 5f; // Speed at which the rotation returns to zero

    private float currentRotation = 0f; // Tracks the current rotation around the Y-axis

    void Update()
    {
        // Get input from the arrow keys
        float horizontal = Input.GetAxis("Horizontal"); // Left and Right arrows
        float vertical = Input.GetAxis("Vertical"); // Up and Down arrows

        // Move the object forward/backward
        Vector3 movement = new Vector3(horizontal * horizontalSpeed, 0, vertical * moveSpeed) * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // Rotate the object based on horizontal movement (simulating drifting)
        if (horizontal != 0)
        {
            // Calculate the new potential rotation
            float rotation = horizontal * rotationSpeed * Time.deltaTime;

            // Update the current rotation and clamp it to the max/min angles
            currentRotation += rotation;
            currentRotation = Mathf.Clamp(currentRotation, -maxRotationAngle, maxRotationAngle);

            // Apply the rotation
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
        else
        {
            // Smoothly return to zero when there's no horizontal input
            if (Mathf.Abs(currentRotation) > 0.1f)
            {
                currentRotation = Mathf.MoveTowards(currentRotation, 0, rotationReturnSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, currentRotation, 0);
            }
            //merger changes test
        }
    }
}
