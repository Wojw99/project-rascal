using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VisibilityHandler : MonoBehaviour
{
    [Header("Game Variables")]
    [SerializeField] private GvKey requiredGameVariable = GvKey.none;
    [SerializeField] private GvKey activatedGameVariable = GvKey.none;

    private void Start() {
        HandleActivision();
        GameVariablesWizard.instance.GameVariableChanged += OnGameVariableChanged;
    }

    public void OnInteractionEnd() {
        GameVariablesWizard.instance.ActivateGameVariable(activatedGameVariable);
    }

    private void OnGameVariableChanged(GvKey gvKey) {
        HandleActivision();
    }

    private void HandleActivision() {
        bool state = GameVariablesWizard.instance.GetGameVariable(requiredGameVariable);
        gameObject.SetActive(state);
    }

    private void OnDestroy() {
        GameVariablesWizard.instance.GameVariableChanged -= OnGameVariableChanged;
    }
}
