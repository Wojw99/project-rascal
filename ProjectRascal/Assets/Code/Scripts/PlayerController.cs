using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IDamagaController {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private int meleeAttackCastDuration = 52;
    [SerializeField] private Camera mainCamera;

    private GameCharacter gameCharacter;
    private CharacterCanvas characterCanvas;
    private NavMeshAgent navMeshAgent;
    private CharacterState playerState = CharacterState.Idle;
    private HumanAnimator humanAnimator;
    private WeaponDD weaponDD;
    private Vector3 lookDirection;
    private float minDistanceForRunning = 1.1f;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        humanAnimator = GetComponent<HumanAnimator>();
        lookDirection = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        gameCharacter = GetComponent<GameCharacter>();
        characterCanvas = GetComponentInChildren<CharacterCanvas>();
        weaponDD = GetComponentInChildren<WeaponDD>();
    }

    private void Update() {
        HandleRotation();
        HandleRunning();
        HandleMeleeAttack();
        HandleIdle();
        HandleInteractions();
        HandleThunderstruck();
    }

    private void HandleThunderstruck() {
        if(InputWizard.instance.IsKey1Pressed() && playerState != CharacterState.Casting) {
            playerState = CharacterState.Casting;
            humanAnimator.AnimateBuff();

            float t1 = meleeAttackCastDuration / 60f;
            Invoke("ResetToIdle", t1);
            Invoke("SpawnThunderstruck", t1 / 2);
        }
    }

    private void SpawnThunderstruck() {
        var mousePosition = Input.mousePosition;
        var mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y - transform.position.y));
        var pos = new Vector3(mouseWorldPosition.x, transform.position.y, mouseWorldPosition.z);

        var thunderstruck = DamageDealerWizard.instance.SummonThunderstruck(pos);
        if(thunderstruck.TryGetComponent(out DamageDealer damageDealer)) {
            damageDealer.FeedAndDealDamage(ownerCharacter: gameCharacter, damageStartTime: 0.1f, damageDuration: 0.5f);
        }
    }

    private Potion targetInteractible;

    private void HandleInteractions() {
        if(playerState == CharacterState.Idle || playerState == CharacterState.Running) {
            if(Physics.Raycast(transform.position, lookDirection, out RaycastHit raycastHit, interactionDistance)) {
                if(raycastHit.transform.TryGetComponent(out Potion potion)) {
                    turnOffTargetInteractibleVision();
                    targetInteractible = potion;
                    potion.OnVisionStart();
                    if(InputWizard.instance.IsInteractionKeyPressed()) {
                        potion.Interact(transform.gameObject);
                    }
                } else {
                    turnOffTargetInteractibleVision();
                }
            } else {
                turnOffTargetInteractibleVision();
            }
        }
    }

    private void turnOffTargetInteractibleVision() {
        if(targetInteractible != null) {
            targetInteractible.OnVisionEnd();
            targetInteractible = null;
        }
    }

    private void HandleRotation() {
        var mousePosition = Input.mousePosition;

        var mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y - transform.position.y));

        var direction = mouseWorldPosition - transform.position;
        direction.y = 0f; 

        if(direction != lookDirection) {
            lookDirection = direction;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private float CountDistanceToMouse() {
        var mousePosition = Input.mousePosition;
        var mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.transform.position.y));
        var distance = Vector3.Distance(transform.position, mouseWorldPosition);
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
        || CountDistanceToMouse() <= minDistanceForRunning) { 
            playerState = CharacterState.Idle;
            humanAnimator.AnimateIdle();
        } 
    }

    private void HandleMeleeAttack() {
        if(InputWizard.instance.IsLeftClickJustPressed() && playerState != CharacterState.Casting) {
            playerState = CharacterState.Casting;
            humanAnimator.AnimateMeleeAttack();
            float t1 = meleeAttackCastDuration / 60f;
            Invoke("ResetToIdle", t1);
            weaponDD.FeedAndDealDamage(ownerCharacter: gameCharacter, damageDuration: t1);
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

    public CharacterState PlayerState {
        get { return playerState; }
        set { playerState = value; }
    }

    public enum CharacterState {
        Idle, Running, Casting
    }
}