using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputWizard : MonoBehaviour
{
    #region Singleton

    public static InputWizard instance;

    private void Awake() {
        instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private InputWizard() {
        
    }

    #endregion

    private PlayerInputActions playerInputActions;

    public bool IsLeftClickPressed() {
        return playerInputActions.Player.LeftClick.IsPressed();
    }

    public bool IsRightClickPressed() {
        return playerInputActions.Player.RightClick.IsPressed();
    }

    public bool IsRightClickJustPressed() {
        return Input.GetMouseButtonDown(1);
    }

    public bool IsLeftClickJustPressed() {
        return Input.GetMouseButtonDown(0);
    }

    public bool IsInteractionKeyPressed() {
        return Input.GetKeyDown(KeyCode.E);
    }
}
