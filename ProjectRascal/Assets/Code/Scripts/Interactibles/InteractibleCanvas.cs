using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractibleCanvas : MapCanvas
{
    private void Start() {
        ParentStart();
    }

    private void Update() {
        UpdateRotation();
    }
}
