using UnityEngine;

public class KeepChildStatic : MonoBehaviour
{
    private Transform parentTransform;
    private Vector3 initialRelativePosition;

    void Start()
    {
        // Store the initial relative position of the child to the parent.
        parentTransform = transform.parent;
        initialRelativePosition = transform.localPosition;
    }

    void Update()
    {
        // Update the child's position based on the initial relative position.
        transform.position = parentTransform.position + parentTransform.rotation * initialRelativePosition;
    }
}
