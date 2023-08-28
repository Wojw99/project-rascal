using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity Camera")]
    public class EntityCamera : MonoBehaviour
    {
        [Header("Inputs Settings")]
        [Tooltip("The Input Action Asset from the New Input System containing all the possible actions.")]
        public InputActionAsset actions;

        [Header("General Settings")]
        [Tooltip("The angle of the Camera.")]
        public float angle = 45f;

        [Tooltip("The minimum distance the Camera can reach from the target.")]
        public float minDistance = 5f;

        [Tooltip("The maximum distance the Camera can reach from the target.")]
        public float maxDistance = 20f;

        [Header("Scroll Settings")]
        [Tooltip("A multiplier value to speed up the zoom scroll speed.")]
        public float zoomScrollMultiplier = 2f;

        [Tooltip("A multiplier value to speed up the rotation scroll speed.")]
        public float rotationScrollMultiplier = 10f;

        [Tooltip("The time in seconds it takes for the scroll to reach its target value.")]
        public float scrollSmoothTime = 0.1f;

        protected float m_distance;
        protected float m_rotation;
        protected float m_targetDistance;
        protected float m_targetRotation;
        protected float m_distanceVelocity;
        protected float m_rotationVelocity;

        protected bool m_scrollModifier;
        protected bool m_pointerOverUi;

        protected InputAction m_scrollAction;
        protected InputAction m_scrollModifierAction;

        protected Entity m_entity;

        protected virtual void InitializeEntity() => m_entity = Level.instance.player;

        protected virtual void InitializeActions()
        {
            m_scrollAction = actions["Scroll"];
            m_scrollModifierAction = actions["Scroll Modifier"];
        }

        protected virtual void InitializeActionsCallbacks()
        {
            m_scrollAction.performed += _ => OnScroll();
            m_scrollModifierAction.performed += _ => m_scrollModifier = true;
            m_scrollModifierAction.canceled += _ => m_scrollModifier = false;
        }

        protected virtual void HandleValueSmoothness()
        {
            m_distance = Mathf.SmoothDampAngle(m_distance, m_targetDistance,
                ref m_distanceVelocity, scrollSmoothTime);
            m_rotation = Mathf.SmoothDampAngle(m_rotation, m_targetRotation,
                ref m_rotationVelocity, scrollSmoothTime);
        }

        protected virtual void HandleTransform()
        {
            var target = m_entity.transform.position;
            var rotation = Quaternion.Euler(angle, m_rotation, 0);
            transform.position = rotation * new Vector3(0, 0, -m_distance) + target;
            transform.rotation = rotation;
        }

        protected virtual void HandleMovement()
        {
            HandleValueSmoothness();
            HandleTransform();
        }

        protected virtual void HandlePointer() =>
            m_pointerOverUi = EventSystem.current.IsPointerOverGameObject();

        protected virtual void OnScroll()
        {
            if (m_pointerOverUi) return;

            var scroll = m_scrollAction.ReadValue<float>();

            if (m_scrollModifier)
            {
                m_targetRotation = m_rotation - scroll * rotationScrollMultiplier;
            }
            else
            {
                m_targetDistance = m_distance - scroll * zoomScrollMultiplier;
                m_targetDistance = Mathf.Clamp(m_targetDistance, minDistance, maxDistance);
            }
        }

        /// <summary>
        /// Resets the Camera to its initial rotation and distance.
        /// </summary>
        public virtual void Reset()
        {
            m_rotation = m_targetRotation = 0;
            m_distance = m_targetDistance = maxDistance;
        }

        protected virtual void Start()
        {
            InitializeEntity();
            InitializeActions();
            InitializeActionsCallbacks();
            Reset();
        }

        protected virtual void LateUpdate()
        {
            HandlePointer();
            HandleMovement();
        }

        protected virtual void OnEnable() => actions.Enable();
        protected virtual void OnDisable() => actions.Disable();
    }
}
