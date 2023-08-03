using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private bool isFirstUpdate = true;

    [SerializeField] private Image loadingBar;

    private void Update()
    {
        Debug.Log(SceneLoadWizard.GetLoadingProgress());
        loadingBar.fillAmount = SceneLoadWizard.GetLoadingProgress();

        if (isFirstUpdate) {
            isFirstUpdate = false;
            SceneLoadWizard.LoadingEndCallback();
        }
    }
}
