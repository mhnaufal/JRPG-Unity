using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "JRPG/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public string charName;
    public int maxHealth;
    public int maxAttackDamage;
    public int minAttackDamage;
    public int turnPriority;
}
