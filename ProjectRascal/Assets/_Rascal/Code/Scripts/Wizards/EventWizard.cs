using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class EventWizard : MonoBehaviour
{
    #region Singleton

    public static EventWizard instance;

    private void Awake() {
        instance = this;
    }

    private EventWizard() {

    }

    #endregion

    [SerializeField] List<Sprite> faces;

    List<string> currentDialog = new List<string>();
    int currentDialogIndex = 0;
    ActionType currentAction = ActionType.none;

    public event Action DialogEnd;

    void Update() {
        HandleEndOfMessage();
    }

    public bool IsOff() {
        return currentAction == ActionType.none;
    }

    public void PlayDialog(string dialogKey) {
        currentDialog = StringsWizard.Instance.Dialogs[dialogKey];
        currentDialogIndex = -1;
        HandleNextMessage();
    }

    void HandleNextMessage() {
        currentDialogIndex += 1;
        if(currentDialogIndex >= currentDialog.Count) {
            DialogEnd?.Invoke();
            currentAction = ActionType.none;
            return;
        }

        var dialogOptions = currentDialog[currentDialogIndex].Split(";");
        var type = dialogOptions[0];

        try {
            if (type == ActionType.m.ToString()) {
                StartMessage(dialogOptions);
            } else if (type == ActionType.e.ToString()) {
                StartAction(dialogOptions);
            }
        } catch(Exception ex) {
            Debug.LogError(ex);
            HandleNextMessage();
        }
    }

    void StartAction(string[] actionOptions) {
        currentAction = ActionType.e;

        if(actionOptions.Length < 3) {
            throw new Exception(GetNotInfoWarning(ActionType.e));
        }

        var signal = "";
        var waitTime = 0f;

        try {
            signal = actionOptions[1];
            waitTime = float.Parse(actionOptions[2].Replace('.', ','));
        } catch {
            throw new Exception(GetInvalidFormatWarning(ActionType.e));
        }

        Debug.Log("change signal to " + signal);
        EventSignalizer.instance.CurrentSignal = signal;
        Invoke(nameof(EndActionE), waitTime);
    }

    void EndActionE() {
        if(currentAction == ActionType.e) {
            HandleNextMessage();
        }
    }

    void HandleEndOfMessage() {
        var isKeyPressed = InputWizard.instance.IsSpacePressed();
        if(currentAction == ActionType.m && isKeyPressed) {
            UIWizard.instance.HideMessage();
            HandleNextMessage();
        }
    }

    void StartMessage(string[] actionOptions) {
        currentAction = ActionType.m;

        if(actionOptions.Length < 3) {
            throw new Exception(GetNotInfoWarning(ActionType.m));
        }

        try {
            var nameKey = actionOptions[1];
            var text = actionOptions[2];

            var name = StringsWizard.Instance.Actors[nameKey]["name"];
            var faceKey = StringsWizard.Instance.Actors[nameKey]["face"];
            var face = faces.Where(f => f.name == faceKey).FirstOrDefault();

            UIWizard.instance.ShowMessage(name, face, text);
        } catch {
            throw new Exception(GetInvalidFormatWarning(ActionType.m));
        }
    }

    string GetInvalidFormatWarning(ActionType actionType) {
        return $"Invalid format for an option of action type \"{actionType}\"!";
    }

    string GetNotInfoWarning(ActionType actionType) {
        return $"Not enought information for action type \"{actionType}\"!";
    }

    private void OnDestroy() {
        DialogEnd = null;
    }

    enum ActionType {
        m, e, vfx, rotate, move, sound, music, animation, none
    }
}
