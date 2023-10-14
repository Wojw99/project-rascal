using Assets.Code.Scripts.NetClient;
using Assets.Code.Scripts.NetClient.Attributes;
using Assets.Code.Scripts.NetClient.Emissary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code.Scripts
{
    public class AdventurerController : MonoBehaviour
    {
        //[SerializeField] private float moveSpeed = 5f;
        //[SerializeField] private float interactionDistance = 3f;

        public AdventurerCharacter adventurerCharacter { get; private set; }

        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject leftHand;
        [SerializeField] private GameObject rightHand;
        [SerializeField] private GameObject damageDealer;

        private CharacterCanvas characterCanvas;
        private NavMeshAgent navMeshAgent;
        //private CharacterState playerState = CharacterState.Idle;
        private HumanAnimator humanAnimator;

        private float rotationSpeed = 0.1f;
        private float moveSpeed = 0.1f;
        private float arrivalThreshold = 0.4f;

        private Vector3 targetPosition;
        private Vector3 targetRotation;

        /*private void Awake()
        {
            //adventurerCharacter = new AdventurerCharacter();
            //AdventurerTransformEmissary.instance.OnAdventurerTransformChanged += ChangeAdventurerTransform;
        }*/

        /*private void Update()
        {

            HandleRotation();
            HandleRunning();
        }*/
        /*public AdventurerController(TransformData Transform)
        {
            this.targetPosition = Transform.Position;
            this.targetRotation = Transform.Rotation;

            navMeshAgent = GetComponent<NavMeshAgent>();
            humanAnimator = GetComponent<HumanAnimator>();
            characterCanvas = GetComponentInChildren<CharacterCanvas>();
        }*/

        public static AdventurerController CreateAdventurerController(TransformData transformData, AdventurerAttributesData attrData)
        {
            GameObject adventurerObject = new GameObject("Adventurer");
            AdventurerController adventurerController = adventurerObject.AddComponent<AdventurerController>();

            adventurerController.adventurerCharacter.SetName(attrData.Name);
            adventurerController.adventurerCharacter.SetCurrentHealth(attrData.CurrentHealth);
            adventurerController.adventurerCharacter.SetMaxHealth(attrData.MaxHealth);
            adventurerController.adventurerCharacter.SetCurrentMana(attrData.CurrentMana);
            adventurerController.adventurerCharacter.SetMaxMana(attrData.MaxMana);
            // Inicjowanie pól
            adventurerController.targetPosition = transformData.Position;
            adventurerController.targetRotation = transformData.Rotation;

            adventurerController.navMeshAgent = adventurerObject.GetComponent<NavMeshAgent>();
            adventurerController.humanAnimator = adventurerObject.GetComponent<HumanAnimator>();
            adventurerController.characterCanvas = adventurerObject.GetComponentInChildren<CharacterCanvas>();

            return adventurerController;
        }

        public void HandleRotation(Vector3 Rotation)
        {
            targetRotation = Rotation;
            Quaternion targetRotationQuaternion = Quaternion.Euler(Rotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationQuaternion, Time.deltaTime * rotationSpeed);
        }

        public void HandleRunning(Vector3 Position)
        {
            targetPosition = Position;

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;

            Vector3 normalizedDirection = direction.normalized;

            transform.position += normalizedDirection * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) < arrivalThreshold)
            {
                // Gracz dotarł do celu, więc możesz podjąć odpowiednie działania, np. zatrzymać go
            }
        }

        public string getTransformInfo()
        {
            return targetPosition.ToString() + targetRotation.ToString();
        }


        /*public void SetTransform(Vector3 pos, Vector3 rot)
        {
            this.Position = pos;
            this.Rotation = rot;
        }*/
    }
}
