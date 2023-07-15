using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public event Action<InputAction.CallbackContext> leftClickPerformed;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.LeftClick.performed += leftClickPerformed;
    }

    public bool GetLeftClick() {
        return playerInputActions.Player.LeftClick.IsPressed();
    }

    public bool GetRightClick() {
        return playerInputActions.Player.RightClick.IsPressed();
    }
}
