using UnityEngine;
using System.Collections;

public class MoveTie : MonoBehaviour
{
   public float speed = 5f;  // Speed at which the object moves

    void Update()
    {
        // Get horizontal input (A/D or Left/Right Arrow)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Move the object along the x-axis
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
    }
}
