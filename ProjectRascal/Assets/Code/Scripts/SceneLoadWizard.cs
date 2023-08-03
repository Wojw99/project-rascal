using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadWizard : MonoBehaviour
{
    private class LoadingMonoBehaviour : MonoBehaviour { }

    public enum Scene {
        Wojtek1, Wojtek2, Loading
    }

    private static Action onLoadingEnd;

    private static AsyncOperation loadingAsyncOperation;

    public static void Load(Scene scene) {
        onLoadingEnd = () => {
            var loadingGM = new GameObject("Loading Game Object");
            loadingGM.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
            LoadSceneAsync(scene);
        };

        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    private static IEnumerator LoadSceneAsync(Scene scene) {
        yield return null;

        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while(!loadingAsyncOperation.isDone) {
            yield return null;
        }
    }

    public static float GetLoadingProgress() {
        if(loadingAsyncOperation != null) {
            return loadingAsyncOperation.progress;
        }
        return 1f;
    }

    public static void LoadingEndCallback() {
        if(onLoadingEnd != null) {
            onLoadingEnd();
            onLoadingEnd = null;
        }
    }
}
