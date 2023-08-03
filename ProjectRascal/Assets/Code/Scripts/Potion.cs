using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : InteractibleItem
{
    [SerializeField] private float healthGiven = 0f;

    private void Start() {
        ParentStart();
    }

    public override void Interact(GameObject other) {
        if(other.TryGetComponent(out PlayerCharacter playerCharacter)) {
            playerCharacter.Heal(healthGiven);
            var position = other.transform.position;
            var effectPosition = new Vector3(position.x, position.y - 1f, position.z);
            VfxWizard.instance.SummonHealEffect(effectPosition, other.transform);
            Destroy(transform.gameObject);
        }
    }
}
