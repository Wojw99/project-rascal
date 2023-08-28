using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Game/Game Audio")]
    public class GameAudio : Singleton<GameAudio>
    {
        [Header("Audio Settings")]
        [Range(0, 1f)]
        [Tooltip("The initial volume of the music Audio Source.")]
        public float initialMusicVolume = 0.5f;

        [Range(0, 1f)]
        [Tooltip("The initial volume of the effects Audio Source.")]
        public float initialEffectsVolume = 0.5f;

        [Range(0, 1f)]
        [Tooltip("The initial volume of the ui effects Audio Source.")]
        public float initialUiEffectsVolume = 0.5f;

        [Header("General Audios")]
        [Tooltip("An Audio Clip to be used as a 'denied' effect.")]
        public AudioClip deniedClip;

        protected AudioSource m_musicSource;
        protected AudioSource m_effectsSource;
        protected AudioSource m_uiEffectsSource;

        protected virtual void InitializeMusicSource()
        {
            m_musicSource = gameObject.AddComponent<AudioSource>();
            m_musicSource.loop = true;
            m_musicSource.volume = initialMusicVolume;
        }

        protected virtual void InitializeEffectsSource()
        {
            m_effectsSource = gameObject.AddComponent<AudioSource>();
            m_effectsSource.volume = initialEffectsVolume;
        }

        protected virtual void InitializeUIEffectsSource()
        {
            m_uiEffectsSource = gameObject.AddComponent<AudioSource>();
            m_uiEffectsSource.volume = initialUiEffectsVolume;
        }

        /// <summary>
        /// Plays an Audio Clip with the music Audio Source in loop.
        /// </summary>
        /// <param name="clip">The Audio Clip you want to play.</param>
        public virtual void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;

            m_musicSource.clip = clip;
            m_musicSource.Play();
        }

        /// <summary>
        /// Stops playing the current music.
        /// </summary>
        public virtual void StopMusic() => m_musicSource.Stop();

        /// <summary>
        /// Plays an Audio Clip with the effects Audio Source for one time.
        /// </summary>
        /// <param name="clip">The Audio Clip you want to play.</param>
        public virtual void PlayEffect(AudioClip clip)
        {
            if (clip == null) return;

            m_effectsSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Plays an Audio Clip with the ui effects Audio Source for one time.
        /// </summary>
        /// <param name="clip">The Audio Clip you want to play.</param>
        public virtual void PlayUiEffect(AudioClip clip)
        {
            if (clip == null) return;

            m_uiEffectsSource.Stop();
            m_uiEffectsSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Plays te denied sound using the effects Audio Source.
        /// </summary>
        public virtual void PlayDeniedSound() => PlayEffect(deniedClip);

        protected override void Initialize()
        {
            InitializeMusicSource();
            InitializeEffectsSource();
            InitializeUIEffectsSource();
        }
    }
}
