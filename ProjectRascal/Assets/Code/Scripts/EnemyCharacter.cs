using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralCharacter : MonoBehaviour
{
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected Image healthBarSprite;

    private void Start() {
        
    }

    private void Update() {
        
    }

    public void TakeDamage(float damageAmount) {
        currentHealth -= damageAmount;
        healthBarSprite.fillAmount = currentHealth / maxHealth;
    }


}
