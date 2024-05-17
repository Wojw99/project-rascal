using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderstruckDD : DamageDealer
{
    private void Start() {
        DamageDealerStart();
    }

    private new void DealDamage() {
        finalDamage = damageAmount + ownerCharacter.Magic;
        base.DealDamage();
    }
}
