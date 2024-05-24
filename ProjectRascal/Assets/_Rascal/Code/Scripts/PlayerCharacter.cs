using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : GameCharacter
{
    protected int _abyssKnowledge = 0;
    protected int _abyssEnergy = 100;

    [SerializeField] private readonly int abyssEnergyStart = 100;
    [SerializeField] private readonly int abyssKnowledgeStart = 0;
    [SerializeField] private readonly int maxAbyssKnowledge = 1000;
    [SerializeField] private readonly int maxAbyssEnergy = 200;
    [SerializeField] private readonly int defaultKnowledgeOfStoryProgress = 50;

    public static event Action<int> AbyssEnergyChanged;
    public static event Action<int> AbyssKnowledgeChanged;

    public static event Action<int> AbyssEnergyPercentChanged;
    public static event Action<int> AbyssKnowledgePercentChanged;

    private void Start() {
        EnemyController.EnemyDeath += OnEnemyDeath;
        GameVariablesWizard.instance.GameVariableChanged += OnGameVariableChanged;
        StartCoroutine(InitStats());
    }

    private IEnumerator InitStats() {
        yield return new WaitForSeconds(0.1f);
        AbyssEnergy = abyssEnergyStart;
        AbyssKnowledge = abyssKnowledgeStart;
    }

    private void OnGameVariableChanged(GvKey gvKey) {
        AbyssKnowledge += defaultKnowledgeOfStoryProgress;
    }

    public void OnEnemyDeath(int abyssEnergyReward, int abyssKnowledgeReward) {
        AddAbyssEnergy(abyssEnergyReward);
        AddAbyssKnowledge(abyssKnowledgeReward);
    }

    public override void TakeDamage(float damageAmount) {
        AddAbyssEnergy((int)damageAmount * -1);
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
            AbyssEnergyChanged?.Invoke(_abyssEnergy);

            var percent = (int)CalculatePercent(_abyssEnergy, maxAbyssEnergy);
            AbyssEnergyPercentChanged?.Invoke(percent);
        }
    }

    public int AbyssKnowledge {
        get => _abyssKnowledge;
        set {
            _abyssKnowledge = value;
            AbyssKnowledgeChanged?.Invoke(_abyssKnowledge);

            var percent = (int)CalculatePercent(_abyssKnowledge, maxAbyssKnowledge);
            AbyssKnowledgePercentChanged?.Invoke(percent);
        }
    }

    private double CalculatePercent(int value, int maxValue) {
        return Convert.ToDouble(value) / Convert.ToDouble(maxValue) * 100;
    }

    public int MaxAbyssEnergy => maxAbyssEnergy;
    public int AbyssEnergyPercent => (AbyssEnergy / maxAbyssEnergy) * 100;

    public int MaxAbyssKnowledge => maxAbyssKnowledge;
    public int AbyssKnowledgePercent => (AbyssKnowledge / maxAbyssKnowledge) * 100;
}
