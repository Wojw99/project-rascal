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
    [SerializeField] private GameObject handLight;
    [SerializeField] private GameObject handLight2;
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private GameObject magicBulletStartEffect;
    [SerializeField] private GameObject magicBulletExplosionEffect;
    [SerializeField] private GameObject thunderstruckStartEffect;
    [SerializeField] private GameObject magicArmorExtraEffecr;
    [SerializeField] private GameObject magicGranadeEffect;
    [SerializeField] private GameObject necroImpactEffect;
    [SerializeField] private GameObject fireballStartEffect;
    [SerializeField] private GameObject fireballExplosionEffect;

    public void SummonFireballStartEffect(Vector3 position, Quaternion rotation) {
        GameObject.Instantiate(fireballStartEffect, position, rotation);
    }

    public void SummonFireballExplosionEffect(Vector3 position, Quaternion rotation) {
        GameObject.Instantiate(fireballExplosionEffect, position, rotation);
    }

    public void SummonNecroImpactEffect(Vector3 position) {
        GameObject.Instantiate(necroImpactEffect, position, Quaternion.identity);
    }

    public void SummonMagicGranadeEffect(Vector3 position) {
        GameObject.Instantiate(magicGranadeEffect, position, Quaternion.identity);
    }

    public void SummonMagicArmorExtraEffect(Vector3 position) {
        GameObject.Instantiate(magicArmorExtraEffecr, position, Quaternion.identity);
    }

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
        GameObject.Instantiate(magicBulletStartEffect, position, rotation, parent);
    }

    public void SummonHandLight(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject.Instantiate(handLight, position, rotation, parent);
    }

    public void SummonSlashEffect(Vector3 position, Transform parent) {
        GameObject.Instantiate(slashEffect, position, Quaternion.identity, parent);
    }

    public void SummonMagicBulletStartEffect(Vector3 position) {
        GameObject.Instantiate(magicBulletStartEffect, position, Quaternion.identity);
    }

    public void SummonMagicBulletExplosionEffect(Vector3 position, Quaternion rotation) {
        GameObject.Instantiate(magicBulletExplosionEffect, position, rotation);
    }

    public void SummonThunderstruckStartEffect(Vector3 position) {
        GameObject.Instantiate(thunderstruckStartEffect, position, Quaternion.identity);
    }
}
