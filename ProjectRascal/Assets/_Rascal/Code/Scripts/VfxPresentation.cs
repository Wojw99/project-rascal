using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxPresentation : MonoBehaviour
{
    private HumanAnimator humanAnimator;
    [SerializeField] private Transform buffSpawnPoint;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform startSpawnPoint;
    [SerializeField] private GameObject[] vfxList;
    [SerializeField] private Material[] vfxMaterials;
    [SerializeField] private GameObject environment0;
    [SerializeField] private GameObject environment1;

    private void Start() {
        humanAnimator = GetComponent<HumanAnimator>();
        vfxMaterials[0].SetFloat("_DissolveAmount", 0f);
        vfxMaterials[1].SetFloat("_DissolveAmount", 0f);
    }

    private void Update() {
        HandleKey1();
        HandleKey2();
        HandleEnvironmentChange();
        HandleMouseL();
    }

    private void HandleEnvironmentChange() {
        if (Input.GetKeyDown(KeyCode.E)) {
            var isActive = environment0.activeSelf;
            environment0.SetActive(!isActive);
            environment1.SetActive(isActive);
        }
    }

    private void HandleMouseL() {
        if(InputWizard.instance.IsLeftClickJustPressed()) {
            humanAnimator.AnimateAttack2Handed();
        }
    }

    private void HandleKey1() {
        if(InputWizard.instance.IsKey1Pressed()) {
            humanAnimator.AnimateBuff();
            var duration = HumanAnimator.NormalizeDuration(humanAnimator.BuffCastDuration);
            StartCoroutine(WaitForIdle(duration));
            // StartCoroutine(WaitForMaterialDissholveOn(1, 1f));
            GameObject.Instantiate(vfxList[0], buffSpawnPoint.position, buffSpawnPoint.rotation);
        }
    }

    private void HandleKey2() {
        if(InputWizard.instance.IsKey2Pressed()) {
            humanAnimator.AnimateSpellCast2();

            var duration = HumanAnimator.NormalizeDuration(humanAnimator.SpellCast2CastDuration);

            StartCoroutine(WaitForIdle(duration));
            StartCoroutine(WaitForVFX(1, duration / 4, projectileSpawnPoint.position, projectileSpawnPoint.rotation));
            StartCoroutine(WaitForVFX(2, duration / 4, startSpawnPoint.position, startSpawnPoint.rotation));
        }
    }

    private IEnumerator WaitForVFX(int vfxIndex, float delay, Vector3 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Instantiate(vfxList[vfxIndex], position, rotation);
    }

    private IEnumerator WaitForMaterialDissholveOn(int materialIndex, float delay) {
        yield return new WaitForSeconds(delay);
        vfxMaterials[materialIndex].SetFloat("_DissolveAmount", 1f);
    }

    private IEnumerator WaitForIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetToIdle();
    }

    private void ResetToIdle() {
        humanAnimator.AnimateIdle();
    }
}
