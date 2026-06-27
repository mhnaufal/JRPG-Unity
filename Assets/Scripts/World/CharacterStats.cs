using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "JRPG/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public int maxHealth;
    public int attackDamage;
    public int turnPriority;
}
