using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(Collider))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/Misc/Projectile")]
    public class Projectile : MonoBehaviour
    {
        [Tooltip("The maximum distance this Projectile can reach.")]
        public float maxDistance = 15f;

        protected int m_damage;
        protected bool m_critical;
        protected string m_target;

        protected Vector3 m_origin;

        protected Entity m_entity;
        protected Entity m_otherEntity;
        protected Destructible m_destructible;

        /// <summary>
        /// Sets the damage data for this Projectile.
        /// </summary>
        /// <param name="entity">The Entity casting this Projectile.</param>
        /// <param name="damage">The amount of damage points.</param>
        /// <param name="critical">If true, the Projectile damage will be considered critical.</param>
        /// <param name="target">The tag of the target for this Projectile to interact with.</param>
        public virtual void SetDamage(Entity entity, int damage, bool critical, string target)
        {
            m_entity = entity;
            m_damage = damage;
            m_critical = critical;
            m_target = target;
        }

        protected virtual void Update()
        {
            transform.position += transform.forward * 15f * Time.deltaTime;

            if (Vector3.Distance(m_origin, transform.position) >= maxDistance)
                Destroy(gameObject);
        }

        protected virtual void OnEnable() => m_origin = transform.position;

        protected virtual void HandleEntityAttack(Collider other)
        {
            if (other.CompareTag(m_target) &&
                other.TryGetComponent(out m_otherEntity))
            {
                gameObject.SetActive(false);
                m_otherEntity.Damage(m_entity, m_damage, m_critical);
                Destroy(gameObject);
            }
        }

        protected virtual void HandleDestructibleAttack(Collider other)
        {
            if (m_entity.CompareTag(GameTags.Player) &&
                other.CompareTag(GameTags.Destructible) &&
                other.TryGetComponent(out m_destructible))
            {
                m_destructible.Damage(m_damage);
                Destroy(gameObject);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            HandleEntityAttack(other);
            HandleDestructibleAttack(other);
        }
    }
}
