using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIWizard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI writingTextMesh;
    [SerializeField] private GameObject writingContainer;

    [SerializeField] private TextMeshProUGUI messageTextMesh;
    [SerializeField] private TextMeshProUGUI nameTextMesh;
    [SerializeField] private Image faceImage;
    [SerializeField] private GameObject messageContainer;

    public event Action WritingHide;
    public event Action WritingShowed;
    public event Action MessageShowed;
    public event Action MessageHide;

    private void Start() {
        HideWriting();
        HideMessage();
    }

    public void ShowMessage(string name, Sprite faceSprite, string text) {
        nameTextMesh.text = name;
        messageTextMesh.text = text;
        faceImage.sprite = faceSprite;
        messageContainer.SetActive(true);
        MessageShowed?.Invoke();
        UIHandler.HideCharacterUi = true;
    }

    public void HideMessage() {
        nameTextMesh.text = string.Empty;
        messageTextMesh.text = string.Empty;
        messageContainer.SetActive(false);
        MessageHide?.Invoke();
        UIHandler.HideCharacterUi = false;
    }

    public void ShowWriting(string text) {
        writingTextMesh.text = text;
        writingContainer.SetActive(true);
        WritingShowed?.Invoke();
        UIHandler.HideCharacterUi = true;
    }

    public void HideWriting() {
        writingContainer.SetActive(false);
        WritingHide?.Invoke();
        UIHandler.HideCharacterUi = false;
    }

    private void OnDestroy() {
        WritingHide = null;
    }

    #region Singleton

    public static UIWizard instance;

    private void Awake() {
        instance = this;
    }

    private UIWizard() {

    }

    #endregion
}
