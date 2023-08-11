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
    }

    public void AnimateBuffMagicArmor() {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("BuffMagicArmor");
    }

    public void AnimateSurprise() {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Surprise");
    }

    public void AnimateGesture1() {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Gesture1");
    }

    public void AnimateDeath() {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Death");
    }

    public void AnimateGetHit() {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("GetHit");
    }

    public void AnimateMeleeAttack() {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Attack");
    }

    public void AnimateGathering() {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Gathering");
    }

    public void AnimateSpellCast2() {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("SpellCast2");
    }

    public void AnimateCastingLoop() {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("CastingLoop");
    }

    public void AnimateBuff() {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Buff");
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
