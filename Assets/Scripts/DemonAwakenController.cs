using UnityEngine;

public class DemonAwakenController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerController controller;

    private static readonly int AwakenTrig = Animator.StringToHash("Awaken");
    private static readonly int IsDemonBool = Animator.StringToHash("IsDemon");

    private bool isDemon = false;
    private bool isAwakening = false;

    private void Reset()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(toggleKey)) return;
        if (isAwakening) return; // don't allow toggling during awakening

        if (!isDemon)
        {
            // Start awakening (demon ON) -> UI/HP switches at animation end event
            isAwakening = true;
            animator.SetBool(IsDemonBool, true);
            animator.SetTrigger(AwakenTrig);
        }
        else
        {
            // Demon OFF instantly
            isDemon = false;
            animator.SetBool(IsDemonBool, false);

            // Switch gameplay + UI back to normal
            if (controller != null) controller.SetDemon(false);
            if (health != null) health.SetDemonForm(false);
        }
    }

    // ✅ Animation Event at END of knight_awakening clip
    public void OnAwakeningComplete()
    {
        isAwakening = false;
        isDemon = true;

        // Switch gameplay + UI to demon
        if (controller != null) controller.SetDemon(true);
        if (health != null) health.SetDemonForm(true);
    }
}
