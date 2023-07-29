using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealerWizard : MonoBehaviour
{
    #region Singleton

    public static DamageDealerWizard instance;

    private void Awake() {
        instance = this;
    }

    private DamageDealerWizard() {

    }

    #endregion

    [SerializeField] private GameObject thunderstruck;

    public GameObject SummonThunderstruck(Vector3 position) {
        return GameObject.Instantiate(thunderstruck, position, Quaternion.identity);
    }
}
