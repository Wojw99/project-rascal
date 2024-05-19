using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : GameCharacter
{
    [SerializeField] protected int _abyssKnowledge = 0;
    [SerializeField] protected int _abyssEnergy = 0;

    [SerializeField] private readonly int maxAbyssKnowledge = 1000;
    [SerializeField] private readonly int maxAbyssEnergy = 200;
    [SerializeField] private readonly int defaultKnowledgeForStoryProgress = 50;

    public event Action StatsChanged;

    private void Start() {
        UIWizard.instance.UpdateHpBar(currentHealth, maxHealth);
        UIWizard.instance.UpdateMpBar(currentMana, maxMana);
        EnemyController.EnemyDeath += OnEnemyDeath;
        GameVariablesWizard.instance.GameVariableChanged += OnGameVariableChanged;
    }

    private void OnGameVariableChanged(GvKey gvKey) {
        AbyssKnowledge += defaultKnowledgeForStoryProgress;
    }

    public void OnEnemyDeath(int abyssEnergyReward, int abyssKnowledgeReward) {
        AddAbyssEnergy(abyssEnergyReward);
        AddAbyssKnowledge(abyssKnowledgeReward);
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
        AbyssKnowledge += amount;
        AbyssKnowledge = Mathf.Clamp(AbyssKnowledge, 0, maxAbyssKnowledge);
    }

    public void AddAbyssEnergy(int amount) {
        AbyssEnergy += amount;
        AbyssEnergy = Mathf.Clamp(AbyssEnergy, 0, maxAbyssEnergy);
    }

    private void OnDestroy() {
        EnemyController.EnemyDeath -= OnEnemyDeath;
    }

    public int AbyssEnergy {
        get => _abyssEnergy;
        set {
            _abyssEnergy = value;
            StatsChanged?.Invoke();
        }
    }

    public int AbyssKnowledge {
        get => _abyssKnowledge;
        set {
            _abyssKnowledge = value;
            StatsChanged?.Invoke();
        }
    }
}
