using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableWeapon : MonoBehaviour
{
    [SerializeField] private float damageAmount = 0f;
    [SerializeField] private bool isBloodSpillVisible = true;
    private Collider damageAreaCollider;
    private float finalDamage = 0f;
    private List<GameObject> damagedGameObjects;

    private void Start() {
        damageAreaCollider = GetComponent<Collider>();
        damageAreaCollider.enabled = false;
        damagedGameObjects = new List<GameObject>();
    }

    public void TakeDamage(float additionalDamage = 1f, float damageDuration = 0.87f) {
        finalDamage = damageAmount + additionalDamage;
        damageAreaCollider.enabled = true;
        damagedGameObjects.Clear();
        Invoke("DisableDamage", damageDuration);
    }

    private void DisableDamage() => damageAreaCollider.enabled = false;

    private void OnTriggerEnter(Collider other) {
        if(!damagedGameObjects.Contains(other.gameObject)) {
            var character = other.GetComponent<GameCharacter>();
            var controller = other.GetComponent<IDamagaController>();

            damagedGameObjects.Add(other.gameObject);

            if(character != null) {
                character.TakeDamage(finalDamage);
            }

            if(controller != null) {
                controller.VisualizeDamage(transform.position, true);
            }
        }
    }
}
