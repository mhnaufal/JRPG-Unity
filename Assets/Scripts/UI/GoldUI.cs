using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private CharacterInventory characterInventory;
    [SerializeField] private TMP_Text goldAmountText;

    void Update()
    {
        goldAmountText.text = characterInventory.collectedGold.ToString();
    }
}
