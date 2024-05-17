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
        GameVariablesWizard.instance.GameVariablesChanged += OnGameVariablesChanged;
    }

    public void OnInteractionEnd() {
        GameVariablesWizard.instance.ActivateGameVariable(activatedGameVariable);
    }

    private void OnGameVariablesChanged() {
        HandleActivision();
    }

    private void HandleActivision() {
        bool state = GameVariablesWizard.instance.GetGameVariable(requiredGameVariable);
        gameObject.SetActive(state);
    }

    private void OnDestroy() {
        GameVariablesWizard.instance.GameVariablesChanged -= OnGameVariablesChanged;
    }
}
