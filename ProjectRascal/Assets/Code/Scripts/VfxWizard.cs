using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VfxWizard : MonoBehaviour
{
    [SerializeField] private VisualEffect groundClickEffect;
    [SerializeField] private VisualEffect bloodSpillEffect;

    public void SummonGroundClickEffect(Vector3 position) {
        GameObject.Instantiate(groundClickEffect, position, Quaternion.identity);
    }

    public void SummonBloodSpillEffect(Vector3 position, Quaternion rotation) {
        GameObject.Instantiate(bloodSpillEffect, position, rotation);
    }
}
