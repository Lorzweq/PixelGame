using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private Vector2 offset;
    [SerializeField] private float smoothTime = 0.15f;

    [Header("Axis Locks")]
    [SerializeField] private bool lockY = false;
    [SerializeField] private bool lockX = false;

    [Header("Limits (optional)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 desired = new Vector3(
            lockX ? transform.position.x : target.position.x + offset.x,
            lockY ? transform.position.y : target.position.y + offset.y,
            transform.position.z
        );

        if (useBounds)
        {
            desired.x = Mathf.Clamp(desired.x, minBounds.x, maxBounds.x);
            desired.y = Mathf.Clamp(desired.y, minBounds.y, maxBounds.y);
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desired,
            ref velocity,
            smoothTime
        );
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
