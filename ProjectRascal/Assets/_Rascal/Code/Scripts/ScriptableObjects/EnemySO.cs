using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("General")]
    public GameObject visual;
    public float maxHealth;
    public float maxMana;
    public float attack;
    public float magic;
}
