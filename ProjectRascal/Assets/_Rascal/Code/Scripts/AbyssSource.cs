using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssSource : Interactible
{
    [SerializeField] private int abyssKnowledge = 0;
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject dummyVisual;
    [SerializeField] private string nameTextKey;
    [SerializeField] private string actionTextKey = "draw";

    private bool drawed = false;

    private void Start() {
        ParentStart();
        nameTextMesh.text = StringsWizard.Instance.GetText(nameTextKey);
        actionTextMesh.text = StringsWizard.Instance.GetText(actionTextKey);

        visual.transform.position = Vector3.zero;
        visual.transform.rotation = Quaternion.Euler(-90, 0, 0);
        Instantiate(visual, transform);
        Destroy(dummyVisual);
    }

    public override void Interact(GameObject other) {
        if(other.TryGetComponent(out PlayerCharacter playerCharacter)) {
            if(!drawed) {
                playerCharacter.AddAbyssKnowledge(abyssKnowledge);
                drawed = true;
                GetComponentInChildren<Canvas>().enabled = false;
            }
        }
    }
}
