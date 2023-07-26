using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int meleeAttackCastDuration = 52;
    [SerializeField] private int meleeAttackDamageTime = 26;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private VfxWizard vfxWizard;
    [SerializeField] private TestWizard testWizard;

    private GameCharacter gameCharacter;
    private CharacterCanvas characterCanvas;
    private NavMeshAgent navMeshAgent;
    private PlayerState playerState = PlayerState.Idle;
    private HumanAnimator humanAnimator;
    private Vector3 lookDirection;
    private Collider meleeAttackCollider;
    private float minDistanceForRunning = 1.1f;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        humanAnimator = GetComponent<HumanAnimator>();
        lookDirection = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        meleeAttackCollider = GetComponent<CapsuleCollider>();
        gameCharacter = GetComponent<GameCharacter>();
        characterCanvas = GetComponentInChildren<CharacterCanvas>();
        DisableMeleeAttackCollider();
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
        && playerState != PlayerState.Casting 
        && CountDistanceToMouse() > minDistanceForRunning) { 
            var movement = lookDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += movement;
            playerState = PlayerState.Running;
            humanAnimator.AnimateRunning();
        } 
    }

    private void HandleIdle() {
        if((!gameInput.IsRightClickPressed() 
        && playerState != PlayerState.Casting)
        || CountDistanceToMouse() <= minDistanceForRunning) { 
            playerState = PlayerState.Idle;
            humanAnimator.AnimateIdle();
        } 
    }

    private void HandleMeleeAttack() {
        if(gameInput.IsLeftClickJustPressed() && playerState != PlayerState.Casting) {
            playerState = PlayerState.Casting;
            humanAnimator.AnimateMeleeAttack();
            float t1 = meleeAttackCastDuration / 60f;
            float t2 = meleeAttackDamageTime / 60f;
            Debug.Log(t1);
            Debug.Log(t2);
            Invoke("ResetToIdle", t1);
            Invoke("EnforceDamage", t2);
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

    private void EnforceDamage()
    {
        meleeAttackCollider.enabled = true;
        Invoke("DisableMeleeAttackCollider", 0.05f);
    }

    private void DisableMeleeAttackCollider() {
        meleeAttackCollider.enabled = false;
    }

    private void ResetToIdle() {
        playerState = PlayerState.Idle;
        humanAnimator.AnimateIdle();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")) {
            var enemyCharacter = other.GetComponent<GameCharacter>();
            var enemyController = other.GetComponent<EnemyController>();
            if(enemyCharacter != null) {
                enemyCharacter.TakeDamage(5f);
            }
            if(enemyController != null) {
                enemyController.VisualizeDamage(transform.position);
            }
        }
    }

    public PlayerState PlayerStateProp {
        get { return playerState; }
        set { playerState = value; }
    }

    public enum PlayerState {
        Idle, Running, Casting
    }
}