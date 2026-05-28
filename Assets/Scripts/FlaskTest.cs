using UnityEngine;

public class FlaskTest : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.Play("HPFlask_Drain", 0, 0f); // must match the STATE name
        }
    }
}
