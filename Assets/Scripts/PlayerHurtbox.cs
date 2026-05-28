using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    public bool invulnerable = false;
    public PlayerHealth health;

    void Awake()
    {
        if (health == null) health = GetComponent<PlayerHealth>();
    }

    public void ApplyDamage(int dmg)
    {
        if (invulnerable) return;
        health.TakeDamage(dmg);
    }
}
