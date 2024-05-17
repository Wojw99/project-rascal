using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Interactible : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] protected TextMeshProUGUI nameTextMesh;
    [SerializeField] protected TextMeshProUGUI actionTextMesh;

    protected void ParentStart() {
        HideActionText();
    }

    protected void InstantiateAtParentLocation(GameObject gameObject) {
        Instantiate(gameObject, transform.position, transform.rotation);
    }

    protected void InstantiateAsChild(GameObject gameObject) {
        Instantiate(gameObject, transform);
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
