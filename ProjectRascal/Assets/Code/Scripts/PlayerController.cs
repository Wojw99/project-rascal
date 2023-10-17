using Assets.Code.Scripts.NetClient.Emissary;
using NetClient;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using NetworkCore.NetworkUtility;

public class PlayerController : MonoBehaviour, IDamagaController {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject damageDealer;

    private PlayerCharacter playerCharacter;
    private CharacterCanvas characterCanvas;
    private NavMeshAgent navMeshAgent;
    private HumanAnimator humanAnimator;
    private Vector3 lookDirection;
    private float minDistanceForRunning = 1.1f;
    private float minDistanceForRotating = 0.3f;
    private Vector3 mouseGroundPosition;
    private Interactible targetInteractible;
    private SkillController skillController;

    private AdventurerState playerState = AdventurerState.Idle;
    private AdventurerState previousPlayerState = AdventurerState.Idle;

    private bool CharacterLoadSuccesFlag = false;
    private bool SendTransform = false;
    

    private void Start() {
        CharacterLoadEmissary.instance.OnCharacterLoadSucces += CharacterLoadSucces;
        CharacterLoadEmissary.instance.OnCharacterLoadFailed += CharacterLoadFailed;
        CharacterLoadEmissary.instance.CommitSendCharacterLoadRequest("gracz");
    }

    private void CharacterLoadSucces()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        humanAnimator = GetComponent<HumanAnimator>();
        playerCharacter = GetComponent<PlayerCharacter>();
        characterCanvas = GetComponentInChildren<CharacterCanvas>();
        skillController = GetComponent<SkillController>();

        //lookDirection = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

        lookDirection = CharacterTransformEmissary.instance.Rotation;
        transform.position = CharacterTransformEmissary.instance.Position;

