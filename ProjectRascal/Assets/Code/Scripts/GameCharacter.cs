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
        get { return VId; }
    }

    public string Name
    {
        get { return name; }
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
