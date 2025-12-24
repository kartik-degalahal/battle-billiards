using UnityEngine;

public class FaceCameraForHealthBar : MonoBehaviour
{
    // Offset to keep the bar above the ball
    public Vector3 offset = new Vector3(0, 1.2f, 0);
    private Transform ballTransform;

    void Start()
    {
        // Get the parent (the ball)
        ballTransform = transform.parent;
    }

    void LateUpdate()
    {
        if (ballTransform != null)
        {
            // 1. Force the position to stay at the ball's position + offset
            transform.position = ballTransform.position + offset;

            // 2. Force the rotation to stay flat/upright
            transform.rotation = Quaternion.identity;
        }
    }
}