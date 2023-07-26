using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private VfxWizard vfxWizard;
    private CharacterCanvas characterCanvas;
    private HumanAnimator humanAnimator;
    private GameCharacter gameCharacter;

    private void Start() {
        humanAnimator = GetComponent<HumanAnimator>();
        humanAnimator.AnimateRunning();
        gameCharacter = GetComponent<GameCharacter>();
        characterCanvas = GetComponentInChildren<CharacterCanvas>();
        Debug.Log(humanAnimator.ToString());
        Debug.Log(gameCharacter.ToString());
        Debug.Log(characterCanvas.ToString());
    }

    private void Update() {
        
    }

    public void VisualizeDamage(Vector3 hitDirection){
        var bloodSpillPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        vfxWizard.SummonBloodSpillEffect(bloodSpillPosition, Quaternion.LookRotation(hitDirection));
        if (gameCharacter.IsDead()) {
            humanAnimator.AnimateDeath();
            characterCanvas.DisableHealthBar();
        } else {
            humanAnimator.AnimateGetHit();
            characterCanvas.UpdateHealthBar(gameCharacter.CurrentHealth, gameCharacter.MaxHealth);
        }
    }

    public enum EnemyState
    {
        Idle, Running, Casting, GettingHit, Death
    }
}
