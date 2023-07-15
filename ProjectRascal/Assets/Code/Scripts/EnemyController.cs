using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private VfxWizard vfxWizard;

    public void TakeDamage(Vector3 hitDirection){
        var bloodSpillPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        vfxWizard.SummonBloodSpillEffect(bloodSpillPosition, Quaternion.LookRotation(hitDirection));
    }
}
