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
    [SerializeField] private GameObject testingSphere2;
    [SerializeField] private GameObject testingSphere3;

    public void SummonTestingSphere(Vector3 position) {
        GameObject.Instantiate(testingSphere, position, Quaternion.identity);
    }

    public void SummonTestingSphere2(Vector3 position) {
        GameObject.Instantiate(testingSphere2, position, Quaternion.identity);
    }

    public void SummonTestingSphere3(Vector3 position) {
        GameObject.Instantiate(testingSphere3, position, Quaternion.identity);
    }
}
