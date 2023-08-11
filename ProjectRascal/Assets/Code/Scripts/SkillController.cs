using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject bulletSpawnPoint;

    private GameCharacter gameCharacter;

    private void Start() {
        gameCharacter = GetComponent<GameCharacter>();
    }

    public IEnumerator WaitForHandLight(float delay)
    {
        yield return new WaitForSeconds(delay);
        SummonHandLight();
    }

    public void SummonHandLight() {
        VfxWizard.instance.SummonHandLight(leftHand.transform.position, Quaternion.identity, leftHand.transform);
        VfxWizard.instance.SummonHandLight(rightHand.transform.position, Quaternion.identity, rightHand.transform);
    }

    public IEnumerator WaitForThunderstruck(float delay, Vector3 mouseGroundPosition)
    {
        yield return new WaitForSeconds(delay);
        SpawnThunderstruck(mouseGroundPosition);
    }

    public IEnumerator WaitForMagicBullet(float delay, Vector3 mouseGroundPosition)
    {
        yield return new WaitForSeconds(delay);
        SpawnMagicBullet(mouseGroundPosition);
    }

    public void SpawnMagicBullet(Vector3 mouseGroundPosition) {
        var spawnTransform = bulletSpawnPoint.transform;
        var bullet = DamageDealerWizard.instance.SummonMagicBullet(spawnTransform.position, spawnTransform.rotation);
        if(bullet.TryGetComponent(out MagicBulletDD damageDealer)) {
            damageDealer.FeedAndDealDamage(ownerCharacter: gameCharacter, endPoint: mouseGroundPosition, damageStartTime: 0f, damageDuration: 5f);
            damageDealer.SetLifetime();
        }
    }

    public void SpawnThunderstruck(Vector3 mouseGroundPosition) {
        var handLightDelay = 0.1f;
        StartCoroutine(WaitForHandLight(handLightDelay));

        var spawnPosition = mouseGroundPosition + Vector3.up * 0.01f;
        var thunderstruck = DamageDealerWizard.instance.SummonThunderstruck(spawnPosition);
        if(thunderstruck.TryGetComponent(out DamageDealer damageDealer)) {
            damageDealer.FeedAndDealDamage(ownerCharacter: gameCharacter, damageStartTime: 0.77f, damageDuration: 0.4f);
            damageDealer.SetLifetime();
        }
    }
}
