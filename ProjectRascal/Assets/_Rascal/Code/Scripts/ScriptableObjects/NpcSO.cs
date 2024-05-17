using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcSO", menuName = "ScriptableObjects/NpcSO")]
public class NpcSO : ScriptableObject
{
    [Header("General")]
    public GameObject npcVisual;
    public string nameTextKey;
    public string actionTextKey;
    public string dialogKey;
    
    [Header("After dialog")]
    public bool showDialogAfter = true;
    public string dialogAfterKey;
    public bool spawnEnemyAfter = false;
    public EnemySO enemySO;
}
