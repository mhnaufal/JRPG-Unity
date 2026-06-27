using UnityEngine;

public class GoldScript : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private int goldAmount = 1;

    void Awake()
    {
        interactionIcon.SetActive(false);
    }
    public void OnEnterRange()
    {
        interactionIcon.SetActive(true);
    }
    public void OnExitRange()
    {
        interactionIcon.SetActive(false);
    }
    public void Interact(PlayerController player)
    {
        player.AddGold(goldAmount);
        Destroy(gameObject);
    }
}
