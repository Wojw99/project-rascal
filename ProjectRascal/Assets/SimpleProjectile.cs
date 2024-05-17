using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class SimpleProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 24f;
    [SerializeField] private GameObject vfxAfter;

    private void Start() {
        InitializeVfxAfter();
    }

    private void InitializeVfxAfter() {
        if(vfxAfter != null) {
            var vfx = GetComponentInChildren<VisualEffect>();
            var lifetime = vfx.GetFloat("Lifetime");
            StartCoroutine(WaitForVFX(lifetime));
        }
    }

    private IEnumerator WaitForVFX(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Instantiate(vfxAfter);
        Instantiate(vfxAfter, transform.position, transform.rotation);
    }

    private void Update() {
        var velocityVector = new Vector3(transform.forward.x, 0f, transform.forward.z);
        transform.position += velocityVector * moveSpeed * Time.deltaTime;;
    }
}
