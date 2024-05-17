using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactible
{
    [SerializeField] private GameObject dummyVisual;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Scriptable Objects")]
    [SerializeField] private NpcSO npcSO;
    [SerializeField] private EnemySO enemySO;

    private bool dialogShowed = false;

    private void Start() {
        ParentStart();
        SetupModel();
        nameTextMesh.text = StringsWizard.Instance.GetActorName(npcSO.nameTextKey);
        actionTextMesh.text = StringsWizard.Instance.GetText(npcSO.actionTextKey);
        EventWizard.instance.DialogEnd += OnDialogEnd;
    }

    private void OnDialogEnd() {
        dialogShowed = true;
        if(npcSO.spawnEnemyAfter) {
            var enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            enemy.GetComponent<EnemyCharacter>()?.SetupWithNewData(enemySO);
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
        }
    }
    private void OnDestroy() {
        EventWizard.instance.DialogEnd -= OnDialogEnd;
    }
}