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
        animator.SetBool(IS_RUNNING, playerController.IsMoving);
        animator.SetBool(IS_ATTACKING_ONE_HANDED, playerController.IsAttacking);
    }

    public void OnOneHandedMeleeAttackEnd() {
        playerController.IsAttacking = false;
    }
}
