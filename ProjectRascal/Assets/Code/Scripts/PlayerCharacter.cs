using Assets.Code.Scripts.NetClient.Emissary;
using NetworkCore.Packets;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : GameCharacter
{
    [SerializeField] protected int gold = 0;

    public void CharacterLoadSucces()
    {
        name = CharacterStateEmissary.instance.Name;
        currentHealth = CharacterStateEmissary.instance.CurrentHealth;
        currentMana = CharacterStateEmissary.instance.CurrentMana;
        maxHealth = CharacterStateEmissary.instance.MaxHealth;
        maxMana = CharacterStateEmissary.instance.MaxMana;
    }

    private void ChangeName()
    {
        name = CharacterStateEmissary.instance.Name;
    }

    private void ChangeCurrentHealth()
    {
        currentHealth = CharacterStateEmissary.instance.CurrentHealth;
        UIWizard.instance.UpdateHpBar(currentHealth, maxHealth);
    }

    private void ChangeCurrentMana()
    {
        currentMana = CharacterStateEmissary.instance.CurrentMana;
        UIWizard.instance.UpdateMpBar(currentMana, maxMana);
    }

    private void ChangeMaxHealth()
    {
        maxHealth = CharacterStateEmissary.instance.MaxHealth;
        UIWizard.instance.UpdateHpBar(currentHealth, maxHealth);
    }

    private void ChangeMaxMana()
    {
        maxMana = CharacterStateEmissary.instance.MaxMana;
        UIWizard.instance.UpdateMpBar(currentMana, maxMana);
    }

    private void Start() {
        CharacterLoadEmissary.instance.OnCharacterLoadSucces += CharacterLoadSucces;
        CharacterStateEmissary.instance.OnPlayerNameUpdate += ChangeName;
        CharacterStateEmissary.instance.OnPlayerCurrentHealthUpdate += ChangeCurrentHealth;
        CharacterStateEmissary.instance.OnPlayerCurrentManaUpdate += ChangeCurrentMana;
        CharacterStateEmissary.instance.OnPlayerMaxHealthUpdate += ChangeMaxHealth;
        CharacterStateEmissary.instance.OnPlayerMaxManaUpdate += ChangeMaxMana;

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
