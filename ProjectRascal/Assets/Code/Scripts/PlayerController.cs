using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float minDistanceToMove = 0.2f;
    [SerializeField] private Camera mainCamera;
    private Vector3 targetPosition = Vector3.zero;
    private bool isMoving = false;
    private Vector3 cameraOffset = Vector3.zero;
    
    private void Start() {
        cameraOffset = mainCamera.transform.position;
        targetPosition = transform.position;
        var cameraData = mainCamera.GetUniversalAdditionalCameraData();
        cameraData.enabled = false;
    }

    private void Update() {
        // MoveByKeyboard();
        MoveByMouse();
    }

    private void MoveByKeyboard() {
        var inputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W)) {
            inputVector.y = +1;
        } else if (Input.GetKey(KeyCode.S)) {
            inputVector.y = -1;
        } else if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1;
        } else if (Input.GetKey(KeyCode.D)) {
            inputVector.x = +1;
        }

        inputVector = inputVector.normalized;

        var moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void MoveByMouse() {
        if(Input.GetMouseButton(0)) {
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
            MoveCamera();

            if(transform.position == targetPosition) {
                isMoving = false;
            }
        }
    }

    private void MoveCamera() {
        var cameraOffset = new Vector3(0f, 11.4f, -4.64f);
        var nextCameraPosition = targetPosition + cameraOffset;
        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, nextCameraPosition, Time.deltaTime * moveSpeed);;
    }
}
