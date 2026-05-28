using UnityEngine;

public class AttackTest : MonoBehaviour
{
    Animator anim;

    void Awake() => anim = GetComponent<Animator>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Trigger Attack");
            anim.SetTrigger("Attack");
        }
    }
}
