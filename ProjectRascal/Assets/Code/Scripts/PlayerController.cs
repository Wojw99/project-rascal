using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IDamagaController {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject damageDealer;

    private GameCharacter gameCharacter;
    private CharacterCanvas characterCanvas;
    private NavMeshAgent navMeshAgent;
    private CharacterState playerState = CharacterState.Idle;
    private HumanAnimator humanAnimator;
    private Vector3 lookDirection;
    private float minDistanceForRunning = 1.1f;
    private float minDistanceForRotating = 0.3f;
    private Vector3 mouseGroundPosition;
    private Interactible targetInteractible;
    private SkillController skillController;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        humanAnimator = GetComponent<HumanAnimator>();
        lookDirection = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        gameCharacter = GetComponent<GameCharacter>();
        characterCanvas = GetComponentInChildren<CharacterCanvas>();
        skillController = GetComponent<SkillController>();
    }

    private void Update() {
        if(EventWizard.instance.IsOff()) {
            mouseGroundPosition = GetMouseGroundPosition();
            HandleRotation();
            HandleRunning();
            HandleMeleeAttack();
            HandleIdle();
            HandleInteractions();
            HandleKey1();
            HandleKey2();
            HandleKey3();
            HandleUI();
            HandleTestAnim();
        }
    }

    private void HandleTestAnim() {
        if(InputWizard.instance.IsSpacePressed()) {
            humanAnimator.AnimateSurprise();
        }
    }

    public void UpdateWeaponDD(GameObject gameObject) {
        var position = damageDealer.transform.position;
        var rotation = damageDealer.transform.rotation;
        var dd = Instantiate(gameObject, rightHand.transform);
        dd.transform.position = position;
        dd.transform.rotation = rotation;
        Destroy(damageDealer);
        damageDealer = dd;
    }

    private void HandleUI() {
        if(InputWizard.instance.IsEscPressed()) {
            UIWizard.instance.HideWriting();
        }
    }

    private void HandleKey1() {
        if(InputWizard.instance.IsKey1Pressed() && playerState != CharacterState.Casting) {
            playerState = CharacterState.Casting;
            humanAnimator.AnimateBuff();
            
            var duration = HumanAnimator.NormalizeDuration(humanAnimator.BuffCastDuration);
            StartCoroutine(WaitForIdle(duration));
            StartCoroutine(skillController.WaitForThunderstruck(0f, mouseGroundPosition));
        }
    }

    private void HandleKey2() {
        if(InputWizard.instance.IsKey2Pressed() && playerState != CharacterState.Casting) {
            playerState = CharacterState.Casting;
            humanAnimator.AnimateSpellCast2();

            var duration = HumanAnimator.NormalizeDuration(humanAnimator.SpellCast2CastDuration);
            StartCoroutine(WaitForIdle(duration));
            StartCoroutine(skillController.WaitForMagicBullet(duration / 4, mouseGroundPosition));
        }
    }

    private void HandleKey3() {
        if(InputWizard.instance.IsKey3Pressed() && playerState != CharacterState.Casting) {
            playerState = CharacterState.Casting;
            humanAnimator.AnimateBuff();    

            var duration = HumanAnimator.NormalizeDuration(humanAnimator.BuffCastDuration);
            var skillStatesController = GetComponent<SkillStatesController>();
            skillStatesController.SummonMagicArmor(duration);
            StartCoroutine(WaitForIdle(duration));
        }
    }

    private IEnumerator WaitForIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetToIdle();
    }

    private void HandleInteractions() {
        if(playerState == CharacterState.Idle || playerState == CharacterState.Running) {
            if(Physics.Raycast(transform.position, lookDirection, out RaycastHit raycastHit, interactionDistance)) {
                if(raycastHit.transform.TryGetComponent(out Interactible interactibleItem)) {
                    TurnOffTargetInteractibleVision();
                    targetInteractible = interactibleItem;
                    interactibleItem.OnVisionStart();
                    if(InputWizard.instance.IsInteractionKeyPressed()) {
                        humanAnimator.AnimateGathering();
                        playerState = CharacterState.Casting;
                        interactibleItem.Interact(transform.gameObject);
                        var delay = HumanAnimator.NormalizeDuration(humanAnimator.GatheringCastDuration);
                        Invoke("ResetToIdle", delay);
                    }
                } else {
                    TurnOffTargetInteractibleVision();
                }
            } else {
                TurnOffTargetInteractibleVision();
            }
        }
    }

    private void TurnOffTargetInteractibleVision() {
        if(targetInteractible != null) {
            targetInteractible.OnVisionEnd();
            targetInteractible = null;
        }
    }

    private void HandleRotation() {
        var direction = mouseGroundPosition - transform.position;
        direction.y = 0f; 

        if(direction != lookDirection && CountDistanceToMouse() > minDistanceForRotating) {
            lookDirection = direction;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private float CountDistanceToMouse() {
        var distance = Vector3.Distance(transform.position, mouseGroundPosition);
        return distance;
    }

    private void HandleRunning() {
        if(InputWizard.instance.IsRightClickPressed() 
        && playerState != CharacterState.Casting 
        && CountDistanceToMouse() > minDistanceForRunning) { 
            var movement = lookDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += movement;
            playerState = CharacterState.Running;
            humanAnimator.AnimateRunning();
        } 
    }

    private void HandleIdle() {
        if((!InputWizard.instance.IsRightClickPressed() 
        && playerState != CharacterState.Casting)
        || (CountDistanceToMouse() <= minDistanceForRunning 
        && playerState != CharacterState.Casting)) { 
            playerState = CharacterState.Idle;
            humanAnimator.AnimateIdle();
        } 
    }

    private void HandleMeleeAttack() {
        if(InputWizard.instance.IsLeftClickJustPressed() && playerState != CharacterState.Casting) {
            playerState = CharacterState.Casting;
            humanAnimator.AnimateMeleeAttack();
            var duration = HumanAnimator.NormalizeDuration(humanAnimator.MeleeAttackCastDuration);
            Invoke("ResetToIdle", duration);
            var weaponDD = GetComponentInChildren<WeaponDD>();
            weaponDD.FeedAndDealDamage(ownerCharacter: gameCharacter, damageDuration: duration);
        }
    }
    
    public void VisualizeDamage(Vector3 hitPosition, bool bloodSpill = true){
        if(bloodSpill) {
            var bloodSpillPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            VfxWizard.instance.SummonBloodSpillEffect(bloodSpillPosition, Quaternion.LookRotation(hitPosition));
        }
        if (gameCharacter.IsDead()) {
            characterCanvas.DisableHealthBarAndName();
        } else {
            characterCanvas.UpdateHealthBar(gameCharacter.CurrentHealth, gameCharacter.MaxHealth);
        }
    }

    private void ResetToIdle() {
        playerState = CharacterState.Idle;
        humanAnimator.AnimateIdle();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactionDistance);
    }

    private Vector3 GetMouseGroundPosition() {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            var groundPosition = -0.5f;
            var mousePosition = hit.point;
            mousePosition.y = groundPosition;
            return mousePosition;
        }
        return Input.mousePosition;
    }

    public CharacterState PlayerState {
        get { return playerState; }
        set { playerState = value; }
    }

    public enum CharacterState {
        Idle, Running, Casting
    }
}