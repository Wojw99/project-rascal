using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : InteractibleItem
{
    [SerializeField] private int eventId = 0;
    [SerializeField] private GameObject enemySpawned;

    private bool A = false;

    private void Start() {
        ParentStart();
    }

    protected override void HandleSignal(string signal) {
        if(signal == "e0") {
            EventSignalizer.instance.Clear();
            Debug.Log("summon");
            VfxWizard.instance.SummonFancyCircleEffect(transform.position);
        } 
        if(signal == "e1") {
            EventSignalizer.instance.Clear();
            Debug.Log("transform");
            Instantiate(enemySpawned, transform.position, transform.rotation);
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
