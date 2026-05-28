using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target; // assign Player here (or find by tag below)
    [SerializeField] private string playerTag = "Player";
    [Header("Visual")]
    public bool facesRightByDefault = false; // set false if your sprite faces LEFT in the artwork

    [Header("Behavior")]
    public float aggroRange = 6f;
    public float stopDistance = 1.2f;   // attack range-ish
    public float moveSpeed = 3f;

    [Header("Physics")]
    public bool useSmoothMove = true;
    public float accel = 30f; // only used when smooth move is on

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag(playerTag);
            if (p != null) target = p.transform;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector2 myPos = rb.position;
        Vector2 targetPos = target.position;

        float dist = Vector2.Distance(myPos, targetPos);

        // Not aggro -> stand still
        if (dist > aggroRange)
        {
            Stop();
            return;
        }

        // In range -> stop (so you don’t overlap)
        if (dist <= stopDistance)
        {
            Stop();
            return;
        }

        // Follow
        Vector2 dir = (targetPos - myPos).normalized;
        Vector2 desiredVel = dir * moveSpeed;

        if (useSmoothMove)
        {
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, desiredVel, accel * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = desiredVel;
        }

        // Optional: face direction (flip X)
        if (dir.x != 0f)
        {
            float sign = Mathf.Sign(dir.x);

            // If the art faces left by default, invert the sign
            if (!facesRightByDefault) sign *= -1f;

            Vector3 s = transform.localScale;
            s.x = sign * Mathf.Abs(s.x);
            transform.localScale = s;
        }

    }

    void Stop()
    {
        rb.linearVelocity = Vector2.zero;
    }

    // Handy debug circles in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
