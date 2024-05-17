using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogInvoker : MonoBehaviour
{
    [SerializeField] private string dialogKey;
    [SerializeField] private float timeOffset = 0f;

    private void OnDialogEnd() {
        EventWizard.instance.DialogEnd -= OnDialogEnd;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent(out PlayerCharacter playerCharacter)) {
            StartCoroutine(PlayDialog());
        }
    }

    private IEnumerator PlayDialog() {
        yield return new WaitForSeconds(timeOffset);
        EventWizard.instance.PlayDialog(dialogKey);
        EventWizard.instance.DialogEnd += OnDialogEnd;
    }
}
