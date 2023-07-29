using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DecorationController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private VfxWizard vfxWizard;
    [SerializeField] private InputWizard gameInput;
    [SerializeField] private float groundClickEffectPauseTime = 0.15f;
    private bool groundClickEffectEnabled = true;

    private void Awake() {
        // gameInput.leftClickPerformed += OnLeftClickPerformed;
    }

    private void OnLeftClickPerformed(InputAction.CallbackContext context)
    {
        // Debug.Log("LCP");
        // if (!groundClickEffectEnabled) return;
        // var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // if(Physics.Raycast(ray, out RaycastHit hit)) {
        //     if(TagWizard.IsGround(hit.collider)) {
        //         var clickPosition = hit.point;
        //         PerformGroundClickEffect(clickPosition);
        //         Debug.Log("Perform ground click");
        //     }
        // }
    }

    private void PerformGroundClickEffect(Vector3 clickPosition) {
        vfxWizard.SummonFancyCircleEffect(clickPosition); 
        DisableGroundClickEffect();
        Invoke("EnableGroundClickEffect", groundClickEffectPauseTime);
    }

    private void DisableGroundClickEffect() {
        groundClickEffectEnabled = false;
    }

    private void EnableGroundClickEffect() {
        groundClickEffectEnabled = true;
    }
}
