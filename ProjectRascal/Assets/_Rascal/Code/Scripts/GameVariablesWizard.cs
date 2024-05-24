using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GvKey {
    none,
    letter1Read,
    villager1Talked,
    spirit1Talked,
    grondBeaten,
    hektorTalked,
    spirit2Talked
}

public class GameVariablesWizard : MonoBehaviour
{
    private List<GameVariable> gameVariables = new();

    public event Action<GvKey> GameVariableChanged;

    private void Start() {
        InitGameVariablesBasedOnKeys();
    }

    private void InitGameVariablesBasedOnKeys() {
        foreach (GvKey key in Enum.GetValues(typeof(GvKey))) {
            gameVariables.Add(new GameVariable { key = key, value = false });
        }
    }

    public void ActivateGameVariable(GvKey key) {
        var gameVariable = gameVariables.Find(x => x.key == key);
        if(gameVariable != null) {
            gameVariable.value = true;
            GameVariableChanged?.Invoke(key);
        }
    }

    public bool GetGameVariable(GvKey key) {
        if (key == GvKey.none) {
            return true;
        }

        var gameVariable = gameVariables.Find(x => x.key == key);
        if(gameVariable != null) {
            return gameVariable.value;
        }
        return false;
    }

    private void OnDestroy() {
        GameVariableChanged = null;
    }

    #region Singleton

    public static GameVariablesWizard instance;

    private void Awake() {
        instance = this;
    }

    private GameVariablesWizard() {

    }

    #endregion
}
 
[Serializable]
public class GameVariable
{
    public GvKey key;
    public bool value;
}