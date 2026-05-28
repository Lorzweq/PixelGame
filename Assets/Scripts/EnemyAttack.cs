using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;              // drag Player here
    public float attackRange = 1.2f;

    public float cooldown = 1.2f;
    public float windup = 0.2f;
    public float activeTime = 0.12f;

    public EnemyHitbox hitbox;

    float nextTime;

    void Awake()
    {
        if (hitbox == null)
            hitbox = GetComponentInChildren<EnemyHitbox>(true);
    }

    void Update()
    {
        if (target == null || hitbox == null) return;
        if (Time.time < nextTime) return;

        float dist = Vector2.Distance(transform.position, target.position);
        if (dist <= attackRange)
        {
            nextTime = Time.time + cooldown;
            Invoke(nameof(DoSwing), windup);
        }
    }

    void DoSwing()
    {
        hitbox.Activate(activeTime);
    }
}
