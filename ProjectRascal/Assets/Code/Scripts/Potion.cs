using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private float healthGiven = 0f;
    private InteractibleCanvas canvas;

    private void Start() {
        canvas = GetComponentInChildren<InteractibleCanvas>();
        canvas.HideActionText();
    }

    public void Interact(GameObject other) {
        if(other.TryGetComponent(out GameCharacter gameCharacter)) {
            gameCharacter.Heal(healthGiven);
            var position = other.transform.position;
            var effectPosition = new Vector3(position.x, position.y - 1f, position.z);
            VfxWizard.instance.SummonHealEffect(effectPosition);
            Destroy(transform.gameObject);
        }
    }

    public void OnVisionStart() {
        canvas.ShowActionText();
    }

    public void OnVisionEnd() {
        canvas.HideActionText();
    }
}
