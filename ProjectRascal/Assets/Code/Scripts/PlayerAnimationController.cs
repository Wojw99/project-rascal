using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private const string IS_RUNNING = "IsRunning";

    private Animator animator;
    [SerializeField] private PlayerController playerController;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        animator.SetBool(IS_RUNNING, playerController.IsMoving);
    }
}
