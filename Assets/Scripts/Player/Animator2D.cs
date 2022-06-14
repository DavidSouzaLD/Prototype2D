using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Animator2D : MonoBehaviour
{
    Animator animator;
    Controller2D controller2D;

    private void Start()
    {
        if (GetComponentInParent<Controller2D>() != null)
        {
            controller2D = GetComponentInParent<Controller2D>();
        }
        else
        {
            this.enabled = false;
        }

        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        Vector2 _moveAxis = controller2D.Input.MoveAxis();

        if (_moveAxis.x != 0) // Walk
        {
            animator.SetBool("Walk", true);
        }
        else // Idle
        {
            animator.SetBool("Walk", false);
        }

        if (controller2D._startJump)
        {
            animator.Play("Jump", 0);
        }
    }
}
