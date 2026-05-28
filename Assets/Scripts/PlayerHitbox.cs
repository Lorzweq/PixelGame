using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int minDamage = 6;
    [SerializeField] private int maxDamage = 9;

    [Header("Hitbox Settings")]
    [SerializeField] private CapsuleCollider2D hitboxCollider; // Reference to the ACTUAL collider
    [SerializeField] private LayerMask enemyLayer;

    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private Color activeColor = Color.red;
    [SerializeField] private Color inactiveColor = Color.green;

    private int currentDamage;
    private bool isActive = false;

    void Start()
    {
        // Try to get BoxCollider2D if not set
        if (hitboxCollider == null)
        {
            hitboxCollider = GetComponent<CapsuleCollider2D>();
            if (hitboxCollider == null)
            {
                Debug.LogError($"❌ No BoxCollider2D on {gameObject.name}!");
                return;
            }
        }

        Debug.Log($"✅ PlayerHitbox initialized on {gameObject.name}");
        Debug.Log($"   Collider offset: {hitboxCollider.offset}");
        Debug.Log($"   Collider size: {hitboxCollider.size}");
    }

    public void EnableHitbox()
    {
        // Generate random damage
        currentDamage = Random.Range(minDamage, maxDamage + 1);
        isActive = true;

        Debug.Log($"\n🎯 HITBOX ENABLED");
        Debug.Log($"   Damage: {currentDamage}");
        Debug.Log($"   World Center: {GetHitboxWorldCenter()}");

        // Immediately check for hits
        CheckForHits();
    }

    public void DisableHitbox()
    {
        isActive = false;
        Debug.Log("🔴 Hitbox disabled");
    }

    // Get the ACTUAL world position of the collider (including offset)
    private Vector2 GetHitboxWorldCenter()
    {
        return transform.TransformPoint(hitboxCollider.offset);
    }

    // Get the ACTUAL size of the collider in world space
    private Vector2 GetHitboxWorldSize()
    {
        return hitboxCollider.size * Mathf.Abs(transform.lossyScale.x);
    }

    private void CheckForHits()
    {
        if (hitboxCollider == null) return;

        Vector2 center = GetHitboxWorldCenter();
        Vector2 size = GetHitboxWorldSize();

        Debug.Log($"🔍 Checking hits at: {center}");
        Debug.Log($"   Size: {size}");
        Debug.Log($"   Collider offset: {hitboxCollider.offset}");
        Debug.Log($"   Transform position: {transform.position}");

        // Check ALL colliders first to see what's in the area
        Collider2D[] allColliders = Physics2D.OverlapBoxAll(center, size, 0f);
        Debug.Log($"Found {allColliders.Length} total colliders:");
        foreach (Collider2D col in allColliders)
        {
            if (col.gameObject == gameObject) continue;
            Debug.Log($"   - {col.gameObject.name} (Layer: {LayerMask.LayerToName(col.gameObject.layer)})");
        }

        // Now check only enemies
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, enemyLayer);

        Debug.Log($"Found {hits.Length} enemies in hitbox");

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            Debug.Log($"Hit: {hit.gameObject.name}");

            // Try to get Health component
            Health health = hit.GetComponent<Health>();
            if (health == null)
            {
                health = hit.GetComponentInParent<Health>();
            }

            if (health != null)
            {
                Debug.Log($"💥 Dealing {currentDamage} damage to {hit.gameObject.name}");
                health.TakeDamage(currentDamage);
            }
            else
            {
                Debug.Log($"⚠️ No Health component found on {hit.gameObject.name}");
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        if (hitboxCollider == null) return;

        Vector2 center = GetHitboxWorldCenter();
        Vector2 size = GetHitboxWorldSize();

        Gizmos.color = isActive ? activeColor : inactiveColor;
        Gizmos.DrawWireCube(center, size);

        if (isActive)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(center, 0.05f);

            // Draw line from player to hitbox
            if (transform.parent != null)
            {
                Gizmos.DrawLine(transform.parent.position, center);
            }
        }
    }

    // Editor helper
    [ContextMenu("Test Hitbox Now")]
    void TestHitboxInEditor()
    {
        Debug.Log("\n=== EDITOR TEST ===");
        EnableHitbox();
        Invoke(nameof(DisableHitbox), 0.1f);
    }

    void Update()
    {
        // Debug: Show hitbox position in real-time
        if (isActive && hitboxCollider != null)
        {
            Vector2 center = GetHitboxWorldCenter();
            Vector2 size = GetHitboxWorldSize();

            // Draw debug lines
            Vector2 halfSize = size * 0.5f;
            Vector2 topLeft = center + new Vector2(-halfSize.x, halfSize.y);
            Vector2 topRight = center + new Vector2(halfSize.x, halfSize.y);
            Vector2 bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);
            Vector2 bottomRight = center + new Vector2(halfSize.x, -halfSize.y);

            Debug.DrawLine(topLeft, topRight, Color.red);
            Debug.DrawLine(topRight, bottomRight, Color.red);
            Debug.DrawLine(bottomRight, bottomLeft, Color.red);
            Debug.DrawLine(bottomLeft, topLeft, Color.red);
        }
    }
}