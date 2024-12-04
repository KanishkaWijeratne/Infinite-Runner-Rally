using UnityEngine;

public class ObjectTie : MonoBehaviour
{
    public Transform targetObject; // The object this one will follow
    public Vector3 offset; // Offset to maintain relative to the target

    void Update()
    {
        if (targetObject != null)
        {
            // Keep the object tied to the target's position and rotation
            transform.position = targetObject.position + offset;
            transform.rotation = targetObject.rotation;
        }
    }
}
