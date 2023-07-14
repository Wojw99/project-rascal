using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    // * * * * * * SERIALIZED * * * * * *
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float attackingDistance = 1f;
    [SerializeField] private float stoppingDistance = 0.5f;

    // * * * * * * PRIVATE * * * * * *
    private NavMeshAgent navMeshAgent;
    // private bool isMoving = false; 
    // private bool isAttacking = false; 
    // private bool isCharging = false;
    private MovableState playerState = MovableState.Idle;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        // MoveByMouse();
        // HandleAttack();
        HandleMovement();
        HandleStop();
    }

    private void HandleMovement() {
        if(gameInput.GetLeftClick()) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 
            if(Physics.Raycast(ray, out hit)) {
                if(hit.collider.CompareTag("Enemy")) {
                    // Clicked for attack
                    navMeshAgent.destination = hit.collider.transform.position;
                    navMeshAgent.stoppingDistance = attackingDistance;
                    if(canMove()) {
                        playerState = MovableState.Charge;
                    } else {
                        playerState = MovableState.Attack;
                    }
                } else {
                    // Clicked for movement
                    navMeshAgent.destination = hit.point;
                    navMeshAgent.stoppingDistance = stoppingDistance;
                    playerState = MovableState.Move;
                }
            }            
        }
    }

    private void HandleStop() {
        if (playerState == MovableState.Charge || playerState == MovableState.Move) {
            if (!canMove()) {
                if (playerState == MovableState.Charge) {
                    playerState = MovableState.Attack;
                } else {
                    playerState = MovableState.Idle;
                }
            }
        }
    }

    public void StopAttack() {
        if(playerState == MovableState.Attack) {
            playerState = MovableState.Idle;
        }
    }

    private bool canMove() {
        var distToDestination = Vector3.Distance(transform.position, navMeshAgent.destination);
        return distToDestination > navMeshAgent.stoppingDistance;
    }

    // public bool IsMoving
    // {
    //     get { return isMoving; }
    // }

    // public bool IsAttacking
    // {
    //     get { return isAttacking; }
    //     set { isAttacking = value; }
    // }

    public MovableState PlayerState {
        get { return playerState; }
        set { playerState = value; }
    }

    public enum MovableState
    {
        Idle, Charge, Move, Attack
    }
}