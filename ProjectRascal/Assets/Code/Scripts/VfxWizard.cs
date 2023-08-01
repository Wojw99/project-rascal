using UnityEngine;

public class VfxWizard : MonoBehaviour
{
    #region Singleton

    public static VfxWizard instance;

    private void Awake() {
        instance = this;
    }

    private VfxWizard() {

    }

    #endregion

    [SerializeField] private GameObject fancyCircleEffect;
    [SerializeField] private GameObject bloodSpillEffect;
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject spellLight;
    [SerializeField] private GameObject slashEffect;

    public void SummonFancyCircleEffect(Vector3 position) {
        GameObject.Instantiate(fancyCircleEffect, position, Quaternion.identity);
    }

    public void SummonBloodSpillEffect(Vector3 position, Quaternion rotation) {
        GameObject.Instantiate(bloodSpillEffect, position, rotation);
    }

    public void SummonHealEffect(Vector3 position, Transform parent) {
        GameObject.Instantiate(healEffect, position, Quaternion.identity, parent);
    }

    public void SummonSpelllight(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject.Instantiate(spellLight, position, rotation, parent);
    }

    public void SummonSlashEffect(Vector3 position, Transform parent) {
        GameObject.Instantiate(slashEffect, position, Quaternion.identity, parent);
    }
}
