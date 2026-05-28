using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer blinkLine;

    public float speed = 5f;
    public float jumpForce = 12f;

    [Header("Demon")]
    [SerializeField] private float demonJumpMultiplier = 1.35f;

    [Header("Blink / Teleport")]
    [SerializeField] private float blinkDistance = 3f;          // "3 characters away" (tweak)
    [SerializeField] private float blinkCooldown = 0.6f;
    [SerializeField] private LayerMask blinkBlockLayers;        // set to Ground + Walls etc
    [SerializeField] private float blinkCastRadius = 0.2f;       // approx player radius (tweak)

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.12f;
    [SerializeField] private LayerMask groundLayer;

    private bool grounded;
    private bool isDemon;

    private float nextBlinkTime;
    private int facingDir = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!anim) anim = GetComponentInChildren<Animator>();
    }

    public void SetDemon(bool demon)
    {
        isDemon = demon;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");

        // Grounded check
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // Remember facing direction
        if (x != 0) facingDir = x > 0 ? 1 : -1;

        // BLINK (G)
        if (Input.GetKeyDown(KeyCode.G) && Time.time >= nextBlinkTime)
        {
            nextBlinkTime = Time.time + blinkCooldown;

            // Play animation (use your Dash trigger or rename it to Blink if you want)
            anim.SetTrigger("Dash");

            Vector2 start = rb.position;
            Vector2 dir = Vector2.right * facingDir;

            // Cast from start towards target to prevent blinking into walls
            RaycastHit2D hit = Physics2D.CircleCast(
                start,
                blinkCastRadius,
                dir,
                blinkDistance,
                blinkBlockLayers
            );

            Vector2 target;

            if (hit.collider != null)
            {
                // Stop just before the obstacle
                float safeDist = Mathf.Max(0f, hit.distance - 0.05f);
                target = start + dir * safeDist;
            }
            else
            {
                target = start + dir * blinkDistance;
            }

            Vector2 startPos = rb.position;
            rb.position = target;
            rb.linearVelocity = Vector2.zero; // optional: prevents carrying momentum after blink
            PlayBlinkLine(startPos, target);
        }

        // Normal Move
        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            float jf = isDemon ? jumpForce * demonJumpMultiplier : jumpForce;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jf);
        }

        // Animator params
        anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("Grounded", grounded);

        // Facing
        GetComponent<PlayerFacing>().SetFacingFromMove(x);
    }

    private void PlayBlinkLine(Vector2 from, Vector2 to)
    {
        StopCoroutine(nameof(BlinkLineRoutine));
        StartCoroutine(BlinkLineRoutine(from, to));
    }

    private System.Collections.IEnumerator BlinkLineRoutine(Vector2 from, Vector2 to)
    {
        blinkLine.enabled = true;
        blinkLine.SetPosition(0, from);
        blinkLine.SetPosition(1, to);

        yield return new WaitForSeconds(0.08f);

        blinkLine.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
