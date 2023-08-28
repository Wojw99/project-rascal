using UnityEngine;
using System.Collections;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(Collider))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity Hitbox")]
    public class EntityHitbox : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("The duration in seconds to disable the Hitbox when it gets toggled.")]
        public float defaultToggleDuration = 0.1f;

        protected int m_damage;
        protected bool m_critical;

        protected Entity m_entity;
        protected Entity m_otherEntity;
        protected Destructible m_destructible;
        protected Collider m_collider;

        protected virtual void InitializeEntity() => m_entity = GetComponentInParent<Entity>();

        protected virtual void InitializeCollider()
        {
            m_collider = GetComponent<Collider>();
            m_collider.isTrigger = true;
        }

        /// <summary>
        /// Sets the damage information of this Hitbox.
        /// </summary>
        /// <param name="amount">The amount of damage points.</param>
        /// <param name="critical">Set it to true if the damage is critical.</param>
        public virtual void SetDamage(int amount, bool critical)
        {
            m_damage = amount;
            m_critical = critical;
        }

        /// <summary>
        /// Toggles the Hitbox for a while.
        /// </summary>
        public void Toggle() => Toggle(defaultToggleDuration);

        /// <summary>
        /// Toggles the Hitbox for a given duration in seconds.
        /// </summary>
        /// <param name="duration">The duration in seconds you want the Hitbox to stay activated.</param>
        public void Toggle(float duration)
        {
            StartCoroutine(ToggleRoutine(duration));
        }

        public IEnumerator ToggleRoutine(float duration)
        {
            m_collider.enabled = true;
            yield return new WaitForSeconds(duration);
            m_collider.enabled = false;
        }

        protected virtual void HandleEntityAttack(Collider other)
        {
            if (other.CompareTag(m_entity.targetTag) &&
                other.TryGetComponent(out m_otherEntity))
            {
                m_otherEntity.Damage(m_entity, m_damage, m_critical);
                m_collider.enabled = false;
            }
        }

        protected virtual void HandleDestructibleAttack(Collider other)
        {
            if (m_entity.CompareTag(GameTags.Player) &&
                other.CompareTag(GameTags.Destructible) &&
                other.TryGetComponent(out m_destructible))
            {
                m_destructible.Damage(m_damage);
                m_collider.enabled = false;
            }
        }

        protected virtual void Start()
        {
            InitializeEntity();
            InitializeCollider();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.transform == m_entity.transform) return;

            HandleEntityAttack(other);
            HandleDestructibleAttack(other);
        }
    }
}
