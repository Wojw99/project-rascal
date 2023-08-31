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
        name = PlayerCharacterLoadEmissary.instance.PlayerCharacterAttributes.name;
        currentHealth = PlayerCharacterLoadEmissary.instance.PlayerCharacterAttributes.currentHealth;
        currentMana = PlayerCharacterLoadEmissary.instance.PlayerCharacterAttributes.currentMana;
        maxHealth = PlayerCharacterLoadEmissary.instance.PlayerCharacterAttributes.maxHealth;
        maxMana = PlayerCharacterLoadEmissary.instance.PlayerCharacterAttributes.maxMana;
    }

    private void ChangeName()
    {
        name = CharacterStateEmissary.instance.PlayerChrAttr.name;
    }

    private void ChangeCurrentHealth()
    {
        currentHealth = CharacterStateEmissary.instance.PlayerChrAttr.currentHealth;
        UIWizard.instance.UpdateHpBar(currentHealth, maxHealth);
    }

    private void ChangeCurrentMana()
    {
        currentMana = CharacterStateEmissary.instance.PlayerChrAttr.currentMana;
        UIWizard.instance.UpdateMpBar(currentMana, maxMana);
    }

    private void ChangeMaxHealth()
    {
        maxHealth = CharacterStateEmissary.instance.PlayerChrAttr.maxHealth;
        UIWizard.instance.UpdateHpBar(currentHealth, maxHealth);
    }

    private void ChangeMaxMana()
    {
        maxMana = CharacterStateEmissary.instance.PlayerChrAttr.maxMana;
        UIWizard.instance.UpdateMpBar(currentMana, maxMana);
    }

    private void Start() {
        PlayerCharacterLoadEmissary.instance.OnCharacterLoadSucces += CharacterLoadSucces;
        CharacterStateEmissary.instance.OnPlayerNameChanged += ChangeName;
        CharacterStateEmissary.instance.OnPlayerCurrentHealthChanged += ChangeCurrentHealth;
        CharacterStateEmissary.instance.OnPlayerCurrentManaChanged += ChangeCurrentMana;
        CharacterStateEmissary.instance.OnPlayerMaxHealthChanged += ChangeMaxHealth;
        CharacterStateEmissary.instance.OnPlayerMaxManaChanged += ChangeMaxMana;

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
