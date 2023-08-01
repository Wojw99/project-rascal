using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Writing : InteractibleItem
{
    [SerializeField] private string text = "hello word!";

    private void Start() {
        ParentStart();
    }

    public override void Interact(GameObject other) {
        UIWizard.instance.ShowWriting(text);
    }
}
