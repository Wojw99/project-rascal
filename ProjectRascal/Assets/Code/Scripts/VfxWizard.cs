using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VfxWizard : MonoBehaviour
{
    #region Singleton

    public static VfxWizard instance;

    private void Awake() {
        instance = this;
    }

    private VfxWizard() {

    }

    #endregion

    [SerializeField] private GameObject fancyCircleEffect;
    [SerializeField] private GameObject bloodSpillEffect;
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject thunderstruck;

    public void SummonFancyCircleEffect(Vector3 position) {
        GameObject.Instantiate(fancyCircleEffect, position, Quaternion.identity);
    }

    public void SummonBloodSpillEffect(Vector3 position, Quaternion rotation) {
        GameObject.Instantiate(bloodSpillEffect, position, rotation);
    }

    public void SummonHealEffect(Vector3 position, Transform parent) {
        GameObject.Instantiate(healEffect, position, Quaternion.identity, parent);
    }

    public void SummonThunderstruck(Vector3 position, Quaternion rotation) {
        GameObject.Instantiate(thunderstruck, position, rotation);
    }
}
