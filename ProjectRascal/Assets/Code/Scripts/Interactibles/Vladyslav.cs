using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vladyslav : Interactible
{
    [SerializeField] private GameObject enemySpawned;
    [SerializeField] private GameObject spawnVFX;

    private bool A = false;

    private void Start() {
        ParentStart();
    }

    protected override void HandleSignal(string signal) {
        if(signal == "e0") {
            InstantiateAsChild(spawnVFX);
        } 
        if(signal == "e1") {
            InstantiateAtLocation(enemySpawned);
            Destroy(transform.gameObject);
        }
    }

    public override void Interact(GameObject other) {
        if(other.TryGetComponent(out PlayerCharacter playerCharacter)) {
            if(A) {
                EventWizard.instance.PlayEvent(1);
            } else {
                EventWizard.instance.PlayEvent(0);
                A = true;
            }
        }
    }
}
