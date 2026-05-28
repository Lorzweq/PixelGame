using UnityEngine;

public class WaterTint : MonoBehaviour
{
    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color inWaterColor = new Color(0.6f, 0.8f, 1f, 1f); // blue-ish

    [Header("Optional")]
    public float lerpSpeed = 8f;

    SpriteRenderer sr;
    Color target;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        target = normalColor;
    }

    void Update()
    {
        if (sr) sr.color = Color.Lerp(sr.color, target, Time.deltaTime * lerpSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water")) target = inWaterColor;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water")) target = normalColor;
    }
}
