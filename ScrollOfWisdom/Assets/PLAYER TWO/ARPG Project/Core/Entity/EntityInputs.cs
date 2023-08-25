using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity Inputs")]
    public class EntityInputs : MonoBehaviour
    {
        [Tooltip("The Input Action Asset from the New Input System containing all the possible actions.")]
        public InputActionAsset actions;

        protected Entity m_entity;
        protected Transform m_target;
        protected Highlighter m_highlighter;
        protected Interactive m_interactive;
        protected Camera m_camera;

        protected InputAction m_setDestinationAction;
        protected InputAction m_attackModeAction;
        protected InputAction m_skillAction;
        protected InputAction m_consumeItem0;
        protected InputAction m_consumeItem1;
        protected InputAction m_consumeItem2;
        protected InputAction m_consumeItem3;
        protected InputAction m_selectSkill0;
        protected InputAction m_selectSkill1;
        protected InputAction m_selectSkill2;
        protected InputAction m_selectSkill3;
        protected InputAction m_directionalMovement;
        protected InputAction m_attackAction;
        protected InputAction m_interactAction;

        protected bool m_holdMove;
        protected bool m_holdAttack;
        protected bool m_holdSkill;
        protected bool m_attackMode;
        protected bool m_pointerOverUi;

        protected float m_lockDirectionTime;

        protected RaycastHit[] m_hitResults = new RaycastHit[32];

        public new Camera camera
        {
            get
            {
                if (m_camera == null)
                    m_camera = Camera.main;

                return m_camera;
            }
        }

        public bool moveDirectionLocked => m_lockDirectionTime > 0;

        protected virtual GamePause m_gamePause => GamePause.instance;

        protected EntityAreaScanner m_areaScanner;

        protected virtual void InitializeEntity() => m_entity = GetComponent<Entity>();
        protected virtual void InitializeAreaScanner() => m_areaScanner = GetComponent<EntityAreaScanner>();

        protected virtual void InitializeActions()
        {
            m_setDestinationAction = actions["Set Destination"];
            m_skillAction = actions["Skill"];
            m_attackModeAction = actions["Attack Mode"];
            m_consumeItem0 = actions["Consume Item 0"];
            m_consumeItem1 = actions["Consume Item 1"];
            m_consumeItem2 = actions["Consume Item 2"];
            m_consumeItem3 = actions["Consume Item 3"];
            m_selectSkill0 = actions["Select Skill 0"];
            m_selectSkill1 = actions["Select Skill 1"];
            m_selectSkill2 = actions["Select Skill 2"];
            m_selectSkill3 = actions["Select Skill 3"];
            m_directionalMovement = actions["Directional Movement"];
            m_attackAction = actions["Attack"];
            m_interactAction = actions["Interact"];
        }

        protected virtual void InitializeCallbacks()
        {
            m_entity.onDie.AddListener(() => this.enabled = false);
        }

        protected virtual void InitializeActionsCallbacks()
        {
            m_setDestinationAction.performed += OnSetDestination;
            m_setDestinationAction.canceled += OnSetDestinationCancelled;
            m_skillAction.performed += OnSkill;
            m_skillAction.canceled += OnSkillCancelled;
            m_attackModeAction.performed += OnAttackMode;
            m_attackModeAction.canceled += OnAttackModeCancelled;
            m_consumeItem0.performed += OnConsumeItem0;
            m_consumeItem1.performed += OnConsumeItem1;
            m_consumeItem2.performed += OnConsumeItem2;
            m_consumeItem3.performed += OnConsumeItem3;
            m_selectSkill0.performed += OnSelectSkill0;
            m_selectSkill1.performed += OnSelectSkill1;
            m_selectSkill2.performed += OnSelectSkill2;
            m_selectSkill3.performed += OnSelectSkill3;
            m_attackAction.performed += OnAttack;
            m_attackAction.canceled += OnAttackCancelled;
            m_interactAction.performed += OnInteract;
        }

        protected virtual void FinalizeActionCallbacks()
        {
            m_setDestinationAction.performed -= OnSetDestination;
            m_setDestinationAction.canceled -= OnSetDestinationCancelled;
            m_skillAction.performed -= OnSkill;
            m_skillAction.canceled -= OnSkillCancelled;
            m_attackModeAction.performed -= OnAttackMode;
            m_attackModeAction.canceled -= OnAttackModeCancelled;
            m_consumeItem0.performed -= OnConsumeItem0;
            m_consumeItem1.performed -= OnConsumeItem1;
            m_consumeItem2.performed -= OnConsumeItem2;
            m_consumeItem3.performed -= OnConsumeItem3;
            m_selectSkill0.performed -= OnSelectSkill0;
            m_selectSkill1.performed -= OnSelectSkill1;
            m_selectSkill2.performed -= OnSelectSkill2;
            m_selectSkill3.performed -= OnSelectSkill3;
            m_attackAction.performed -= OnAttack;
            m_attackAction.canceled -= OnAttackCancelled;
            m_interactAction.performed -= OnInteract;
        }

        public virtual bool MouseRaycast(out RaycastHit closestHit, int layer = Physics.DefaultRaycastLayers)
        {
            closestHit = new RaycastHit();

            if (m_pointerOverUi) return false;

            var position = Mouse.current.position.ReadValue();
            var ray = camera.ScreenPointToRay(position);
            var hits = Physics.RaycastNonAlloc(ray, m_hitResults, Mathf.Infinity, layer);
            var closestPoint = hits > 0 ? m_hitResults[0].point : Vector3.zero;

            for (int i = 0; i < hits; i++)
            {
                if (m_hitResults[i].transform == transform)
                    continue;
                else if (m_hitResults[i].collider.CompareTag(GameTags.Collectible))
                {
                    closestHit = m_hitResults[i];
                    break;
                }
                else if (closestHit.collider == null ||
                    Vector3.Distance(ray.origin, m_hitResults[i].point) <=
                    Vector3.Distance(ray.origin, closestPoint))
                {
                    closestHit = m_hitResults[i];
                    closestPoint = m_hitResults[i].point;

                    if (!closestHit.collider.CompareTag(GameTags.Untagged))
                        closestHit.point = closestHit.transform.position;
                }
            }

            return closestHit.collider != null;
        }

        protected virtual bool TrySetTarget(Collider collider)
        {
            if (!collider.CompareTag(GameTags.Untagged))
            {
                m_target = collider.transform;
                return true;
            }

            return false;
        }

        protected virtual bool TryRefreshTarget()
        {
#if UNITY_STANDALONE || UNITY_WEBGL
            return MouseRaycast(out var hit) && TrySetTarget(hit.collider);
#else
            m_target = m_areaScanner.GetClosestTarget();
            return m_target;
#endif
        }

        protected virtual bool IsTargetAttackable() => GameTags.IsTarget(m_target?.gameObject);
        protected virtual bool IsTargetInteractive() => GameTags.IsInteractive(m_target?.gameObject);

        protected virtual Vector3 GetMouseDirection()
        {
            var mouse = Mouse.current.position.ReadValue();
            var center = new Vector2(Screen.width, Screen.height) / 2;
            var direction = (mouse - center).normalized;
            return new Vector3(direction.x, 0, direction.y);
        }

        protected virtual void OnSetDestination(InputAction.CallbackContext _)
        {
#if UNITY_STANDALONE || UNITY_WEBGL
            if (m_gamePause.isPaused) return;

            m_target = null;
            m_entity.useSkill = false;

            if (MouseRaycast(out var hit))
            {
                var foundTarget = TrySetTarget(hit.collider);

                if (IsTargetAttackable())
                {
                    m_holdAttack = true;
                    return;
                }
                else if (IsTargetInteractive())
                {
                    m_entity.targetInteractive = m_target.GetComponent<Interactive>();
                    m_entity.MoveTo(m_target.position);
                    return;
                }

                m_holdMove = true;
            }
#endif
        }

        protected virtual void OnSetDestinationCancelled(InputAction.CallbackContext _) =>
            m_holdMove = m_holdAttack = false;

        protected virtual void OnSkill(InputAction.CallbackContext _)
        {
            if (m_gamePause.isPaused || !m_entity.skills.CanUseSkill()) return;

            m_entity.useSkill = true;

#if UNITY_STANDALONE || UNITY_WEBGL
            if (MouseRaycast(out var hit))
            {
                m_holdSkill = true;
                TrySetTarget(hit.collider);
            }
#else
            m_holdSkill = true;
#endif
        }

        protected virtual void OnSkillCancelled(InputAction.CallbackContext _) => m_holdSkill = false;
        protected virtual void OnAttackMode(InputAction.CallbackContext _) => m_attackMode = true;
        protected virtual void OnAttackModeCancelled(InputAction.CallbackContext _) => m_attackMode = false;
        protected virtual void OnConsumeItem0(InputAction.CallbackContext _) => m_entity.items.ConsumeItem(0);
        protected virtual void OnConsumeItem1(InputAction.CallbackContext _) => m_entity.items.ConsumeItem(1);
        protected virtual void OnConsumeItem2(InputAction.CallbackContext _) => m_entity.items.ConsumeItem(2);
        protected virtual void OnConsumeItem3(InputAction.CallbackContext _) => m_entity.items.ConsumeItem(3);
        protected virtual void OnSelectSkill0(InputAction.CallbackContext _) => m_entity.skills.ChangeTo(0);
        protected virtual void OnSelectSkill1(InputAction.CallbackContext _) => m_entity.skills.ChangeTo(1);
        protected virtual void OnSelectSkill2(InputAction.CallbackContext _) => m_entity.skills.ChangeTo(2);
        protected virtual void OnSelectSkill3(InputAction.CallbackContext _) => m_entity.skills.ChangeTo(3);

        protected virtual void OnAttack(InputAction.CallbackContext _)
        {
            m_entity.useSkill = false;
            m_holdAttack = m_attackMode = true;
        }

        protected virtual void OnAttackCancelled(InputAction.CallbackContext _) => m_holdAttack = m_attackMode = false;

        protected virtual void OnInteract(InputAction.CallbackContext _)
        {
            m_interactive = m_areaScanner.GetClosestInteractiveObject();

            if (m_interactive)
            {
                m_entity.targetInteractive = m_interactive;
                m_entity.MoveTo(m_interactive.transform.position);
            }
        }

        protected virtual void HandlePointer() =>
            m_pointerOverUi = EventSystem.current.IsPointerOverGameObject();

        protected virtual void HandleMovement()
        {
#if UNITY_STANDALONE || UNITY_WEBGL
            if (m_holdMove)
            {
                m_entity.lookDirection = GetMouseDirection();

                if (m_attackMode)
                    m_entity.FreeAttack();
                else if (MouseRaycast(out var hit))
                    m_entity.MoveTo(hit.point);
            }
#endif
        }

        protected virtual void HandleAttack()
        {
            if (m_holdAttack || m_holdSkill)
            {
                if (m_attackMode || m_holdSkill && !m_entity.skills.RequireTarget())
                {
#if UNITY_STANDALONE || UNITY_WEBGL
                    m_entity.lookDirection = GetMouseDirection();
                    m_entity.FreeAttack();
#else
                    if (IsTargetActive() || TryRefreshTarget())
                        m_entity.MoveToAttack(m_target);
                    else
                        m_entity.FreeAttack();
#endif
                }
                else if (IsTargetActive() || TryRefreshTarget() && IsTargetAttackable())
                {
                    m_entity.MoveToAttack(m_target);
                }
                else
                {
                    m_holdMove = !m_holdSkill;
                    m_holdAttack = false;
                }
            }
        }

        protected virtual void HandleHighlight()
        {
#if UNITY_STANDALONE || UNITY_WEBGL
            m_highlighter?.SetHighlight(false);

            if (!MouseRaycast(out var hit) ||
                !hit.collider.TryGetComponent(out m_highlighter))
                return;

            m_highlighter?.SetHighlight(true);
#endif
        }

        protected virtual void HandleMoveDirectionUnlock()
        {
            if (!moveDirectionLocked) return;

            m_lockDirectionTime -= Time.deltaTime;
            m_lockDirectionTime = Mathf.Max(m_lockDirectionTime, 0);
        }

        protected virtual bool IsTargetActive()
        {
            if (m_target && m_target.TryGetComponent(out Entity entity))
                return !entity.isDead;

            return false;
        }

        public virtual Vector3 GetMoveDirection()
        {
            if (moveDirectionLocked)
                return Vector3.zero;

            var raw = m_directionalMovement.ReadValue<Vector2>();
            var direction = GetAxisWithCrossDeadzone(raw);

            if (direction.sqrMagnitude > 0)
            {
                var rotation = Quaternion.AngleAxis(camera.transform.eulerAngles.y, Vector3.up);
                direction = rotation * direction;
                direction = direction.normalized;
            }

            return direction.normalized;
        }

        /// <summary>
        /// Locks the move direction making the Entity unable to move for a while.
        /// </summary>
        /// <param name="duration">The duration in seconds to keep direction locked.</param>
        public virtual void LockMoveDirection(float duration = 0.5f) => m_lockDirectionTime = duration;

        /// <summary>
		/// Remaps a given axis considering the Input System's default deadzone.
		/// This method uses a cross shape instead of a circle one to evaluate the deadzone range.
		/// </summary>
		/// <param name="axis">The axis you want to remap.</param>
		public virtual Vector3 GetAxisWithCrossDeadzone(Vector2 axis)
        {
            var deadzone = InputSystem.settings.defaultDeadzoneMin;
            axis.x = Mathf.Abs(axis.x) > deadzone ? RemapToDeadzone(axis.x, deadzone) : 0;
            axis.y = Mathf.Abs(axis.y) > deadzone ? RemapToDeadzone(axis.y, deadzone) : 0;
            return new Vector3(axis.x, 0, axis.y);
        }

        /// <summary>
		/// Remaps a value to a 0-1 range considering a given deadzone.
		/// </summary>
		/// <param name="value">The value you wants to remap.</param>
		/// <param name="deadzone">The minimum deadzone value.</param>
		protected float RemapToDeadzone(float value, float deadzone) => (value - deadzone) / (1 - deadzone);

        protected virtual void Start()
        {
            InitializeEntity();
            InitializeAreaScanner();
            InitializeActions();
            InitializeCallbacks();
            InitializeActionsCallbacks();
        }

        protected virtual void Update()
        {
            HandlePointer();
            HandleMovement();
            HandleAttack();
            HandleHighlight();
            HandleMoveDirectionUnlock();
        }

        protected virtual void OnEnable() => actions.Enable();
        protected virtual void OnDisable() => actions.Disable();
        protected virtual void OnDestroy() => FinalizeActionCallbacks();
    }
}
