using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HpFlaskUI : MonoBehaviour
{
    [SerializeField] private Image flaskImage;

    [Tooltip("Frames ordered from FULL (0) to EMPTY (last)")]
    [SerializeField] private Sprite[] hpFrames;

    [Header("Smoothing")]
    [SerializeField] private float drainSeconds = 0.18f; // good default for 20 frames
    [SerializeField] private bool smoothHealToo = false;

    private Coroutine animRoutine;
    private float displayedHp = -1f;

    public void SetHp(int currentHp, int maxHp)
    {
        if (hpFrames == null || hpFrames.Length == 0) return;
        if (maxHp <= 0) return;

        // Initialize on first call
        if (displayedHp < 0f)
            displayedHp = currentHp;

        if (animRoutine != null) StopCoroutine(animRoutine);

        bool isDamage = currentHp < displayedHp;
        bool doSmooth = isDamage || smoothHealToo;

        if (!doSmooth)
        {
            displayedHp = currentHp;
            ApplyFrame(displayedHp, maxHp);
            return;
        }

        animRoutine = StartCoroutine(AnimateTo(currentHp, maxHp));
    }

    private IEnumerator AnimateTo(int targetHp, int maxHp)
    {
        float start = displayedHp;
        float end = targetHp;

        float t = 0f;
        float dur = Mathf.Max(0.01f, drainSeconds);

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / dur;
            displayedHp = Mathf.Lerp(start, end, t);
            ApplyFrame(displayedHp, maxHp);
            yield return null;
        }

        displayedHp = end;
        ApplyFrame(displayedHp, maxHp);
        animRoutine = null;
    }

    private void ApplyFrame(float hpValue, int maxHp)
    {
        float hpPercent = Mathf.Clamp01(hpValue / maxHp);

        int frameCount = hpFrames.Length;

        // 100% => 0, 0% => last
        int index = Mathf.FloorToInt((1f - hpPercent) * frameCount);
        index = Mathf.Clamp(index, 0, frameCount - 1);

        flaskImage.sprite = hpFrames[index];
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);

        if (!visible)
        {
            displayedHp = -1f;
            if (animRoutine != null)
            {
                StopCoroutine(animRoutine);
                animRoutine = null;
            }
        }
    }


}
