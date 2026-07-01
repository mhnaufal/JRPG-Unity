using System.Collections;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public CharacterStats stats;
    public int currentHealth;
    public Animator animator;

    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    public void Setup()
    {
        currentHealth = stats.maxHealth;
    }

    public bool TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        return currentHealth <= 0;
    }

    public void PlayAnimation(string stateName)
    {
        if (animator != null)
        {
            animator.Play(stateName);
        }
    }

    public IEnumerator MoveTo(Vector3 target, float speed)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }
}
