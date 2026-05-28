using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [Header("Damage")]
    public int minDamage = 6;
    public int maxDamage = 12;

    public LayerMask playerLayer;
    public float knockback = 4f; // optional

    bool active;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Activate(float duration)
    {
        if (active) return;
        active = true;
        gameObject.SetActive(true);
        CancelInvoke(nameof(Deactivate));
        Invoke(nameof(Deactivate), duration);
    }

    void Deactivate()
    {
        active = false;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!active) return;
        if (((1 << other.gameObject.layer) & playerLayer.value) == 0) return;

        var hurtbox = other.GetComponentInParent<PlayerHurtbox>();
        if (hurtbox == null) return;

        int damage = Random.Range(minDamage, maxDamage + 1);
        hurtbox.ApplyDamage(damage);

        // Optional knockback
        var rb = other.GetComponentInParent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 dir = (rb.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            rb.AddForce(dir * knockback, ForceMode2D.Impulse);
        }

        // One hit per swing
        Deactivate();
    }
}
