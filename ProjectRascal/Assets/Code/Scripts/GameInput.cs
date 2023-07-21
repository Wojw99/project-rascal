using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public bool IsLeftClickPressed() {
        return playerInputActions.Player.LeftClick.IsPressed();
    }

    public bool IsRightClickPressed() {
        return playerInputActions.Player.RightClick.IsPressed();
    }

    public bool IsRightClickJustPressed() {
        return Input.GetMouseButtonDown(1);
    }
}
