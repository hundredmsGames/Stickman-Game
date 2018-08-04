using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothTime = 0.1f;
    public Vector3 offset;

    private void LateUpdate()
    {
        Vector3 velocity = Vector3.zero;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.position = new Vector3(smoothedPosition.x, transform.position.y, -10f);
    }
}
