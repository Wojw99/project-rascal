using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : GameCharacter
{
    [SerializeField] private GameObject dummyVisual;
    [SerializeField] private EnemySO enemyData;

    private void Start()
    {
        SetupWithData(enemyData);
    }

    public void SetupWithNewData(EnemySO data)
    {
        enemyData = data;
        SetupWithData(data);
    }

    private void SetupWithData(EnemySO data)
    {
        currentHealth = data.maxHealth;
        maxHealth = data.maxHealth;
        currentMana = data.maxMana;
        maxMana = data.maxMana;
        attack = data.attack;
        magic = data.magic;

        var visual = data.visual;
        if(visual != null) {
            visual.transform.position = Vector3.zero;
            visual.transform.rotation = Quaternion.identity;
            Instantiate(visual, transform);
            Destroy(dummyVisual);
        }

        GetComponent<EnemyController>().UpdateAnimatorAndWeapon();
    }

    public EnemySO EnemyData
    {
        get { return enemyData; }
    }
}
