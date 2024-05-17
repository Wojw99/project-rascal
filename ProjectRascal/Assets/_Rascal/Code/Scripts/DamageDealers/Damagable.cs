using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] private float damageAmount = 5f;
    private CapsuleCollider capsuleCollider;

    private void Start() {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        var character = other.GetComponent<GameCharacter>();
        var controller = other.GetComponent<IDamagaController>();
        if(character != null) {
            character.TakeDamage(damageAmount);
        }
        if(controller != null) {
            controller.VisualizeDamage(transform.position, bloodSpill: false);
        }
    }
}
