using UnityEngine;

public class GameCharacter : MonoBehaviour
{
    [SerializeField] private float currentHealth = 20;
    [SerializeField] private float maxHealth = 20;
    [SerializeField] private float attack = 5;

    public void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;
        if(currentHealth < 0) {
            currentHealth = 0;
        }
    }

    public bool IsDead() => currentHealth <= 0;

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

    public float Attack
    {
        get { return attack; }
    }
}
