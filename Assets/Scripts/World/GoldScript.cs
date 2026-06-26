using UnityEngine;

public class GoldScript : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject interactionIcon;

    void Start()
    {
        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionIcon.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionIcon.SetActive(false);
        }
    }
}
