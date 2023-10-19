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
using System.Collections;

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

        private Coroutine movementCoroutine;

        float lastTargetTransformTime = 0f;

        private void Awake()
        {
            adventurerCharacter = GetComponent<AdventurerCharacter>();
            humanAnimator = GetComponent<HumanAnimator>();
            
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if(targetPosition != transform.position)
            {
                if (movementCoroutine == null)
                {
                    Debug.Log($"NetworkLatency =  {NetworkTimeSyncEmissary.instance.GetNetworkLatency() * 1000} ms.");

                    movementCoroutine = StartCoroutine(MoveTowardsPosition(targetPosition, 
                        CalculateInterpolationTime(2 + NetworkTimeSyncEmissary.instance.GetNetworkLatency())));
                }
            }
           /* else
            {
                humanAnimator.AnimateIdle();
            }*/
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

        /*private void HandleIdle()
        {
            if (adventurerState != AdventurerState.Casting && adventurerState != AdventurerState.Running)
            {
                //adventurerState = AdventurerState.Idle;
                humanAnimator.AnimateIdle();
            }
        }*/

        /* public void HandleRotation()
         {
             Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotation);
             transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationQuaternion, Time.deltaTime * rotationSpeed);

             //if (targetRotationQuaternion != transform.rotation)
             //{
             //}
         }*/

        /* public void HandleRunning()
         {
             if(adventurerState == AdventurerState.Running)
             {
                 if(targetPosition != transform.position)
                 {
                     Vector3 direction = targetPosition - transform.position;
                     direction.y = 0f;

                     Vector3 normalizedDirection = direction.normalized;

                     transform.position += normalizedDirection * moveSpeed * Time.deltaTime;

                     if (Vector3.Distance(transform.position, targetPosition) < arrivalThreshold)
                     {
                         // Adventurer comes to target position.
                         adventurerState = AdventurerState.Idle;
                     }

                     humanAnimator.AnimateRunning();
                     //adventurerState = AdventurerState.Running;
                 }
             }
         }*/

        public IEnumerator MoveTowardsPosition(Vector3 targetPosition, float time)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.position;

            humanAnimator.AnimateRunning();
            
            while (elapsedTime < time)
            {
                Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotation);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationQuaternion, Time.deltaTime * rotationSpeed);
                transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Debug.Log("WIELKI TEST WSZYSTKIEGO");
            movementCoroutine = null;
            transform.position = targetPosition; // Zapewnij dokładne ustawienie na koniec
            humanAnimator.AnimateIdle();
        }

        private float CalculateInterpolationTime(float latency)
        {
            // Dostosuj czas interpolacji na podstawie opóźnienia sieciowego
            // Możesz dostosować te wartości do uzyskania pożądanego efektu
            float minInterpolationTime = 0.05f;
            float maxInterpolationTime = 0.2f;

            // Przykładowe skalowanie w zależności od opóźnienia
            float scale = Mathf.Clamp01(latency / 0.1f); // Skalowanie między 0 a 1 w zależności od opóźnienia

            // Zastosuj skalowanie do czasu interpolacji
            float interpolationTime = Mathf.Lerp(minInterpolationTime, maxInterpolationTime, scale);

            return interpolationTime;
        }

        public string getTransformInfo()
        {
            return targetPosition.ToString() + targetRotation.ToString();
        }


        public void SetTargetTransform(Vector3 pos, Vector3 rot)
        {
            //Debug.Log("SetTransform Called");

            if (movementCoroutine != null)
            {
                StopCoroutine(movementCoroutine);
                movementCoroutine = null;
            }

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
