using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCanvas : MapCanvas
{
    [SerializeField] private string actorKey;
    [SerializeField] private Image healthBarSprite;
    [SerializeField] private Image healthBarBackgroundSprite;
    [SerializeField] private TextMeshProUGUI nameTextMesh;
    // private Camera mainCamera;

    private void Start() {
        // mainCamera = Camera.main;
        ParentStart();
        healthBarSprite.fillAmount = 1;
        nameTextMesh.text = StringsWizard.Instance.GetActorName(actorKey);
    }

    private void Update() {
        UpdateRotation();
    }

    public void UpdateHealthBar(float current, float max) {
        healthBarSprite.fillAmount = current / max;
    }

    public void DisableHealthBarAndName() {
        healthBarSprite.enabled = false;
        healthBarBackgroundSprite.enabled = false;
        nameTextMesh.enabled = false;
    }
}
