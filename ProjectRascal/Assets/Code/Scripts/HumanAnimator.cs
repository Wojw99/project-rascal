using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAnimator : MonoBehaviour
{
    // private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        // navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // UpdateSpeed();
        StayAtLastFrameOnDeath();
    }

    private void StayAtLastFrameOnDeath() {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        // if (state.IsName("Death") && state.normalizedTime >= 0.99f) {
        //     Debug.Log("is last death frame");
        //     var layer = -1;
        //     var normalizedTime = 1f;
        //     animator.Play(state.shortNameHash, layer, normalizedTime);
        //     animator.speed = 0f; 
        // }
    }

    public void AnimateDeath() {
        animator.SetTrigger("Death");
    }

    public void AnimateGetHit() {
        animator.SetTrigger("GetHit");
    }

    public void AnimateMeleeAttack() {
        animator.SetTrigger("Attack");
    }

    public void AnimateRunning() {
        animator.SetFloat("Speed", 1f, .1f, Time.deltaTime);
    }

    public void AnimateIdle() {
        animator.SetFloat("Speed", 0f, .1f, Time.deltaTime);
    }

    // private void UpdateSpeed() {
    //     var speedPercent = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
    //     animator.SetFloat("Speed", speedPercent, .1f, Time.deltaTime);
    // }
}
