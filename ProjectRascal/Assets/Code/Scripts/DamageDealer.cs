using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] protected float damageAmount = 0f;
    [SerializeField] protected bool isBloodSpillVisible = true;
    protected Collider damageAreaCollider;
    protected float finalDamage = 0f;
    protected List<GameObject> injured;
    protected GameCharacter ownerCharacter;
    protected Transform startPoint;
    protected Transform endPoint;
    protected float damageDuration;

    protected void DamageDealerStart() {
        damageAreaCollider = GetComponent<Collider>();
        damageAreaCollider.enabled = false;
        injured = new List<GameObject>();
    }

    public void FeedAndDealDamage(GameCharacter ownerCharacter, float damageDuration = 1f, float damageStartTime = 0f, Transform startPoint = null, Transform endPoint = null) {
        this.ownerCharacter = ownerCharacter;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.damageDuration = damageDuration;
        Invoke("DealDamage", damageStartTime);
    }
    
    protected void DealDamage() {
        damageAreaCollider.enabled = true;
        injured.Clear();
        Invoke("EndDamage", damageDuration);
    }

    protected void EndDamage() => damageAreaCollider.enabled = false;

    protected void OnTriggerEnter(Collider other) {
        Debug.Log(other);
        if(!injured.Contains(other.gameObject)) {
            var character = other.GetComponent<GameCharacter>();
            var controller = other.GetComponent<IDamagaController>();

            injured.Add(other.gameObject);

            if(character != null) {
                character.TakeDamage(finalDamage);
            }

            if(controller != null) {
                controller.VisualizeDamage(transform.position, isBloodSpillVisible);
            }
        }
    }
}
