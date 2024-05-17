using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactible
{
    [SerializeField] private GameObject dummyVisual;

    [Header("Content")]
    [SerializeField] private NpcSO npcSO;

    private bool dialogShowed = false;
    private bool isInteracting = false;

    private void Start() {
        ParentStart();
        SetupModel();
        nameTextMesh.text = StringsWizard.Instance.GetActorName(npcSO.nameTextKey);
        actionTextMesh.text = StringsWizard.Instance.GetText(npcSO.actionTextKey);
        
    }

    private void OnDialogEnd() {
        EventWizard.instance.DialogEnd -= OnDialogEnd;
        dialogShowed = true;
        if(npcSO.spawnEnemyAfter) {
            Instantiate(npcSO.enemyPrefab, transform.position, transform.rotation);
            Destroy(transform.gameObject);
        }
    }

    private void SetupModel() {
        var npcVisual = npcSO.npcVisual;
        if(npcVisual != null) {
            npcVisual.transform.position = Vector3.zero;
            npcVisual.transform.rotation = Quaternion.identity;
            Instantiate(npcVisual, transform);
            Destroy(dummyVisual);
        }
    }

    public override void Interact(GameObject other) {
        if(other.TryGetComponent(out PlayerCharacter playerCharacter)) {
            if(!dialogShowed) {
                EventWizard.instance.PlayDialog(npcSO.dialogKey);
            } else if(dialogShowed && npcSO.showDialogAfter) {
                EventWizard.instance.PlayDialog(npcSO.dialogAfterKey);
            }
            EventWizard.instance.DialogEnd += OnDialogEnd; 
        }
    }
}