using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(Entity))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity Audio")]
    public class EntityAudio : MonoBehaviour
    {
        [Tooltip("List of audios to play when the Entity gets hit.")]
        public AudioClip[] hitClips;

        [Tooltip("List of audios to play when the Entity gets a critical hit.")]
        public AudioClip[] criticalHitClips;

        [Tooltip("List of audios to play when the Entity dies.")]
        public AudioClip[] dieClips;

        [Tooltip("List of audios to play when the Entity performs a melee attack.")]
        public AudioClip[] meleeAttackClips;

        [Tooltip("List of audios to play when the Entity have a target assigned.")]
        public AudioClip[] targetSetClips;

        protected Entity m_entity;
        protected AudioClip m_tempClip;

        protected GameAudio m_audio => GameAudio.instance;

        protected virtual void InitializeEntity() => m_entity = GetComponent<Entity>();

        protected virtual void InitializeCallbacks()
        {
            m_entity.onDamage.AddListener((amount, source, critical) => OnDamage(critical));
            m_entity.onPerformAttack.AddListener(OnPerformAttack);
            m_entity.onTargetSet.AddListener(OnTargetSet);
            m_entity.onDie.AddListener(OnDie);
        }

        /// <summary>
        /// Plays a given Audio Clip.
        /// </summary>
        /// <param name="audioClip">The Audio Clip you want to play.</param>
        public virtual void PlayClip(AudioClip audioClip)
        {
            if (audioClip)
                m_audio.PlayEffect(audioClip);
        }

        /// <summary>
        /// Plays a random Audio Clip from an array of Audio Clips.
        /// </summary>
        /// <param name="clips">The array of Audio Clips to play a random audio from.</param>
        protected void PlayRandomClip(AudioClip[] clips)
        {
            if (TryGetRandomClip(clips, out m_tempClip))
                PlayClip(m_tempClip);
        }

        /// <summary>
        /// Tries getting a random Audio Clip from an array of Audio Clips.
        /// </summary>
        /// <param name="clips">The array of Audio Clips you want to find a random one.</param>
        /// <param name="clip"></param>
        /// <returns>Returns true if it found an Audio Clip.</returns>
        protected bool TryGetRandomClip(AudioClip[] clips, out AudioClip clip)
        {
            clip = null;

            if (clips != null && clips.Length > 0)
                clip = clips[Random.Range(0, clips.Length)];

            return clip != null;
        }

        protected virtual void OnDamage(bool critical)
        {
            if (critical)
                PlayRandomClip(criticalHitClips);
            else
                PlayRandomClip(hitClips);
        }

        protected virtual void OnPerformAttack(EntityAttackType attackType)
        {
            switch (attackType)
            {
                default:
                    PlayRandomClip(meleeAttackClips);
                    break;
                case EntityAttackType.Weapon:
                    PlayRandomClip(m_entity.items.GetWeapon().attackClips);
                    break;
                case EntityAttackType.Skill:
                    PlayClip(m_entity.skills.current?.sound);
                    break;
            }
        }

        protected virtual void OnTargetSet()
        {
            if (m_entity.target != null)
                PlayRandomClip(targetSetClips);
        }

        protected virtual void OnDie() => PlayRandomClip(dieClips);

        protected virtual void Awake()
        {
            InitializeEntity();
            InitializeCallbacks();
        }
    }
}
