using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Game/Game Scenes")]
    public class GameScenes : Singleton<GameScenes>
    {
        [Header("Loading Settings")]
        [Tooltip("The Game Object that represents the loading screen.")]
        public GameObject loadingScreen;

        [Tooltip("The Slider component that represents the loading progress.")]
        public Slider loadingSlider;

        [Header("Loading Settings")]
        [Tooltip("An Audio Clip that plays when the loading starts.")]
        public AudioClip loadStartClip;

        [Tooltip("An Audio Cip that plays when the loading finishes.")]
        public AudioClip loadFinishClip;

        /// <summary>
        /// Loads a given scene by its name from the build settings.
        /// </summary>
        /// <param name="scene">The name of the scene you want to load.</param>
        public virtual void LoadScene(string scene)
        {
            GameSave.instance.Save();
            GameAudio.instance.PlayEffect(loadStartClip);
            Fader.instance.FadeOut(() => StartCoroutine(LoadSceneRoutine(scene)));
        }

        protected virtual IEnumerator LoadSceneRoutine(string scene)
        {
            var operation = SceneManager.LoadSceneAsync(scene);

            loadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                loadingSlider.value = operation.progress;
                yield return null;
            }

            loadingScreen.SetActive(false);
            Fader.instance.FadeIn();
            GameAudio.instance.PlayEffect(loadFinishClip);
        }
    }
}
