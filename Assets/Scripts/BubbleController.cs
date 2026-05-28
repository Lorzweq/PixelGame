using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public ParticleSystem bubbles;

    ParticleSystem.EmissionModule emission;

    void Awake()
    {
        emission = bubbles.emission;
        emission.enabled = false;
    }

    public void SetSwimming(bool isSwimming)
    {
        emission.enabled = isSwimming;
        if (isSwimming && !bubbles.isPlaying) bubbles.Play();
        if (!isSwimming && bubbles.isPlaying) bubbles.Stop();
    }
}
