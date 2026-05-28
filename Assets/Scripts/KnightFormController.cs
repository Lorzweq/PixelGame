using UnityEngine;

public class KnightFormController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;

    [Header("Input")]
    [SerializeField] private KeyCode transformKey = KeyCode.F;

    private static readonly int AwakenTrig = Animator.StringToHash("Awaken");
    private static readonly int IsDemonBool = Animator.StringToHash("IsDemon");

    private bool isDemon;

    private void Reset()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(transformKey))
        {
            ToggleForm();
        }
    }

    public void ToggleForm()
    {
        isDemon = !isDemon;

        // When turning demon ON: play awakening first, then go demon idle.
        if (isDemon)
        {
            animator.SetBool(IsDemonBool, true);
            animator.SetTrigger(AwakenTrig);
        }
        // When turning demon OFF: instantly go back to knight idle (via transition).
        else
        {
            animator.ResetTrigger(AwakenTrig);
            animator.SetBool(IsDemonBool, false);
        }
    }
}
