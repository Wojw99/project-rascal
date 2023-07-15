using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWizard : MonoBehaviour
{
    [SerializeField] private GameObject testingSphere;

    public void SummonTestingSphere(Vector3 position) {
        GameObject.Instantiate(testingSphere, position, Quaternion.identity);
    }
}
