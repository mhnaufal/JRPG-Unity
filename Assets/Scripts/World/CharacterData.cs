using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "JRPG/CharacterData")]
public class CharacterData : ScriptableObject
{
    public int collectedGold;
    public int maxHealth;
    public int attackDamage;
    public int turnPriority;
}
