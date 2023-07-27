using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamagaController
{
    [SerializeField] private float detectionDistance;
    [SerializeField] private float attackDistance;
    private CharacterCanvas characterCanvas;
    private HumanAnimator humanAnimator;
    private GameCharacter gameCharacter;
    private NavMeshAgent navMeshAgent;
    private CharacterState characterState = CharacterState.Idle;
    private GameObject chasingTarget;
    private DamagableWeapon damagableWeapon;

    private void Start() {
        humanAnimator = GetComponent<HumanAnimator>();
        gameCharacter = GetComponent<GameCharacter>();
        characterCanvas = GetComponentInChildren<CharacterCanvas>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        damagableWeapon = GetComponentInChildren<DamagableWeapon>();
        Debug.Log(humanAnimator.ToString());
        Debug.Log(gameCharacter.ToString());
        Debug.Log(characterCanvas.ToString());
    }

    private void Update() {
        if(HandleDeath()) return;
        HandleRotation();
        HandleChasing();
    }

    private bool HandleDeath() {
        if(gameCharacter.IsDead()) {
            characterState = CharacterState.Death;
            humanAnimator.AnimateDeath();
            if(TryGetComponent<BoxCollider>(out BoxCollider boxCollider)) {
                boxCollider.enabled = false;
            }
            navMeshAgent.enabled = false;
            return true;
        }
        return false;
    }

    private void HandleRotation() {
        if(characterState != CharacterState.Casting && chasingTarget != null) {
            transform.LookAt(chasingTarget.transform);
        }
    }

    private void HandleChasing() {
        if(characterState != CharacterState.Casting && chasingTarget != null) {
            var distanceToTarget = Vector3.Distance(transform.position, chasingTarget.transform.position);
            if(distanceToTarget <= attackDistance) {
                characterState = CharacterState.Casting;
                navMeshAgent.isStopped = true;
                humanAnimator.AnimateMeleeAttack();
                Invoke("EnforceDamage", 0.8f);
                return;
            }
        }

        var colliders = Physics.OverlapSphere(transform.position, detectionDistance, LayerMask.GetMask("Player"));
        if(characterState != CharacterState.Casting && colliders.Length > 0) {
            var newTarget = colliders[0].gameObject;

            if(chasingTarget == null || Vector3.Distance(transform.position, chasingTarget.transform.position) > attackDistance) {
                navMeshAgent.destination = newTarget.transform.position;
                navMeshAgent.isStopped = false;
                chasingTarget = newTarget;
            }

            characterState = CharacterState.Chasing;
            humanAnimator.AnimateRunning();
            return;
        }

        humanAnimator.AnimateIdle();
        chasingTarget = null;
    }

    private void EnforceDamage()
    {
        if(characterState != CharacterState.Death) {
            characterState = CharacterState.Idle;
            damagableWeapon.TakeDamage(gameCharacter.Attack);
        }
    }

    private void HandleAttack() {
        
    }

    private void HandleIdle() {

    }

    public void VisualizeDamage(Vector3 hitDirection, bool bloodSpill = true){
        if(bloodSpill) {
            var bloodSpillPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            VfxWizard.instance.SummonBloodSpillEffect(bloodSpillPosition, Quaternion.LookRotation(hitDirection));
        }
        if (gameCharacter.IsDead()) {
            humanAnimator.AnimateDeath();
            characterCanvas.DisableHealthBar();
        } else {
            // humanAnimator.AnimateGetHit();
            characterCanvas.UpdateHealthBar(gameCharacter.CurrentHealth, gameCharacter.MaxHealth);
        }
    }

    public enum CharacterState
    {
        Idle, Chasing, Casting, GettingHit, Death
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }
}