using UnityEngine;
using System.Collections;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Game/Level Respawner")]
    public class LevelRespawner : MonoBehaviour
    {
        [Header("General Settings")]
        public float fadeOutDelay;
        public float fadeInDelay;

        [Header("Respawn Settings")]
        public bool resetExperience;

        protected Coroutine m_deathRoutine;
        protected Coroutine m_respawnRoutine;

        protected Entity m_player => Level.instance.player;
        protected Transform m_respawn => Level.instance.playerOrigin;

        protected virtual void InitializeCallbacks()
        {
            m_player.onDie.AddListener(OnPlayerDie);
        }

        protected virtual void OnPlayerDie()
        {
            if (m_deathRoutine != null) StopCoroutine(m_deathRoutine);
            if (m_respawnRoutine != null) StopCoroutine(m_respawnRoutine);

            m_deathRoutine = StartCoroutine(DeathRoutine());
        }

        protected IEnumerator DeathRoutine()
        {
            yield return new WaitForSeconds(fadeOutDelay);

            Fader.instance.FadeOut(() =>
            {
                m_respawnRoutine = StartCoroutine(RespawnRoutine());
            });
        }

        protected IEnumerator RespawnRoutine()
        {
            var position = m_respawn.position;
            var rotation = m_respawn.rotation;

            m_player.Teleport(position, rotation);
            m_player.Revive();

            if (resetExperience)
                m_player.stats.ResetExperience();

            yield return new WaitForSeconds(fadeInDelay);

            Fader.instance.FadeIn(() =>
            {
                m_player.inputs.enabled = true;
            });
        }

        protected virtual void Start() => InitializeCallbacks();
    }
}
