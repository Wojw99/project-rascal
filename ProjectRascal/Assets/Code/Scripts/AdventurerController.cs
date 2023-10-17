using Assets.Code.Scripts.NetClient;
using Assets.Code.Scripts.NetClient.Attributes;
using Assets.Code.Scripts.NetClient.Emissary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using NetworkCore.NetworkUtility;

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

        [SerializeField] private Texture2D adventurerTexture;

        //private CharacterCanvas characterCanvas;
        //private NavMeshAgent navMeshAgent;
        //private CharacterState playerState = CharacterState.Idle;
        private HumanAnimator humanAnimator;

        private float rotationSpeed = 0.1f;
        private float moveSpeed = 5f;
        private float arrivalThreshold = 0.4f;

        AdventurerState adventurerState = AdventurerState.Idle;

        private Vector3 targetPosition;
        private Vector3 targetRotation;

        private void Awake()
        {
            adventurerCharacter = GetComponent<AdventurerCharacter>();
            humanAnimator = GetComponent<HumanAnimator>();
            
        }

        private void Update()
        {
            HandleIdle();
            HandleRotation();
            HandleRunning();
        }

        public void InitializeData(AdventurerAttributesData attrData, TransformData transformData)
        {
            adventurerCharacter = GetComponent<AdventurerCharacter>();
            adventurerCharacter.SetName(attrData.Name);
            adventurerCharacter.SetCurrentHealth(attrData.CurrentHealth);
            adventurerCharacter.SetMaxHealth(attrData.MaxHealth);
            adventurerCharacter.SetCurrentMana(attrData.CurrentMana);
            adventurerCharacter.SetMaxMana(attrData.MaxMana);
            targetPosition = transformData.Position;
            targetRotation = transformData.Rotation;
        }



        /*public static AdventurerController CreateAdventurerController(TransformData transformData, AdventurerAttributesData attrData)
        {
            GameObject adventurerObject = new GameObject("Adventurer_"+attrData.CharacterVId);//
            AdventurerController adventurerController = adventurerObject.AddComponent<AdventurerController>();

            adventurerController.adventurerCharacter.SetName(attrData.Name);
            adventurerController.adventurerCharacter.SetCurrentHealth(attrData.CurrentHealth);
            adventurerController.adventurerCharacter.SetMaxHealth(attrData.MaxHealth);
            adventurerController.adventurerCharacter.SetCurrentMana(attrData.CurrentMana);
            adventurerController.adventurerCharacter.SetMaxMana(attrData.MaxMana);
            adventurerController.targetPosition = transformData.Position;
            adventurerController.targetRotation = transformData.Rotation;

            return adventurerController;
        }*/

        private void HandleIdle()
        {
            if (adventurerState != AdventurerState.Casting && adventurerState != AdventurerState.Running)
            {
                //adventurerState = AdventurerState.Idle;
                humanAnimator.AnimateIdle();
            }
        }

        public void HandleRotation()
        {
            Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationQuaternion, Time.deltaTime * rotationSpeed);
            
            //if (targetRotationQuaternion != transform.rotation)
            //{
            //}
        }

        public void HandleRunning()
        {
            if(adventurerState == AdventurerState.Running)
            {
                if(targetPosition != transform.position)
                {
                    Vector3 direction = targetPosition - transform.position;
                    direction.y = 0f;

                    Vector3 normalizedDirection = direction.normalized;

                    transform.position += normalizedDirection * moveSpeed * Time.deltaTime;

                    /*if (Vector3.Distance(transform.position, targetPosition) < arrivalThreshold)
                    {
                        // Adventurer comes to target position.
                        adventurerState = AdventurerState.Idle;
                    }*/

                    humanAnimator.AnimateRunning();
                    //adventurerState = AdventurerState.Running;
                }
            }
        }

        public string getTransformInfo()
        {
            return targetPosition.ToString() + targetRotation.ToString();
        }


        public void SetTransform(Vector3 pos, Vector3 rot)
        {
            Debug.Log("SetTransform Called");
            this.targetPosition = pos;
            this.targetRotation = rot;
            //this.adventurerState = AdventurerState.Running;
        }

        public void SetAdventurerState(AdventurerState state)
        {
            adventurerState = state;
        }
    }
}
