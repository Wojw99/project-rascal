using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour, IDamagaController {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private int meleeAttackCastDuration = 42;
    [SerializeField] private int buffCastDuration = 92;
    [SerializeField] private int gatheringCastDuration = 80;
    [SerializeField] private int spellCast2CastDuration = 60;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject bulletSpawnPoint;
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

    private bool isTurnedOn = true;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        humanAnimator = GetComponent<HumanAnimator>();
        lookDirection = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        gameCharacter = GetComponent<GameCharacter>();
        characterCanvas = GetComponentInChildren<CharacterCanvas>();
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

    public void TurnOn() {
        isTurnedOn = true;
    }

    public void TurnOff() {
        isTurnedOn = false;
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
            Invoke("SummonHandLight", 0.1f);
            float durationNormalized = buffCastDuration / 60f;
            StartCoroutine(WaitForIdle(durationNormalized));
            StartCoroutine(WaitForThunderstruck(0f, mouseGroundPosition));
        }
    }

    private void HandleKey2() {
        if(InputWizard.instance.IsKey2Pressed() && playerState != CharacterState.Casting) {
            playerState = CharacterState.Casting;
            humanAnimator.AnimateSpellCast2();
            float durationNormalized = spellCast2CastDuration / 60f;
            StartCoroutine(WaitForIdle(durationNormalized));
            StartCoroutine(WaitForMagicBullet(durationNormalized / 4, mouseGroundPosition));
        }
    }

    [SerializeField] private Renderer chestRenderer;
    // private MaterialPropertyBlock materialPropertyBlock;

    private void HandleKey3() {
        if(InputWizard.instance.IsKey3Pressed() && playerState != CharacterState.Casting) {
            playerState = CharacterState.Casting;
            humanAnimator.AnimateBuff();    
            float durationNormalized = buffCastDuration / 60f;
            var skillController = GetComponent<SkillController>();
            skillController.SummonMagicArmor(durationNormalized);
            StartCoroutine(WaitForIdle(durationNormalized));
        }
    }

    private void SummonHandLight() {
        VfxWizard.instance.SummonHandLight(leftHand.transform.position, Quaternion.identity, leftHand.transform);
        VfxWizard.instance.SummonHandLight(rightHand.transform.position, Quaternion.identity, rightHand.transform);
    }

    private IEnumerator WaitForIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetToIdle();
    }

    private IEnumerator WaitForThunderstruck(float delay, Vector3 mouseGroundPosition)
    {
        yield return new WaitForSeconds(delay);
        SpawnThunderstruck(mouseGroundPosition);
    }

    private IEnumerator WaitForMagicBullet(float delay, Vector3 mouseGroundPosition)
    {
        yield return new WaitForSeconds(delay);
        SpawnMagicBullet(mouseGroundPosition);
    }

    private IEnumerator WaitForMagicArmor(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnMagicArmor();
    }

    private void SpawnMagicArmor() {
        // var material = chestRenderer.material;
        chestRenderer.material.SetFloat("_Alpha", 1);
        chestRenderer.material.SetFloat("_Grow", 0);
    }

    private void SpawnMagicBullet(Vector3 mouseGroundPosition) {
        var spawnTransform = bulletSpawnPoint.transform;
        var bullet = DamageDealerWizard.instance.SummonMagicBullet(spawnTransform.position, spawnTransform.rotation);
        if(bullet.TryGetComponent(out MagicBulletDD damageDealer)) {
            damageDealer.FeedAndDealDamage(ownerCharacter: gameCharacter, endPoint: mouseGroundPosition, damageStartTime: 0f, damageDuration: 5f);
            damageDealer.SetLifetime();
        }
    }

    private void SpawnThunderstruck(Vector3 mouseGroundPosition) {
        var spawnPosition = mouseGroundPosition + Vector3.up * 0.01f;
        var thunderstruck = DamageDealerWizard.instance.SummonThunderstruck(spawnPosition);
        if(thunderstruck.TryGetComponent(out DamageDealer damageDealer)) {
            damageDealer.FeedAndDealDamage(ownerCharacter: gameCharacter, damageStartTime: 0.77f, damageDuration: 0.4f);
            damageDealer.SetLifetime();
        }
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
                        float delay = gatheringCastDuration / 60f;
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
            float durationNormalized = meleeAttackCastDuration / 60f;
            Invoke("ResetToIdle", durationNormalized);
            var weaponDD = GetComponentInChildren<WeaponDD>();
            weaponDD.FeedAndDealDamage(ownerCharacter: gameCharacter, damageDuration: durationNormalized);
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