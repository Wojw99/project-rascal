using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private PlayerController playerController;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        UpdateSpeed();
        UpdateAttack();
    }

    public void AnimateAttack() {
        animator.SetTrigger("Attack");
    }

    private void UpdateSpeed() {
        var speedPercent = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
        animator.SetFloat("Speed", speedPercent, .1f, Time.deltaTime);
    }

    public void UpdateAttack() {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 0.8f) {
            playerController.StopAttack();
        }
    }
}
