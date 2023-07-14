using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    // * * * * * * SERIALIZED * * * * * *
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float attackingDistance = 1f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private VfxWizard vfxWizard;

    // * * * * * * PRIVATE * * * * * *
    private NavMeshAgent navMeshAgent;
    private PlayerState playerState = PlayerState.Idle;
    private PlayerAnimator playerAnimator;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Update() {
        HandleMovement();
        HandleStop();
    }

    private void HandleMovement() {
        if(gameInput.GetLeftClick()) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 

            if(Physics.Raycast(ray, out hit)) {
                if(hit.collider.CompareTag("Enemy")) {
                    var enemyPosition = hit.collider.transform.position;

                    if(CanMoveTo(enemyPosition)) {
                        ChargeTo(enemyPosition);
                    } else if (playerState != PlayerState.Attack) {
                        Attack();
                    }
                } else {
                    var clickPosition = hit.point;
                    MoveTo(clickPosition);
                    vfxWizard.SummonGroundClickEffect(clickPosition);
                }
            }            
        }
    }

    public void ChargeTo(Vector3 enemyPosition) {
        navMeshAgent.destination = enemyPosition;
        navMeshAgent.stoppingDistance = attackingDistance;
        playerState = PlayerState.Charge; 
    }

    public void Idle() {
        playerState = PlayerState.Idle;
    }
    public void MoveTo(Vector3 clickPosition) {
        navMeshAgent.destination = clickPosition;
        navMeshAgent.stoppingDistance = stoppingDistance;
        playerState = PlayerState.Move;
    }
    public void Attack() {
        playerState = PlayerState.Attack;
        playerAnimator.AnimateAttack();
    }
    private void HandleStop() {
        if (playerState == PlayerState.Charge || playerState == PlayerState.Move) {
            if (!CanMoveTo(navMeshAgent.destination)) {
                if (playerState == PlayerState.Charge) {
                    Attack();
                } else {
                    Idle();
                }
            }
        }
    }

    public void StopAttack() {
        if(playerState == PlayerState.Attack) {
            playerState = PlayerState.Idle;
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