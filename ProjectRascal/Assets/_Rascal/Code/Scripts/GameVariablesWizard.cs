using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameVariablesWizard : MonoBehaviour
{
    [SerializeField] private Dictionary<bool> variablesState;

    #region Singleton

    public static GameVariablesWizard instance;

    private void Awake() {
        instance = this;
    }

    private GameVariablesWizard() {

    }

    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameVariablesWizard))]
class GameVariablesWizardEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }
}
#endif