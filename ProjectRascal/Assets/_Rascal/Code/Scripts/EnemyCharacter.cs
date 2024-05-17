using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : GameCharacter
{
    [SerializeField] private bool isAgressive = true;

    public bool IsAgressive => isAgressive;
}
