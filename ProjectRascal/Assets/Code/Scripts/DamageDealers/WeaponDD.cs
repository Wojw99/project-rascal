using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDD : DamageDealer
{
    private void Start() {
        DamageDealerStart();
    }

    private new void DealDamage() {
        finalDamage = damageAmount + ownerCharacter.Attack;
        base.DealDamage();
    }
}
