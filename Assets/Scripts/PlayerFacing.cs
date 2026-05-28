using UnityEngine;

public class PlayerFacing : MonoBehaviour
{
    [Header("References")]
    public Transform weaponPivot;     // WeaponPivot transform
    public Transform attackHitbox;    // AttackHitbox transform (the object with BoxCollider2D)

    [Header("Hitbox (Right-facing)")]
    public Vector2 hitboxOffsetRight = new Vector2(0.6f, 0f);

    public bool FacingRight { get; private set; } = true;

    BoxCollider2D hitCol;

    void Awake()
    {
        if (attackHitbox != null)
            hitCol = attackHitbox.GetComponent<BoxCollider2D>();

        ApplyFacing(true);
    }

    // Call this from movement input
    public void SetFacingFromMove(float moveX)
    {
        if (moveX > 0.01f) ApplyFacing(true);
        else if (moveX < -0.01f) ApplyFacing(false);
    }

    void ApplyFacing(bool faceRight)
    {
        FacingRight = faceRight;

        // Flip player sprite visually by scaling X
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (FacingRight ? 1f : -1f);
        transform.localScale = s;

        // Keep weapon upright (optional). If weapon looks mirrored weirdly, use this:
        if (weaponPivot != null)
        {
            // weaponPivot localScale stays normal; it inherits player flip which is OK for most pixel weapons
            // If you want the weapon NOT mirrored, uncomment next two lines:
            // Vector3 ws = weaponPivot.localScale;
            // weaponPivot.localScale = new Vector3(Mathf.Abs(ws.x), ws.y, ws.z);
        }

        // Move hitbox to the correct side
        if (hitCol != null)
        {
            Vector2 o = hitboxOffsetRight;
            hitCol.offset = new Vector2(FacingRight ? Mathf.Abs(o.x) : -Mathf.Abs(o.x), o.y);
        }
    }
}
