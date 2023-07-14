using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWizard : MonoBehaviour
{
    [SerializeField] private static GameObject testingSphere;

    public static void SummonTestingSphere(Vector3 position) {
        GameObject.Instantiate(testingSphere, position, Quaternion.identity);
    }
}
