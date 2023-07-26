using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCanvas : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;
    [SerializeField] private Image healthBarBackgroundSprite;
    private Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
        healthBarSprite.fillAmount = 1;
    }

    private void Update() {
        UpdateRotation();
    }

    private void UpdateRotation() {
        var rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, 0f, 0f);
    }

    public void UpdateHealthBar(float current, float max) {
        healthBarSprite.fillAmount = current / max;
    }

    public void DisableHealthBar() {
        healthBarSprite.enabled = false;
        healthBarBackgroundSprite.enabled = false;
    }
}
