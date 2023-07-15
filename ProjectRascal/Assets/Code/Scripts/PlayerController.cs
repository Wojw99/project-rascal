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

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Update() {
        HandleMovement();
        HandleStop();
    }

    private float enemyHitPauseTime = 0.15f;
    private bool enemyHitEnabled = true;

    private void HandleMovement() {
        if(gameInput.GetLeftClick()) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 

            if(Physics.Raycast(ray, out hit)) {
                if(TagWizard.IsEnemy(hit.collider) && enemyHitEnabled) {
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

    private void DisableEnemyHit() {
        enemyHitEnabled = false;
        Invoke("EnableEnemyHit", enemyHitPauseTime);
    }

    private void EnableEnemyHit() => enemyHitEnabled = true;

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
            playerAnimator.AnimateAttack();
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

    public void StopAttack() {
        if(playerState == PlayerState.Attack) {
            playerState = PlayerState.Idle;
            Debug.Log("change state to idle");
        }
    }

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