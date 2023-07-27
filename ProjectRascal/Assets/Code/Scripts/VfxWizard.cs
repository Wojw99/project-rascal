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

    public void SummonFancyCircleEffect(Vector3 position) {
        GameObject.Instantiate(fancyCircleEffect, position, Quaternion.identity);
    }

    public void SummonBloodSpillEffect(Vector3 position, Quaternion rotation) {
        GameObject.Instantiate(bloodSpillEffect, position, rotation);
    }
}
