using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : InteractibleItem
{
    [SerializeField] private SceneLoadWizard.Scene scene;

    private void Start() {
        ParentStart();
    }

    public override void Interact(GameObject other) {
        if(other.TryGetComponent(out PlayerCharacter playerCharacter)) {
            SceneLoadWizard.Load(scene);
        }
    }
}
