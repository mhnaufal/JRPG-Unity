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
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        animator.Play("LancerRedIdle");

        interactionAnimator.Play("InteractionIconOpen");
    }

    private void OnDisable()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        interactionAnimator.Play("InteractionIconClose");
    }
}
