using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VfxWizard : MonoBehaviour
{
    [SerializeField] private VisualEffect groundClickEffect;

    public void SummonGroundClickEffect(Vector3 position) {
        GameObject.Instantiate(groundClickEffect, position, Quaternion.identity);
    }
}
