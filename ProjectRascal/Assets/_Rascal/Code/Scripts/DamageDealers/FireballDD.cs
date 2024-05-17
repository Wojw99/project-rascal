using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballDD : DamageDealer
{
    [SerializeField] private float moveSpeed = 24f;

    private void Start() {
        DamageDealerStart();
    }

    private void Update() {
        var velocityVector = new Vector3(transform.forward.x, 0f, transform.forward.z);
        transform.position += velocityVector * moveSpeed * Time.deltaTime;;
    }

    private new void DealDamage() {
        finalDamage = damageAmount + ownerCharacter.Magic;
        base.DealDamage();
    }

    protected override void Prepare()
    {
        base.Prepare();
        var forwardVector = new Vector3(transform.forward.x, 0f, transform.forward.z) * 0.5f;
        VfxWizard.instance.SummonFireballStartEffect(transform.position + forwardVector, transform.rotation);
    }

    protected override void OnDamageEnd()
    {
        base.OnDamageEnd();
        Destroy(transform.gameObject);
        var position = new Vector3(transform.position.x, -0.5f, transform.position.z);
        var forwardVector = new Vector3(transform.forward.x, 0f, transform.forward.z) * 0.8f;
        Debug.Log(position);
        VfxWizard.instance.SummonFireballExplosionEffect(position + forwardVector, transform.rotation);
    }

    protected override void OnInvalidDamageTarget()
    {
        base.OnDamageEnd();
    }
}
