using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIWizard : MonoBehaviour
{
    #region Singleton

    public static UIWizard instance;

    private void Awake() {
        instance = this;
    }

    private UIWizard() {

    }

    #endregion

    [SerializeField] private Image hpBarSprite;
    [SerializeField] private Image hpBarBackgroundSprite;
    [SerializeField] private Image mpBarSprite;
    [SerializeField] private Image mpBarBackgroundSprite;

    [SerializeField] private TextMeshProUGUI goldTextMesh;

    [SerializeField] private TextMeshProUGUI writingTextMesh;
    [SerializeField] private GameObject writingContainer;

    [SerializeField] private TextMeshProUGUI messageTextMesh;
    [SerializeField] private TextMeshProUGUI nameTextMesh;
    [SerializeField] private Image faceImage;
    [SerializeField] private GameObject messageContainer;

    public event Action WrittingHide;

    private void Start() {
        hpBarSprite.fillAmount = 1;
        mpBarSprite.fillAmount = 1;
        goldTextMesh.text = "0";
        HideWriting();
        HideMessage();
    }

    public void ShowMessage(string name, Sprite faceSprite, string text) {
        nameTextMesh.text = name;
        messageTextMesh.text = text;
        faceImage.sprite = faceSprite;
        messageContainer.SetActive(true);
    }

    public void HideMessage() {
        nameTextMesh.text = string.Empty;
        messageTextMesh.text = string.Empty;
        messageContainer.SetActive(false);
    }

    public void ShowWriting(string text) {
        writingTextMesh.text = text;
        writingContainer.SetActive(true);
    }

    public void HideWriting() {
        writingContainer.SetActive(false);
        WrittingHide?.Invoke();
    }

    public void UpdateHpBar(float current, float max) {
        hpBarSprite.fillAmount = current / max;
    }

    public void UpdateMpBar(float current, float max) {
        mpBarSprite.fillAmount = current / max;
    }

    public void UpdateGold(string current) {
        goldTextMesh.text = current;
    }

    private void OnDestroy() {
        WrittingHide = null;
    }
}
