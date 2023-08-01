using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractibleItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTextMesh;
    [SerializeField] private TextMeshProUGUI actionTextMesh;
    [SerializeField] private string nameText;
    [SerializeField] private string actionText;

    protected void ParentStart() {
        nameTextMesh.text = nameText;
        actionTextMesh.text = actionText;
        HideActionText();
    }

    public void ShowActionText() {
        actionTextMesh.enabled = true;
    }

    public void HideActionText() {
        actionTextMesh.enabled = false;
    }

    public void OnVisionStart() {
        ShowActionText();
    }

    public void OnVisionEnd() {
        HideActionText();
    }

    public virtual void Interact(GameObject other) {
        Debug.Log("Interaction with item.");
    }
}
