using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public Animator interactionAnimator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        animator.Play("LancerRedIdle");

        // interactionAnimator.enabled = true;
        interactionAnimator.Play("InteractionIconOpen");
    }

    private void OnDisable()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        interactionAnimator.Play("InteractionIconClose");
        // interactionAnimator.enabled = false;
    }
}
