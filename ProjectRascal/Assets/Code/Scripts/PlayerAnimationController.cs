using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private const string IS_RUNNING = "IsRunning";
    private const string IS_ATTACKING_ONE_HANDED = "IsOneHandedMeleeAttack";

    private Animator animator;
    [SerializeField] private PlayerController playerController;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        animator.SetBool(IS_RUNNING, playerController.PlayerState == PlayerController.MovableState.Move || playerController.PlayerState == PlayerController.MovableState.Charge);
        animator.SetBool(IS_ATTACKING_ONE_HANDED, playerController.PlayerState == PlayerController.MovableState.Attack);
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var animation = stateInfo.IsName("Base Layer.YourAnimationName") ? "YourAnimationName" : "";

        Debug.Log(animation);
    }

    public void OnOneHandedMeleeAttackEnd() {
        playerController.StopAttack();
    }
}
