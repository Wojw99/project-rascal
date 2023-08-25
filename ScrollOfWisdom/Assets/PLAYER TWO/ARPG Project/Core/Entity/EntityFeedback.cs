using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(Entity))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity Feedback")]
    public class EntityFeedback : MonoBehaviour
    {
        [Header("Damage Text")]
        [Tooltip("A prefab that shows up when the Entity takes damage.")]
        public GameObject damageText;

        [Tooltip("The position offset applied when the damage text is instantiated.")]
        public Vector3 damageTextOffset = new Vector3(0, 1, 0);

        [Header("Level Up")]
        [Tooltip("The Audio Clip that plays when the Entity levels up.")]
        public AudioClip levelUpAudio;

        [Tooltip("The particle instantiated when the Entity levels up.")]
        public ParticleSystem levelUpParticle;

        protected Entity m_entity;

        protected GameAudio m_audio => GameAudio.instance;

        public virtual void OnEntityDamage(int amount, Vector3 _, bool critical)
        {
            var origin = transform.position + damageTextOffset;
            var instance = Instantiate(damageText, origin, Quaternion.identity);

            if (instance.TryGetComponent(out DamageText text))
            {
                text.target = transform;
                text.SetText(amount, critical);
            }
        }

        public virtual void OnLevelUp()
        {
            m_audio.PlayEffect(levelUpAudio);
            levelUpParticle?.Play();
        }

        protected virtual void Start()
        {
            m_entity = GetComponent<Entity>();
            m_entity.onDamage.AddListener(OnEntityDamage);
            m_entity.stats.onLevelUp.AddListener(OnLevelUp);
        }
    }
}
