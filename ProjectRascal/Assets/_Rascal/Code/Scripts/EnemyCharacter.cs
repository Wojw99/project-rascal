using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : GameCharacter
{
    [SerializeField] private bool isAgressive = true;
    [SerializeField] private int abyssEnergyReward = 10;
    [SerializeField] private int abyssKnowledgeReward = 10;

    public bool IsAgressive => isAgressive;
    public int AbyssEnergyReward => abyssEnergyReward;
    public int AbyssKnowledgeReward => abyssKnowledgeReward;
}
