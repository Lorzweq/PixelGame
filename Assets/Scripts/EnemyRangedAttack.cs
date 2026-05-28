using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform target;          // drag Player here (recommended)
    [SerializeField] private Transform firePoint;       // empty child where fireball spawns
    [SerializeField] private FireballProjectile fireballPrefab; // <-- not GameObject


    [Header("Ranges")]
    public float aggroRange = 8f;
    public float minRange = 2f;      // don't throw if too close (optional)

    [Header("Timing")]
    public float windup = 0.25f;     // small telegraph (souls-like)
    public float cooldown = 1.2f;

    [Header("Fireball stats")]
    public int damage = 12;
    public float speed = 7f;
    public LayerMask playerLayer;

    private float nextShotTime;
    private bool isShooting;

    void Awake()
    {
        // If you prefer auto-find:
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }

        if (firePoint == null)
            firePoint = transform; // fallback (but better to set a real firePoint)
    }

    void Update()
    {
        if (!enabled) {return; }

        if (target == null) {return; }
        if (fireballPrefab == null) {  return; }
        if (firePoint == null) {  return; }

        if (Time.time < nextShotTime) {  return; }
        if (isShooting) {  return; }

        float dist = Vector2.Distance(transform.position, target.position);
        

        if (dist <= aggroRange && dist >= minRange)
        {
            
            isShooting = true;
            nextShotTime = Time.time + cooldown;
            Invoke(nameof(ShootNow), windup);
        }
    }


    void ShootNow()
    {
        if (target == null) { isShooting = false; return; }

        Vector2 dir = ((Vector2)target.position - (Vector2)firePoint.position).normalized;

        float spawnOffset = 0.25f; // tune
        Vector3 spawnPos = firePoint.position + (Vector3)(dir * spawnOffset);

        var proj = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        proj.damage = damage;
        proj.speed = speed;
        proj.playerLayer = playerLayer;
        proj.Launch(dir);

        isShooting = false;
    }


}
