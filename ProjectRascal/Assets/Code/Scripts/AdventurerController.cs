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
        //private HumanAnimator humanAnimator;

        private Renderer renderer;

        private float rotationSpeed = 0.1f;
        private float moveSpeed = 0.1f;
        private float arrivalThreshold = 0.4f;

        private Vector3 targetPosition;
        private Vector3 targetRotation;

        private void Awake()
        {
            adventurerCharacter = GetComponent<AdventurerCharacter>();

            //Material material = new Material(Shader.Find("Standard"));
            //material.mainTexture = adventurerTexture;
            
            //renderer = GetComponent<Renderer>();
            //renderer.material = material;
        }

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

        /*private void Awake()
        {
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            humanAnimator = gameObject.GetComponent<HumanAnimator>();
            characterCanvas = gameObject.GetComponentInChildren<CharacterCanvas>();
        }*/

        private void Update()
        {
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


        public void HandleRotation()
        {
           /* if(adventurerCharacter.VId != -1)
            {
                Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotation);
            
                if (targetRotationQuaternion != transform.rotation)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationQuaternion, Time.deltaTime * rotationSpeed);
                }

                Debug.Log("Rotacja adventurera = " + transform.rotation);
            }*/
        }

        public void HandleRunning()
        {
            /*if(adventurerCharacter.VId != -1)
            {
                if(targetPosition != transform.position)
                {
                    Vector3 direction = targetPosition - transform.position;
                    direction.y = 0f;

                    Vector3 normalizedDirection = direction.normalized;

                    transform.position += normalizedDirection * moveSpeed * Time.deltaTime;

                    if (Vector3.Distance(transform.position, targetPosition) < arrivalThreshold)
                    {

                    }

                    Debug.Log("Pozycja adventurera = " + transform.position + "Pozycja zadana = " + targetPosition + "VId = " + adventurerCharacter.VId);
                }
            }*/
        }

        public string getTransformInfo()
        {
            return targetPosition.ToString() + targetRotation.ToString();
        }


        public void SetTransform(Vector3 pos, Vector3 rot)
        {
            Debug.Log("SetTransform called");
            this.targetPosition = pos;
            this.targetRotation = rot;
        }
    }
}
