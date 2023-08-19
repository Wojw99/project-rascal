using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class NecroSlashDD : DamageDealer
{
    [SerializeField] private float moveSpeed = 24f;
    [SerializeField] private float slashLifetime = 0.75f;
    [SerializeField] private VisualEffect vfx;

    private ExposedProperty rotationAttr = "Rotation";

    private void Start() {
        DamageDealerStart();
    }

    private void Update() {
        var velocityVector = new Vector3(transform.forward.x, 0f, transform.forward.z);
        transform.position += velocityVector * moveSpeed * Time.deltaTime;;
    }

    protected override void Prepare()
    {
        base.Prepare();
        Debug.Log("Set rotation to " + transform.rotation.eulerAngles);
        vfx.SetVector3("Rotation", transform.rotation.eulerAngles);
        vfx.SetFloat("Lifetime", slashLifetime);
    }

}
