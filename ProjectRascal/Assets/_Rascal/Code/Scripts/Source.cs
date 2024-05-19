using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : Interactible
{
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject dummyVisual;
    [SerializeField] private string nameTextKey;
    [SerializeField] private string actionTextKey = "draw";
    [SerializeField] private SourceType sourceType = SourceType.AbyssEnergy;
    [SerializeField] private int valueGained = 1;

    private bool drawed = false;

    private void Start() {
        ParentStart();
        nameTextMesh.text = StringsWizard.Instance.GetText(nameTextKey);
        actionTextMesh.text = StringsWizard.Instance.GetText(actionTextKey);
        InitVisual();
    }

    private void InitVisual() {
        if(visual != null) {
            visual.transform.position = Vector3.zero;
            visual.transform.rotation = Quaternion.Euler(-90, 0, 0);
            Instantiate(visual, transform);
        }
        Destroy(dummyVisual);
    }

    public override void Interact(GameObject other) {
        if(other.TryGetComponent(out PlayerCharacter playerCharacter)) {
            if(!drawed) {
                HandleValueGained(playerCharacter);
                drawed = true;
                GetComponentInChildren<Canvas>().enabled = false;
            }
        }
    }

    private void HandleValueGained(PlayerCharacter playerCharacter) {
        if(sourceType == SourceType.AbyssEnergy) {
            playerCharacter.AddAbyssEnergy(valueGained);
        } else {
            playerCharacter.AddAbyssKnowledge(valueGained);
        }
    }

    public enum SourceType {
        AbyssEnergy,
        AbyssKnowledge
    }
}