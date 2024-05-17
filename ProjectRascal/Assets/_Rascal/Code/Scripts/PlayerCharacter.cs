using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : GameCharacter
{
    [SerializeField] protected int gold = 0;
    [SerializeField] protected int abyssKnowledge = 0;
    [SerializeField] protected int abyssEnergy = 0;

    [SerializeField] private readonly int maxAbyssKnowledge = 1000;
    [SerializeField] private readonly int maxAbyssEnergy = 200;

    private void Start() {
        UIWizard.instance.UpdateGold(gold.ToString());
        UIWizard.instance.UpdateHpBar(currentHealth, maxHealth);
        UIWizard.instance.UpdateMpBar(currentMana, maxMana);
    }

    public override void TakeDamage(float damageAmount) {
        base.TakeDamage(damageAmount);
        UIWizard.instance.UpdateHpBar(currentHealth, maxHealth);
    }

    public override void Heal(float healthPoints) {
        base.Heal(healthPoints);
        UIWizard.instance.UpdateHpBar(currentHealth, maxHealth);
    }

    public void AddAbyssKnowledge(int amount) {
        abyssKnowledge += amount;
        abyssKnowledge = Mathf.Clamp(abyssKnowledge, 0, 1000);
    }

    public void AddAbyssEnergy(int amount) {
        abyssEnergy += amount;
        abyssEnergy = Mathf.Clamp(abyssEnergy, 0, 200);
    }

    public void AddGold(int amount) {
        gold += amount;
        if (gold < 0) {
            gold = 0;
        }
        UIWizard.instance.UpdateGold(gold.ToString());
    }

    public float Gold => gold;
}
