using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : GameCharacter
{
    [SerializeField] protected int gold = 0;

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

    public void AddGold(int amount) {
        gold += amount;
        if (gold < 0) {
            gold = 0;
        }
        UIWizard.instance.UpdateGold(gold.ToString());
    }

    public float Gold
    {
        get { return magic; }
    }
}
