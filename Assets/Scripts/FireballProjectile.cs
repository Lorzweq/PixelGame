using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float lifeTime = 3f;

    [Header("Damage")]
    public int damage = 12;
    public LayerMask playerLayer;

    [SerializeField] private SpriteRenderer sr;

    [Header("Visual")]
    public bool spriteFacesRightByDefault = true; // set false if your fireball art faces LEFT by default


    private Vector2 dir;
    private float dieTime;
    private bool launched;

    void OnEnable()
    {
        // Prevent instant despawn even if Launch is called a tiny bit later
        dieTime = Time.time + lifeTime;
        launched = false;
        dir = Vector2.zero;
    }

    void Awake()
    {
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
    }


    public void Launch(Vector2 direction)
    {
        dir = direction.normalized;
        launched = true;
        dieTime = Time.time + lifeTime;

        // Flip visuals to match travel direction
        if (sr != null && Mathf.Abs(dir.x) > 0.001f)
        {
            bool goingLeft = dir.x < 0f;

            // If sprite faces RIGHT by default: left => flipX true
            // If sprite faces LEFT by default: invert
            sr.flipX = spriteFacesRightByDefault ? goingLeft : !goingLeft;
        }
    }

    void Update()
    {
        if (launched)
            transform.position += (Vector3)(dir * speed * Time.deltaTime);

        if (Time.time >= dieTime)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only destroy the fireball if we ACTUALLY hit the player hurtbox
        var hurtbox = other.GetComponentInParent<PlayerHurtbox>();
        if (hurtbox == null) return;

        // Extra safety: ensure it is really the player layer
        if (((1 << other.gameObject.layer) & playerLayer.value) == 0) return;

        hurtbox.ApplyDamage(damage);
        Destroy(gameObject);
    }

}
