using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float attackingDistance = 1f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private VfxWizard vfxWizard;
    [SerializeField] private TestWizard testWizard;

    private NavMeshAgent navMeshAgent;
    private PlayerState playerState = PlayerState.Idle;
    private PlayerAnimator playerAnimator;
    private Collider chargedCollider;
    private Vector3 lookDirection;
    private float moveSpeed = 5f;
    private Collider meleeAttackCollider;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<PlayerAnimator>();
        lookDirection = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        meleeAttackCollider = GetComponent<CapsuleCollider>();
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

    private void HandleRunning() {
        if(gameInput.IsLeftClickPressed() && playerState != PlayerState.Attack) { 
            var movement = lookDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += movement;
            playerState = PlayerState.Move;
            playerAnimator.AnimateRunning();
        } 
    }

    private void HandleIdle() {
        if(!gameInput.IsLeftClickPressed() && playerState != PlayerState.Attack) { 
            playerState = PlayerState.Idle;
            playerAnimator.AnimateIdle();
        } 
    }

    private void HandleMeleeAttack() {
        if(gameInput.IsRightClickJustPressed() && playerState != PlayerState.Attack) {
            playerState = PlayerState.Attack;
            playerAnimator.AnimateMeleeAttack();
            Invoke("ResetToIdle", 0.52f);
            Invoke("EnforceDamage", 0.4f);
        }
    }

    private void EnforceDamage()
    {
        var hitEnemies = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("Enemy"));
        Debug.Log(hitEnemies.Length);
        foreach (Collider enemyCollider in hitEnemies) {
            Debug.Log(enemyCollider);
            var enemyController = enemyCollider.GetComponent<EnemyController>();
            enemyController.TakeDamage(transform.position);
        }
    }

    private void OnDrawGizmos() {
        if(playerState == PlayerState.Attack) {
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.ToString());
    }

    private void OnTriggerStay(Collider other) {
        Debug.Log(other.ToString());
        Debug.Log(other.tag);
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy!");
        }
    }

    private void ResetToIdle() {
        playerState = PlayerState.Idle;
        playerAnimator.AnimateIdle();
    }

    private void HandleMovement2() {
        if(gameInput.IsLeftClickPressed()) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 

            if(Physics.Raycast(ray, out hit)) {
                if(TagWizard.IsEnemy(hit.collider)) {
                    var enemyPosition = hit.collider.transform.position;

                    if(CanMoveTo(enemyPosition)) {
                        ChargeTo(hit.collider);
                    } else {
                        Attack(hit.collider);
                    }
                } else {
                    var clickPosition = hit.point;
                    MoveTo(clickPosition);
                }
            }            
        }
    }

    public void ChargeTo(Collider collider) {
        chargedCollider = collider;
        navMeshAgent.destination = collider.transform.position;
        navMeshAgent.stoppingDistance = attackingDistance;
        playerState = PlayerState.Charge; 
        Debug.Log("change state to charge");
    }

    public void Idle() {
        playerState = PlayerState.Idle;
        Debug.Log("change state to idle");
    }

    public void MoveTo(Vector3 clickPosition) {
        navMeshAgent.destination = clickPosition;
        navMeshAgent.stoppingDistance = stoppingDistance;
        playerState = PlayerState.Move;
        Debug.Log("change state to move");
    }

    public void Attack(Collider collider) {
        if(playerState != PlayerState.Attack) {
            playerState = PlayerState.Attack;
            Debug.Log("change state to attack");
            playerAnimator.AnimateMeleeAttack();
            var enemyController = collider.GetComponent<EnemyController>();
            enemyController.TakeDamage(transform.position);
        }
    }

    private void HandleStop() {
        if (playerState == PlayerState.Charge || playerState == PlayerState.Move) {
            if (!CanMoveTo(navMeshAgent.destination)) {
                if (playerState == PlayerState.Charge) {
                    FinalizeCharge();
                } else {
                    Idle();
                }
            }
        }
    }

    private void FinalizeCharge() {
        if(chargedCollider != null) {
            Attack(chargedCollider);
            chargedCollider = null;
        }
    }

    // public void StopAttack() {
    //     if(playerState == PlayerState.Attack) {
    //         playerState = PlayerState.Idle;
    //         Debug.Log("change state to idle");
    //     }
    // }

    private bool CanMoveTo(Vector3 destination) {
        var distToDestination = Vector3.Distance(transform.position, destination);
        return distToDestination > navMeshAgent.stoppingDistance;
    }

    public PlayerState PlayerStateProp {
        get { return playerState; }
        set { playerState = value; }
    }

    public enum PlayerState
    {
        Idle, Charge, Move, Attack
    }
}