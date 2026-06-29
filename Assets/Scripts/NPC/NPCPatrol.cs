using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class NPCPatrol : MonoBehaviour
{
    [Header("Wander Area")]
    public float wanderWidth = 5;
    public float wanderHeight = 5;
    public Vector2 startingPosition;
    private bool isPaused;
    public float speed = 2.0f;
    private readonly float pauseDuration = 0.5f;
    private readonly float moveTimeout = 2.0f;
    private float moveTimer;
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        targetPosition = GetRandomTarget();
    }

    void Start()
    {
        StartCoroutine(SetNewTargetPosition());
    }

    void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // SPRITE DIRECTION
        Vector2 direction = ((Vector3)targetPosition - transform.position).normalized;
        if (direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        // MOVE
        rb.linearVelocity = direction * speed;
        moveTimer += Time.deltaTime;
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f || moveTimer >= moveTimeout)
        {
            StartCoroutine(SetNewTargetPosition());
        }
    }

    IEnumerator SetNewTargetPosition()
    {
        isPaused = true;
        animator.Play("LancerRedIdle");

        yield return new WaitForSeconds(pauseDuration);

        targetPosition = GetRandomTarget();
        moveTimer = 0f;
        isPaused = false;
        animator.Play("LancerRedWalk");
    }

    private Vector2 GetRandomTarget()
    {
        float halfWidth = wanderWidth / 2;
        float halfHeight = wanderHeight / 2;
        int edge = Random.Range(0, 4);

        return edge switch
        {
            0 => new Vector2(startingPosition.x - halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)),
            1 => new Vector2(startingPosition.x + halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)),
            2 => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y - halfHeight),
            _ => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y + halfHeight),
        };
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(startingPosition, new Vector3(wanderWidth, wanderHeight, 0));
    }
}
