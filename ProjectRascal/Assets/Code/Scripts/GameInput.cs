using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public bool GetLeftClick() {
        return playerInputActions.Player.LeftClick.IsPressed();
    }

    public bool GetRightClick() {
        return playerInputActions.Player.RightClick.IsPressed();
    }
}
