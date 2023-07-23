using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private VfxWizard vfxWizard;
    private HumanAnimator humanAnimator;
    private int hitCount = 0;

    private void Start() {
        humanAnimator = GetComponent<HumanAnimator>();
        humanAnimator.AnimateRunning();
    }

    private void Update() {
        
    }

    public void TakeDamage(Vector3 hitDirection){
        var bloodSpillPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        vfxWizard.SummonBloodSpillEffect(bloodSpillPosition, Quaternion.LookRotation(hitDirection));
        if (hitCount == 3) {
            humanAnimator.AnimateDeath();
        } else {
            humanAnimator.AnimateGetHit();
            hitCount += 1;
        }
    }

    public enum EnemyState
    {
        Idle, Running, Casting, GettingHit, Death
    }
}
