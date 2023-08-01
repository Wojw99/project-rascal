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

    public bool IsKey1Pressed() {
        return Input.GetKeyDown(KeyCode.Alpha1);
    }

    public bool IsKey2Pressed() {
        return Input.GetKeyDown(KeyCode.Alpha2);
    }

    public bool IsKey3Pressed() {
        return Input.GetKeyDown(KeyCode.Alpha3);
    }

    public bool IsKey4Pressed() {
        return Input.GetKeyDown(KeyCode.Alpha4);
    }

    public bool IsKey5Pressed() {
        return Input.GetKeyDown(KeyCode.Alpha5);
    }

    public bool IsEscPressed() {
        return Input.GetKeyDown(KeyCode.Escape);
    }
}
