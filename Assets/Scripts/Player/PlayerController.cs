using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public int facingDirection = 1;
    private float horizontalMove, verticalMove;
    public Rigidbody2D rb;
    public Animator animator;
    public CharacterStats characterStats;
    public CharacterInventory characterInventory;
    private IInteractable interactableInRange;

    void Start()
    {
        characterInventory.collectedGold = 0;
    }

    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        animator.SetFloat("horizontal", Mathf.Abs(horizontalMove));
        animator.SetFloat("vertical", Mathf.Abs(verticalMove));

        if (horizontalMove > 0)
        {
            facingDirection = 1;
        }
        else if (horizontalMove < 0)
        {
            facingDirection = -1;
        }
        transform.localScale = new Vector3(facingDirection, transform.localScale.y, transform.localScale.z);

        if (interactableInRange != null && Input.GetKeyDown(KeyCode.Space))
        {
            interactableInRange.Interact(this);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalMove, verticalMove) * moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out var interactable))
        {
            interactableInRange = interactable;
            interactableInRange.OnEnterRange();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out var interactable) && interactableInRange == interactable)
        {
            interactableInRange.OnExitRange();
            interactableInRange = null;
        }
    }

    public void AddGold(int gold)
    {
        characterInventory.collectedGold += gold;
    }
}
