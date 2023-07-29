using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractibleCanvas : GeneralCanvas
{
    [SerializeField] private TextMeshProUGUI nameTextMesh;
    [SerializeField] private TextMeshProUGUI actionTextMesh;
    [SerializeField] private string nameText;
    [SerializeField] private string actionText;

    private void Start() {
        GeneralStart();
        nameTextMesh.text = nameText;
        actionTextMesh.text = actionText;
    }

    private void Update() {
        UpdateRotation();
    }

    public void ShowActionText() {
        actionTextMesh.enabled = true;
    }

    public void HideActionText() {
        actionTextMesh.enabled = false;
    }
}
