using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Writing : Interactible
{
    [Header("Content")]
    [SerializeField] private string nameTextKey;
    [SerializeField] private string actionTextKey;
    [SerializeField] private string writingContentKey;

    [Header("After reading")]
    [SerializeField] private bool dialogAfterReading = false;
    [SerializeField] private string dialogKey;

    private void Start() {
        ParentStart();
        nameTextMesh.text = StringsWizard.Instance.GetText(nameTextKey);
        actionTextMesh.text = StringsWizard.Instance.GetText(actionTextKey);
    }

    private void OnHideWriting() {
        UIWizard.instance.WrittingHide -= OnHideWriting;

        if(dialogAfterReading) {
            EventWizard.instance.PlayDialog(dialogKey);
        }
    }

    public override void Interact(GameObject other) {
        if(other.TryGetComponent(out PlayerController playerController)) {
            var writing = StringsWizard.Instance.GetText(writingContentKey);
            UIWizard.instance.ShowWriting(writing);
            UIWizard.instance.WrittingHide += OnHideWriting;
        }
    }
}
