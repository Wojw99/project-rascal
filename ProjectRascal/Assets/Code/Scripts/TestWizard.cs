using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWizard : MonoBehaviour
{
    #region Singleton

    public static TestWizard instance;

    private void Awake() {
        instance = this;
    }

    private TestWizard() {
        
    }

    #endregion

    [SerializeField] private GameObject testingSphere;

    public void SummonTestingSphere(Vector3 position) {
        GameObject.Instantiate(testingSphere, position, Quaternion.identity);
    }
}
