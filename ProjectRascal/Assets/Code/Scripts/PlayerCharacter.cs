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
        name = CharacterStateEmissary.Instance.Name;
        currentHealth = CharacterStateEmissary.Instance.CurrentHealth;
        currentMana = CharacterStateEmissary.Instance.CurrentMana;
        maxHealth = CharacterStateEmissary.Instance.MaxHealth;
        maxMana = CharacterStateEmissary.Instance.MaxMana;
    }

    private void ChangeName()
    {
        name = CharacterStateEmissary.Instance.Name;
    }

    private void ChangeCurrentHealth()
    {
        currentHealth = CharacterStateEmissary.Instance.CurrentHealth;
        UIWizard.instance.UpdateHpBar(currentHealth, maxHealth);
    }

    private void ChangeCurrentMana()
    {
        currentMana = CharacterStateEmissary.Instance.CurrentMana;
        UIWizard.instance.UpdateMpBar(currentMana, maxMana);
    }

    private void ChangeMaxHealth()
    {
        maxHealth = CharacterStateEmissary.Instance.MaxHealth;
        UIWizard.instance.UpdateHpBar(currentHealth, maxHealth);
    }

    private void ChangeMaxMana()
    {
        maxMana = CharacterStateEmissary.Instance.MaxMana;
        UIWizard.instance.UpdateMpBar(currentMana, maxMana);
    }

    private void Start() {
        CharacterLoadEmissary.Instance.OnCharacterLoadSucces += CharacterLoadSucces;
        CharacterStateEmissary.Instance.OnPlayerNameUpdate += ChangeName;
        CharacterStateEmissary.Instance.OnPlayerCurrentHealthUpdate += ChangeCurrentHealth;
        CharacterStateEmissary.Instance.OnPlayerCurrentManaUpdate += ChangeCurrentMana;
        CharacterStateEmissary.Instance.OnPlayerMaxHealthUpdate += ChangeMaxHealth;
        CharacterStateEmissary.Instance.OnPlayerMaxManaUpdate += ChangeMaxMana;

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
