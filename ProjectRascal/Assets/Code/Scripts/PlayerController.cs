using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float minDistanceToMove = 0.2f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameInput gameInput;
    private Vector3 targetPosition = Vector3.zero;
    private bool isMoving = false; 
    
    private void Start() {
        targetPosition = transform.position;
    }

    private void Update() {
        MoveByMouse();
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
                }
            }
        }
        
        // Move the Player as long as its position is not equal to the targetPosition
        if(isMoving) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);

            if(transform.position == targetPosition) {
                isMoving = false;
            }
        }
    }

    public bool IsMoving
    {
        get { return isMoving; }
    }
}