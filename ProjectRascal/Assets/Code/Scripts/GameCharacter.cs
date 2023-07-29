using UnityEngine;

public class GameCharacter : MonoBehaviour
{
    [SerializeField] private float currentHealth = 20;
    [SerializeField] private float maxHealth = 20;
    [SerializeField] private float attack = 5;
    [SerializeField] private float magic = 10;

    public void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;
        if(currentHealth < 0) {
            currentHealth = 0;
        }
    }

    public bool IsDead() => currentHealth <= 0;

    public void Heal(float healthPoints) {
        currentHealth += healthPoints;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        var canvas = transform.gameObject.GetComponentInChildren<CharacterCanvas>();
        if(canvas != null) {
            canvas.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

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

    public float Magic
    {
        get { return magic; }
    }
}
