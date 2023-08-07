using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDInteractible : InteractibleItem
{
    [SerializeField] private GameObject visualPrefab;
    [SerializeField] private GameObject visualSpawnPoint;
    [SerializeField] private GameObject damageDealerPrefab;

    private void Start() {
        ParentStart();
        Instantiate(visualPrefab, visualSpawnPoint.transform);
    }

    public override void Interact(GameObject other) {
        if(other.TryGetComponent(out PlayerController playerController)) {
            if(damageDealerPrefab != null) {
                playerController.UpdateWeaponDD(damageDealerPrefab);
            }
            Destroy(transform.gameObject);
        }
    }
}
