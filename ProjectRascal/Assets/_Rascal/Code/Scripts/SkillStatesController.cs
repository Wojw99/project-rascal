using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStatesController : MonoBehaviour
{
    // [SerializeField] private SkinnedMeshRenderer chestRenderer;
    [SerializeField] private Material magicArmorMaterial;

    private float refreshRate = 0.025f;
    private float currentTime = 0f;

    private float magicArmorDuration = 5f;

    private void Start() {
        magicArmorMaterial.SetFloat("_DissolveAmount", 0f);
    }

    public void SummonMagicArmor(float duration = 2f) {
        var transPos = transform.position;
        var position = new Vector3(transPos.x, transPos.y + 1.5f, transPos.z);
        VfxWizard.instance.SummonMagicArmorExtraEffect(position);
        StartCoroutine(WaitForSummonMagicArmor(duration));
    }

    public IEnumerator WaitForSummonMagicArmor(float duration = 2f) {
        if(magicArmorMaterial != null) {
            while(currentTime < duration) {
                currentTime += refreshRate;

                var maxDissolve = 1;
                var dissolveAmount = currentTime * maxDissolve / duration;
                magicArmorMaterial.SetFloat("_DissolveAmount", dissolveAmount);

                yield return new WaitForSeconds(refreshRate);
            }
            currentTime = 0f;
            StartCoroutine(WaitForCancelMagicArmor(magicArmorDuration));
        } else {
            Debug.LogError("No materials in chest!");
        }
    }

    public IEnumerator WaitForCancelMagicArmor(float delay) {
        yield return new WaitForSeconds(delay);

        if(magicArmorMaterial != null) {
            magicArmorMaterial.SetFloat("_DissolveAmount", 0f);
        } else {
            Debug.LogError("No materials in chest!");
        }
    }

    // public IEnumerator WaitForSummonMagicArmor(float duration = 2f) {
    //     var chestMaterials = chestRenderer.materials;
    //     if(chestMaterials.Length > 0) {
    //         while(currentTime < duration) {
    //             currentTime += refreshRate;
    //             foreach(var mat in chestMaterials) {
    //                 var maxDissolve = 1;
    //                 var dissolveAmount = currentTime * maxDissolve / duration;
    //                 mat.SetFloat("_DissolveAmount", dissolveAmount);
    //             }
    //             yield return new WaitForSeconds(refreshRate);
    //         }
    //         currentTime = 0f;
    //         StartCoroutine(WaitForCancelMagicArmor(magicArmorDuration));
    //     } else {
    //         Debug.LogError("No materials in chest!");
    //     }
    // }

    // public IEnumerator WaitForCancelMagicArmor(float delay) {
    //     yield return new WaitForSeconds(delay);

    //     var chestMaterials = chestRenderer.materials;
    //     if(chestMaterials.Length > 0) {
    //             foreach(var mat in chestMaterials) {
    //                 mat.SetFloat("_DissolveAmount", 0f);
    //             }
    //     } else {
    //         Debug.LogError("No materials in chest!");
    //     }
    // }
}
