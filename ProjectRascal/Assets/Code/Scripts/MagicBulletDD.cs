using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBulletDD : DamageDealer
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
    }

    protected override void OnDamageEnd()
    {
        base.OnDamageEnd();
        Destroy(transform.gameObject);
    }

    protected override void OnInvalidDamageTarget()
    {
        base.OnDamageEnd();
        Destroy(transform.gameObject);
    }
}
