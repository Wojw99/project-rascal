using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IDamagaController {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int meleeAttackCastDuration = 52;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private VfxWizard vfxWizard;
    [SerializeField] private TestWizard testWizard;

    private GameCharacter gameCharacter;
    private CharacterCanvas characterCanvas;
    private NavMeshAgent navMeshAgent;
    private CharacterState playerState = CharacterState.Idle;
    private HumanAnimator humanAnimator;
    private DamagableWeapon damagableWeapon;
    private Vector3 lookDirection;
    private float minDistanceForRunning = 1.1f;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        humanAnimator = GetComponent<HumanAnimator>();
        lookDirection = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        gameCharacter = GetComponent<GameCharacter>();
        characterCanvas = GetComponentInChildren<CharacterCanvas>();
        damagableWeapon = GetComponentInChildren<DamagableWeapon>();
    }

    private void Update() {
        HandleRotation();
        HandleRunning();
        HandleMeleeAttack();
        HandleIdle();
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
        if(gameInput.IsRightClickPressed() 
        && playerState != CharacterState.Casting 
        && CountDistanceToMouse() > minDistanceForRunning) { 
            var movement = lookDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += movement;
            playerState = CharacterState.Running;
            humanAnimator.AnimateRunning();
        } 
    }

    private void HandleIdle() {
        if((!gameInput.IsRightClickPressed() 
        && playerState != CharacterState.Casting)
        || CountDistanceToMouse() <= minDistanceForRunning) { 
            playerState = CharacterState.Idle;
            humanAnimator.AnimateIdle();
        } 
    }

    private void HandleMeleeAttack() {
        if(gameInput.IsLeftClickJustPressed() && playerState != CharacterState.Casting) {
            playerState = CharacterState.Casting;
            humanAnimator.AnimateMeleeAttack();
            float t1 = meleeAttackCastDuration / 60f;
            Invoke("ResetToIdle", t1);
            damagableWeapon.TakeDamage(gameCharacter.Attack);
        }
    }

    public void VisualizeDamage(Vector3 hitPosition, bool bloodSpill = true){
        if(bloodSpill) {
            var bloodSpillPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            vfxWizard.SummonBloodSpillEffect(bloodSpillPosition, Quaternion.LookRotation(hitPosition));
        }
        if (gameCharacter.IsDead()) {
            characterCanvas.DisableHealthBar();
        } else {
            characterCanvas.UpdateHealthBar(gameCharacter.CurrentHealth, gameCharacter.MaxHealth);
        }
    }

    private void ResetToIdle() {
        playerState = CharacterState.Idle;
        humanAnimator.AnimateIdle();
    }

    public CharacterState PlayerState {
        get { return playerState; }
        set { playerState = value; }
    }

    public enum CharacterState {
        Idle, Running, Casting
    }
}