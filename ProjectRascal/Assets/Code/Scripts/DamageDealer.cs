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
    protected Vector3 endPoint;
    protected float damageDuration;
    [SerializeField] protected DamageTarget damageTarget = DamageTarget.Character;

    protected void DamageDealerStart() {
        damageAreaCollider = GetComponent<Collider>();
        damageAreaCollider.enabled = false;
        injured = new List<GameObject>();
    }

    public void FeedAndDealDamage(GameCharacter ownerCharacter, float damageDuration = 1f, float damageStartTime = 0f, float lifetime = 3f) {
        this.ownerCharacter = ownerCharacter;
        this.damageDuration = damageDuration;
        Invoke("DealDamage", damageStartTime);
        Prepare();
    }

    public void SetLifetime(float lifetime = 6f) {
        Destroy(transform.gameObject, lifetime);
    }

    public void FeedAndDealDamage(GameCharacter ownerCharacter, Vector3 endPoint, float damageDuration = 1f, float damageStartTime = 0f, float lifetime = 3f) {
        this.endPoint = endPoint;
        FeedAndDealDamage(ownerCharacter, damageDuration, damageStartTime, lifetime);
    }

    protected virtual void Prepare() { }
    protected virtual void OnDamageEnd() { }
    protected virtual void OnInvalidDamageTarget() { }
    
    protected void DealDamage() {
        damageAreaCollider.enabled = true;
        injured.Clear();
        Invoke("EndDamage", damageDuration);
    }

    protected void EndDamage() => damageAreaCollider.enabled = false;

    protected void OnTriggerEnter(Collider other) {
        if(!injured.Contains(other.gameObject)) {
            var character = other.GetComponent<GameCharacter>();

            if(IsValidDamageTarget(character)) {
                injured.Add(other.gameObject);
                character.TakeDamage(finalDamage);

                var controller = other.GetComponent<IDamagaController>();
                if(controller != null) {
                    controller.VisualizeDamage(transform.position, isBloodSpillVisible);
                }

                OnDamageEnd();
            } else {
                OnInvalidDamageTarget();
            }
        }
    }

    private bool IsValidDamageTarget(GameCharacter character) {
        if(damageTarget == DamageTarget.Player && character is PlayerCharacter) {
            return true;
        }

        if(damageTarget == DamageTarget.Enemy && character is EnemyCharacter) {
            return true;
        }

        if(damageTarget == DamageTarget.Character) {
            return true;
        }

        return false;
    }

    public enum DamageTarget {
        Player, Enemy, Character
    }
}
