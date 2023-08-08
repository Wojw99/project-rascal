using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Interactible
{
    [SerializeField] private int amount = 0;

    private void Start() {
        ParentStart();
    }

    public override void Interact(GameObject other) {
        if(other.TryGetComponent(out PlayerCharacter gameCharacter)) {
            gameCharacter.AddGold(amount);
            Destroy(transform.gameObject);
        }
    }
}
