using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.LeftClick.canceled += OnLeftClickCanceled;
    }

    public bool GetLeftClick() {
        return playerInputActions.Player.LeftClick.IsPressed();
    }

    public bool GetRightClick() {
        return playerInputActions.Player.RightClick.IsPressed();
    }

    private void OnLeftClickCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Left mouse button released");
    }

}
