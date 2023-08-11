using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStatesController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer chestRenderer;

    private float refreshRate = 0.025f;
    private float currentTime = 0f;

    public void SummonMagicArmor(float duration = 2f) {
        StartCoroutine(WaitForSummonMagicArmor(duration));
    }

    public IEnumerator WaitForSummonMagicArmor(float duration = 2f) {
        var chestMaterials = chestRenderer.materials;
        if(chestMaterials.Length > 0) {
            while(currentTime < duration) {
                currentTime += refreshRate;
                foreach(var mat in chestMaterials) {
                    var maxDissolve = 1;
                    var dissolveAmount = currentTime * maxDissolve / duration;
                    mat.SetFloat("_DissolveAmount", dissolveAmount);
                }
                yield return new WaitForSeconds(refreshRate);
            }
            currentTime = 0f;
        } else {
            Debug.LogError("No materials in chest!");
        }
    }
}
