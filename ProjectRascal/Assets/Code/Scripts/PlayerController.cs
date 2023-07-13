using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    // * * * * * * SERIALIZED * * * * * *
    [SerializeField] private float moveSpeed = 70f;
    [SerializeField] private float minDistanceToMove = 0.2f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameInput gameInput;
    private Vector3 targetPosition = Vector3.zero;
    private bool isMoving = false; 
    private bool isAttacking = false; 
    private bool isCharging = false;
    [SerializeField] private float attackingDistance = 1f;
    [SerializeField] private float stoppingDistance = 0.5f;

    // * * * * * * PRIVATE * * * * * *
    private NavMeshAgent navMeshAgent;

    private void Start() {
        targetPosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        // MoveByMouse();
        // HandleAttack();
        MoveAgent();
        HandleStop();
    }

    private void MoveAgent() {
        if(gameInput.GetLeftClick() && !isCharging) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 
            if(Physics.Raycast(ray, out hit)) {
                navMeshAgent.destination = hit.point;
                isMoving = true;
                if(hit.collider.CompareTag("Enemy") && !isCharging) {
                    Debug.Log("Enemy clicked.");
                    isCharging = true;
                    navMeshAgent.stoppingDistance = attackingDistance;
                } else {
                    isCharging = false;
                    navMeshAgent.stoppingDistance = stoppingDistance;
                }
            }            
        }
    }

    private void HandleStop() {
        if (!isMoving) {
            return;
        }

        var distToDestination = Vector3.Distance(transform.position, navMeshAgent.destination);
        if (distToDestination <= navMeshAgent.stoppingDistance) {
            if (isCharging) {
                isCharging = false;
                isAttacking = true;
                isMoving = false;
            } else {
                isMoving = false;
            }
        } 
    }

    private void HandleAttack() {
        if(gameInput.GetRightClick()) {
            isAttacking = true;
            // isMoving = false;
        }
    }

    private void MoveByMouse() {
        if(gameInput.GetLeftClick()) {
            // Create the line of infinite length
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 

            // Check if the line collides with an object of the given tag
            if(Physics.Raycast(ray, out hit)) {
                if(hit.collider.CompareTag("Ground")) {
                    var distance = Vector3.Distance(transform.position, hit.point);
                    if(distance >= minDistanceToMove) {
                        targetPosition = hit.point;
                        isMoving = true;
                        transform.LookAt(targetPosition); // Rotate Player to the target
                    }
                } else {
                    Debug.Log(hit.collider.tag);
                }
            }
        }

        // HandleCollisions();
        
        
        // Move the Player as long as its position is not equal to the targetPosition
        if(isMoving) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            if(transform.position == targetPosition) {
                isMoving = false;
            }
        }
    }

    public void HandleCollisions() {
        var moveDir = (targetPosition - transform.position).normalized;
        var playerRadius = 0.5f;
        var playerHeight = 2f;
        var moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        
        if (!canMove) {
            // Cannot move towards moveDir

            // Attempt only x movement
            var moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                targetPosition = new Vector3(targetPosition.x, 0, 0);
            } else {
                var moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    targetPosition = new Vector3(0, 0, targetPosition.z);
                } else {
                    // Cannot move in any direction
                    isMoving = false;
                }
            }
        }
    }

    public bool IsMoving
    {
        get { return isMoving; }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }
}