using System;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 0.0f;
    public int facingDirection = 1;
    public Rigidbody2D rb;
    public Animator animator;
    public CharacterData data;
    public TMP_Text collectedGold;
    private GameObject goldInRange;

    void Start()
    {
        UpdateCollectedGoldUI(data, collectedGold);
    }

    // FixedUpdate is called 50x per frame
    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

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

        rb.linearVelocity = new Vector2(horizontalMove, verticalMove) * moveSpeed;

        if (goldInRange != null && Input.GetKeyDown(KeyCode.Space))
        {
            data.collectedGold += 1;
            UpdateCollectedGoldUI(data, collectedGold);

            Destroy(goldInRange);
            goldInRange = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InteractableGold"))
        {
            goldInRange = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == goldInRange)
        {
            goldInRange = null;
        }
    }

    void UpdateCollectedGoldUI(CharacterData data, TMP_Text collectedGold)
    {
        collectedGold.text = data.collectedGold.ToString();
    }
}
