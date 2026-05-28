using UnityEngine;

public class SimpleAttackTest : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private KeyCode attackKey = KeyCode.Space;
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private float attackCooldown = 0.5f;

    [Header("Hitbox Settings")]
    [SerializeField] private Transform hitboxPosition; // Empty GameObject to position hitbox
    [SerializeField] private Vector2 hitboxSize = new Vector2(1f, 1f);
    [SerializeField] private LayerMask enemyLayer;

    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private Color attackColor = Color.red;
    [SerializeField] private Color readyColor = Color.green;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float cooldownTimer = 0f;

    void Update()
    {
        // Update timers
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                EndAttack();
            }
        }

        // Start attack
        if (Input.GetKeyDown(attackKey) && cooldownTimer <= 0 && !isAttacking)
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
        Debug.Log("⚔️ ATTACK STARTED!");

        isAttacking = true;
        attackTimer = attackDuration;
        cooldownTimer = attackCooldown;

        // Check for hits immediately
        CheckForHits();
    }

    void EndAttack()
    {
        Debug.Log("🔴 ATTACK ENDED");
        isAttacking = false;
    }

    void CheckForHits()
    {
        Vector2 checkPosition;

        if (hitboxPosition != null)
        {
            checkPosition = hitboxPosition.position;
        }
        else
        {
            checkPosition = transform.position + transform.right * 1f; // 1 unit in front
        }

        Debug.Log($"🔍 Checking for enemies at position: {checkPosition}");

        // Box overlap check
        Collider2D[] hits = Physics2D.OverlapBoxAll(checkPosition, hitboxSize, 0f, enemyLayer);

        Debug.Log($"Found {hits.Length} enemies in hitbox");

        foreach (Collider2D hit in hits)
        {
            Debug.Log($"Hit: {hit.gameObject.name}");

            // Try Health component
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                int damage = Random.Range(6, 10); // 6-9 damage
                Debug.Log($"💥 Dealing {damage} damage to {hit.gameObject.name}");
                health.TakeDamage(damage);
                continue;
            }

           

            Debug.Log($"⚠️ No damageable component on {hit.gameObject.name}");
        }
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Vector2 checkPosition;

        if (hitboxPosition != null)
        {
            checkPosition = hitboxPosition.position;
        }
        else
        {
            checkPosition = (Vector2)transform.position + Vector2.right * 1f;
        }

        // Draw hitbox area
        Gizmos.color = isAttacking ? attackColor : readyColor;
        Gizmos.DrawWireCube(checkPosition, hitboxSize);

        // Draw attack direction
        if (!isAttacking)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, checkPosition);
        }
    }

    // Optional: Add this to create a hitbox position helper
    void Reset()
    {
        if (hitboxPosition == null)
        {
            GameObject hitboxObj = new GameObject("HitboxPosition");
            hitboxObj.transform.SetParent(transform);
            hitboxObj.transform.localPosition = new Vector3(1f, 0f, 0f);
            hitboxPosition = hitboxObj.transform;
            Debug.Log("Created hitbox position helper");
        }
    }
}