        CharacterLoadEmissary.instance.CommitSendCharacterLoadSucces(true);
        CharacterLoadSuccesFlag = true;
        
    }

    private void CharacterLoadFailed()
    {
        CharacterLoadSuccesFlag = false;
        // Message with info.
    }

    private void Update() {
        if(CharacterLoadSuccesFlag) {
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
                HandleKey4();
                HandleKey5();
                HandleUI();
                HandleTestAnim();
                HandleSendPlayerTransform();
            }
        }
    }

    private float timeSinceLastPacket = 0f;
    private float packetSendInterval = 0.2f;

    private void HandleSendPlayerTransform()
    {
        timeSinceLastPacket += Time.deltaTime;

        //if (CharacterIsRunning || CharacterIsRotating)

        if (SendTransform && (timeSinceLastPacket >= packetSendInterval))//&& (CharacterIsRunning || CharacterIsRotating)
        {
            CharacterTransformEmissary.instance.CommitSendPlayerCharacterTransfer(playerCharacter.VId,
            transform.position.x, transform.position.y, transform.position.z,
            transform.rotation.x, transform.rotation.y, transform.rotation.z, playerState);

            timeSinceLastPacket = 0f;
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
        if(InputWizard.instance.IsKey1Pressed() && playerState != AdventurerState.Casting) {
            playerState = AdventurerState.Casting;
            humanAnimator.AnimateBuff();
            
            var duration = HumanAnimator.NormalizeDuration(humanAnimator.BuffCastDuration);
            StartCoroutine(WaitForIdle(duration));
            StartCoroutine(skillController.WaitForThunderstruck(0f, mouseGroundPosition));
        }
    }

    private void HandleKey2() {
        if(InputWizard.instance.IsKey2Pressed() && playerState != AdventurerState.Casting) {
            playerState = AdventurerState.Casting;
            humanAnimator.AnimateSpellCast2();

            var duration = HumanAnimator.NormalizeDuration(humanAnimator.SpellCast2CastDuration);
            StartCoroutine(WaitForIdle(duration));
            StartCoroutine(skillController.WaitForMagicBullet(duration / 4, mouseGroundPosition));
        }
    }

    private void HandleKey3() {
        if(InputWizard.instance.IsKey3Pressed() && playerState != AdventurerState.Casting) {
            playerState = AdventurerState.Casting;
            humanAnimator.AnimateBuff();    

            var duration = HumanAnimator.NormalizeDuration(humanAnimator.BuffCastDuration);
            var skillStatesController = GetComponent<SkillStatesController>();
            skillStatesController.SummonMagicArmor(duration);
            StartCoroutine(WaitForIdle(duration));
        }
    }

    private void HandleKey4() {
        if(InputWizard.instance.IsKey4Pressed() && playerState != AdventurerState.Casting) {
            playerState = AdventurerState.Casting;
            humanAnimator.AnimateSpellCast2();    

            var duration = HumanAnimator.NormalizeDuration(humanAnimator.SpellCast2CastDuration);
            StartCoroutine(WaitForIdle(duration));
            StartCoroutine(skillController.WaitForMagicExplosion(0f, mouseGroundPosition));
        }
    }

    private void HandleKey5() {
        if(InputWizard.instance.IsKey5Pressed() && playerState != AdventurerState.Casting) {
            playerState = AdventurerState.Casting;
            humanAnimator.AnimateAttack2Handed();    

            var duration = HumanAnimator.NormalizeDuration(humanAnimator.Attack2HandedDuration);
            StartCoroutine(WaitForIdle(duration));
            StartCoroutine(skillController.WaitForNecroImpact(0.8f, mouseGroundPosition));
            StartCoroutine(skillController.WaitForNecroSlash(0.8f, mouseGroundPosition));
        }
    }


    private IEnumerator WaitForIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetToIdle();
    }

    private void HandleInteractions() {
        if(playerState == AdventurerState.Idle || playerState == AdventurerState.Running) {
            if(Physics.Raycast(transform.position, lookDirection, out RaycastHit raycastHit, interactionDistance)) {
                if(raycastHit.transform.TryGetComponent(out Interactible interactibleItem)) {
                    TurnOffTargetInteractibleVision();
                    targetInteractible = interactibleItem;
                    interactibleItem.OnVisionStart();
                    if(InputWizard.instance.IsInteractionKeyPressed()) {
                        humanAnimator.AnimateGathering();
                        playerState = AdventurerState.Casting;
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
            SendTransform = true;
            lookDirection = direction;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            previousPlayerState = AdventurerState.Running;
        }
    }

    private float CountDistanceToMouse() {
        var distance = Vector3.Distance(transform.position, mouseGroundPosition);
        return distance;
    }
 

    private void HandleRunning() {
        if (InputWizard.instance.IsRightClickPressed() 
        && playerState != AdventurerState.Casting 
        && CountDistanceToMouse() > minDistanceForRunning) {
            if (previousPlayerState != playerState)
            {
                SendTransform = true;
            }
            var movement = lookDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += movement;
            playerState = AdventurerState.Running;
            humanAnimator.AnimateRunning();
            previousPlayerState = playerState;
        }
    }

    private void HandleIdle() {
        if((!InputWizard.instance.IsRightClickPressed() 
        && playerState != AdventurerState.Casting)
        || (CountDistanceToMouse() <= minDistanceForRunning 
        && playerState != AdventurerState.Casting)) { 
            SendTransform = false;
            playerState = AdventurerState.Idle;
            humanAnimator.AnimateIdle();

            if(previousPlayerState != AdventurerState.Idle)
                SendTransform = true;

            previousPlayerState = playerState;
        } 
    }

    private void HandleMeleeAttack() {
        if(InputWizard.instance.IsLeftClickJustPressed() && playerState != AdventurerState.Casting) {
            playerState = AdventurerState.Casting;
            humanAnimator.AnimateMeleeAttack();
            var duration = HumanAnimator.NormalizeDuration(humanAnimator.MeleeAttackCastDuration);
            Invoke("ResetToIdle", duration);
            var weaponDD = GetComponentInChildren<WeaponDD>();
            weaponDD.FeedAndDealDamage(ownerCharacter: playerCharacter, damageDuration: duration);
        }
    }
    
    public void VisualizeDamage(Vector3 hitPosition, bool bloodSpill = true){
        if(bloodSpill) {
            var bloodSpillPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            VfxWizard.instance.SummonBloodSpillEffect(bloodSpillPosition, Quaternion.LookRotation(hitPosition));
        }
        if (playerCharacter.IsDead()) {
            characterCanvas.DisableHealthBarAndName();
        } else {
            characterCanvas.UpdateHealthBar(playerCharacter.CurrentHealth, playerCharacter.MaxHealth);
        }
    }

    private void ResetToIdle() {
        playerState = AdventurerState.Idle;
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

    /*public AdventurerState PlayerState {
        get { return playerState; }
        set { playerState = value; }
    }*/

/*    public enum PlayerState {
        Idle, Running, Casting
    }*/
}