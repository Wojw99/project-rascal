using NetworkCore.Packets;
using Unity.VisualScripting;
using UnityEngine;

public class GameCharacter : MonoBehaviour
{
    [SerializeField] protected int vId = -1;
    [SerializeField] protected new string name = string.Empty;
    [SerializeField] protected float currentHealth = 20;
    [SerializeField] protected float maxHealth = 20;
    [SerializeField] protected float currentMana = 20;
    [SerializeField] protected float maxMana = 20;
    [SerializeField] protected float attack = 5;
    [SerializeField] protected float magic = 10;

    public virtual void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;
        if(currentHealth < 0) {
            currentHealth = 0;
        }
    }

    public bool IsDead() => currentHealth <= 0;

    public virtual void Heal(float healthPoints) {
        currentHealth += healthPoints;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        var canvas = transform.gameObject.GetComponentInChildren<CharacterCanvas>();
        canvas.UpdateHealthBar(currentHealth, maxHealth);
    }

    public int VId
    {
        get;
    }

    public string Name
    {
        get;
    }

    public float CurrentHealth
    {
        get;
    }

    public float MaxHealth
    {
        get;
    }

    public float Attack
    {
        get;
    }

    public float Magic
    {
        get;
    }
}
