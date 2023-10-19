using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private List<string> actorNames;
    [SerializeField] private List<Sprite> actorFaces;

    private List<string> eventActions;
    private int currentActionIndex = 0;

    private ActionType currentAction = ActionType.none;

    private void Update() {
        EndActionM();
    }

    public bool IsOff() {
        return currentAction == ActionType.none;
    }

    public void PlayEvent(int eventId) {
        eventActions = LoadEventActions(eventId);
        currentActionIndex = -1;
        HandleNextAction();
    }

    private void HandleNextAction() {
        currentActionIndex += 1;
        if(currentActionIndex >= eventActions.Count) {
            currentAction = ActionType.none;
            return;
        }

        var actionOptions = eventActions[currentActionIndex].Split(";");
        var type = actionOptions[0];

        try {
            if (type == ActionType.m.ToString()) {
                StartActionM(actionOptions);
            } else if (type == ActionType.e.ToString()) {
                StartActionE(actionOptions);
            }
        } catch(Exception ex) {
            Debug.LogError(ex);
            HandleNextAction();
        }
    }

    private void StartActionE(string[] actionOptions) {
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

        //Debug.Log("change signal to " + signal);
        EventSignalizer.instance.CurrentSignal = signal;
        Invoke(nameof(EndActionE), waitTime);
    }

    private void EndActionE() {
        if(currentAction == ActionType.e) {
            HandleNextAction();
        }
    }

    private void EndActionM() {
        var isKeyPressed = InputWizard.instance.IsSpacePressed();
        if(currentAction == ActionType.m && isKeyPressed) {
            UIWizard.instance.HideMessage();
            HandleNextAction();
        }
    }

    private void StartActionM(string[] actionOptions) {
        currentAction = ActionType.m;

        if(actionOptions.Length < 4) {
            throw new Exception(GetNotInfoWarning(ActionType.m));
        }

        try {
            var nameIndex = int.Parse(actionOptions[1]);
            var faceIndex = int.Parse(actionOptions[2]);
            var text = actionOptions[3];

            var name = actorNames[nameIndex];
            var face = actorFaces[faceIndex];

            UIWizard.instance.ShowMessage(name, face, text);
        } catch {
            throw new Exception(GetInvalidFormatWarning(ActionType.m));
        }
    }

    private List<string> LoadEventActions(int eventId) {
        if(eventId == 0) {
            return new List<string>() {
                "m;2;2;Witaj, podróżniku!",
                "m;1;1;Pokaż mi swoje towary, kupcze.",
                "m;2;2;Ale ja nic nie sprzedaję. Jestem tylko prostym wędkarzem, panie.",
                "m;1;1;W takim razie powodzenia na łowach. Niech ci wędka lekką będzie. Żegnaj.",
                "m;2;2;Bywaj!",
            };
        } else {
            return new List<string>() {
                "m;2;2;To znowu ty? Czego znowu chcesz?",
                "m;1;1;Okłamałeś mnie! Nie możesz być wędkarzem, bo w tej grze jeszcze nie ma wody ani mechanizmu wędkowania!",
                "m;2;2;NIE!",
                "m;1;1;Ukaż mi swoją prawdziwą postać!",
                "e;e0;1.1",
                "m;0;0;Zginiesz, śmiertelna istoto!",
                "e;e1;0",
            };
        }
    }

    private string GetInvalidFormatWarning(ActionType actionType) {
        return $"Invalid format for an option of action type \"{actionType}\"!";
    }

    private string GetNotInfoWarning(ActionType actionType) {
        return $"Not enought information for action type \"{actionType}\"!";
    }

    enum ActionType {
        m, e, vfx, rotate, move, sound, music, animation, none
    }
}
