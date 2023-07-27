using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private float healthGiven = 0f;

    public void Interact(GameObject other) {
        if(other.TryGetComponent(out GameCharacter gameCharacter)) {
            gameCharacter.Heal(healthGiven);
            var position = other.transform.position;
            var effectPosition = new Vector3(position.x, position.y + 0.1f, position.z);
            VfxWizard.instance.SummonFancyCircleEffect(effectPosition);
            Destroy(transform.gameObject);
        }
    }
}
