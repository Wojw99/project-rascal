using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCanvas : MonoBehaviour
{
    private Camera mainCamera;

    public void ParentStart() {
        mainCamera = Camera.main;
    }

    protected void UpdateRotation() {
        var rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, 0f, 0f);
    }
}
