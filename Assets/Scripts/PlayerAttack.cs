using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerHitbox hitbox;

    public float cooldown = 0.35f;
    private float nextTime;

    void Awake()
    {
        if (!anim) anim = GetComponentInChildren<Animator>();

        // Auto-find hitbox if not set
        if (!hitbox)
        {
            hitbox = GetComponentInChildren<PlayerHitbox>();
            if (hitbox)
            {
                Debug.Log($"✅ Found hitbox: {hitbox.gameObject.name}");
            }
            else
            {
                Debug.LogError("❌ No PlayerHitbox found in children!");
            }
        }
    }

    void Update()
    {
        if (Time.time < nextTime) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
        {
            nextTime = Time.time + cooldown;
            anim.SetTrigger("Attack");
        }
    }

    // Animation Events call these:
    public void HitboxOn()
    {
        Debug.Log("🎬 HITBOX ON (Animation Event)");
        if (hitbox)
        {
            hitbox.EnableHitbox();
        }
        else
        {
            Debug.LogError("Hitbox reference is null!");
        }
    }

    public void HitboxOff()
    {
        Debug.Log("🎬 HITBOX OFF (Animation Event)");
        if (hitbox)
        {
            hitbox.DisableHitbox();
        }
    }

    // Debug GUI
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"Cooldown: {Mathf.Max(0, nextTime - Time.time):F2}");

        if (GUI.Button(new Rect(10, 40, 150, 30), "Manual Attack"))
        {
            HitboxOn();
            Invoke(nameof(HitboxOff), 0.1f);
        }
    }
}