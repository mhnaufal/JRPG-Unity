using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(Animator))]
public class NPCPatrol : MonoBehaviour, IInteractable
{
    public Vector2[] patrolPoints;
    public float speed = 2.0f;
    private int currentPatrolIndex;
    private float pauseDuration = 0.5f;
    private bool isPaused;
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private Animator animator;

    public void OnEnterRange() { }
    public void OnExitRange() { }
    public void Interact(PlayerController playerController) { }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(SetPatrolPoint());
    }

    void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector3)targetPosition - transform.position).normalized;
        if (direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        rb.linearVelocity = direction * speed;

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (patrolPoints == null || patrolPoints.Length == 0)
            {
                return;
            }

            StartCoroutine(SetPatrolPoint());
        }
    }

    IEnumerator SetPatrolPoint()
    {
        isPaused = true;
        animator.Play("LancerRedIdle");

        yield return new WaitForSeconds(pauseDuration);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        targetPosition = patrolPoints[currentPatrolIndex];
        isPaused = false;
        animator.Play("LancerRedWalk");
    }
}
