using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity")]
    public class Entity : MonoBehaviour
    {
        public UnityEvent onAttack;
        public UnityEvent onMagicAttack;
        public UnityEvent<EntityAttackType> onPerformAttack;
        public UnityEvent onTargetSet;
        public UnityEvent<int, Vector3, bool> onDamage;
        public UnityEvent onDie;
        public UnityEvent onRevive;

        [Tooltip("The tag of Game Objects that this Entity identifies as potential targets.")]
        public string targetTag;

        [Tooltip("The maximum speed this Entity will use to move around.")]
        public float moveSpeed = 6f;

        [Tooltip("The rotation in degrees per second the Entity will rotate to face directions.")]
        public float rotationSpeed = 720f;

        [Tooltip("The default maximum distance to perform attacks. Internally, this can be overridden by weapons.")]
        public float attackDistance = 1f;

        [Tooltip("The force applied toward the ground to keep the Entity grounded.")]
        public float groundSnapForce = 10f;

        [Tooltip("The reference to the Hitbox used to perform melee attacks.")]
        public EntityHitbox hitbox;

        protected Collider[] m_colliders;
        protected CharacterController m_controller;
        protected Rigidbody m_rigidbody;
        protected EntityAnimator m_animator;
        protected EntityStatsManager m_stats;

        protected float m_lastHitTime;

        protected int m_waypointsSize;
        protected int m_currentWaypoint = -1;
        protected NavMeshPath m_path;
        protected Vector3[] m_waypoints = new Vector3[k_waypointsSize];

        protected List<Entity> m_damageFrom = new List<Entity>();

        public EntityStateMachine states { get; protected set; }
        public EntitySkillManager skills { get; protected set; }
        public EntityInputs inputs { get; protected set; }
        public EntityItemManager items { get; protected set; }
        public EntityInventory inventory { get; protected set; }

        public Vector3 position => transform.position;

        public CharacterController controller
        {
            get
            {
                InitializeController();
                return m_controller;
            }
        }

        public EntityStatsManager stats
        {
            get
            {
                InitializeStats();
                return m_stats;
            }
        }

        /// <summary>
        /// The current velocity of the Entity.
        /// </summary>
        /// <value></value>
        public Vector3 velocity { get; set; }

        /// <summary>
        /// The current X and Z velocity of the Entity.
        /// </summary>
        public Vector3 lateralVelocity
        {
            get { return new Vector3(velocity.x, 0, velocity.z); }
            set { velocity = new Vector3(value.x, velocity.y, value.z); }
        }

        /// <summary>
        /// The current Y velocity of the Entity.
        /// </summary>
        public Vector3 verticalVelocity
        {
            get { return new Vector3(0, velocity.y, 0); }
            set { velocity = new Vector3(velocity.x, value.y, velocity.z); }
        }

        public Vector3 lookDirection { get; set; }
        public Vector3 initialPosition { get; protected set; }

        public Transform target { get; protected set; }

        public float attackDuration { get; protected set; }
        public float skillDuration { get; protected set; }

        public Interactive targetInteractive { get; set; }

        protected const float k_initialRadius = 0.25f;
        protected const float k_minDistanceToMove = 0.25f;
        protected const float k_maxHitRate = 0.15f;
        protected const int k_waypointsSize = 256;

        /// <summary>
        /// Returns true if the Entity is active (responding to movement).
        /// </summary>
        public bool isActive => controller.enabled;

        /// <summary>
        /// Returns true if the Entity have any health.
        /// </summary>
        public bool isDead => stats.health == 0;

        /// <summary>
        /// Returns true if the Entity is performing an attack.
        /// </summary>
        public bool isAttacking => states.IsCurrent<AttackEntityState>() || states.IsCurrent<UseSkillEntityState>();

        /// <summary>
        /// If true, makes the Entity use the current Skill in the next attack.
        /// </summary>
        public bool useSkill { get; set; }

        /// <summary>
        /// If true, allows the Entity to move to the next destination, which
        /// can be the current assigned target or a waypoint.
        /// </summary>
        public bool canUpdateDestination { get; set; } = true;

        /// <summary>
        /// If true, the Entity is controlled by the Player's inputs.
        /// </summary>
        public bool usingInputs => inputs;

        protected virtual void InitializeController()
        {
            if (m_controller) return;

            if (!TryGetComponent(out m_controller))
            {
                m_controller = gameObject.AddComponent<CharacterController>();
                m_controller.radius = k_initialRadius;
            }

            m_controller.minMoveDistance = 0;
            m_controller.skinWidth = m_controller.radius * 0.1f;
            initialPosition = transform.position;
        }

        protected virtual void InitializeRigidbody()
        {
            if (!TryGetComponent(out m_rigidbody))
            {
                m_rigidbody = gameObject.AddComponent<Rigidbody>();
            }

            m_rigidbody.isKinematic = true;
        }

        protected virtual void InitializeStateMachine()
        {
            states = new EntityStateMachine(this);
            states.ChangeTo<IdleEntityState>();
        }

        protected virtual void InitializeStats()
        {
            if (m_stats) return;

            m_stats = GetComponent<EntityStatsManager>();
        }

        protected virtual void InitializeSkillManager() => skills = GetComponent<EntitySkillManager>();
        protected virtual void InitializeItemManager() => items = GetComponent<EntityItemManager>();
        protected virtual void InitializeInventory() => inventory = GetComponent<EntityInventory>();
        protected virtual void InitializePath() => m_path = new NavMeshPath();
        protected virtual void InitializeColliders() => m_colliders = GetComponents<Collider>();
        protected virtual void InitializeInputs() => inputs = GetComponent<EntityInputs>();
        protected virtual void InitializeAnimator() => m_animator = GetComponent<EntityAnimator>();

        /// <summary>
        /// Moves the Entity to a specific position and rotation in the world space.
        /// </summary>
        /// <param name="position">The position to teleport in world space.</param>
        /// <param name="rotation">The rotation to teleport in world space.</param>
        public virtual void Teleport(Vector3 position, Quaternion rotation)
        {
            controller.enabled = false;
            m_currentWaypoint = -1;
            velocity = Vector3.zero;
            m_waypoints = new Vector3[k_waypointsSize];
            transform.SetPositionAndRotation(position, rotation);
            controller.enabled = true;
        }

        /// <summary>
        /// Returns the current distance to the assigned target.
        /// </summary>
        public virtual float GetDistanceToDestination()
        {
            var waypointIdx = m_waypointsSize - 1;

            if (m_waypoints == null || waypointIdx <= m_waypoints.Length) return 0;

            var waypoint = m_waypoints[m_waypointsSize - 1];
            var destination = new Vector3(waypoint.x, position.y, waypoint.z);

            return Vector3.Distance(position, destination);
        }

        /// <summary>
        /// Returns the distance to a point in world space without considering its Y position.
        /// </summary>
        /// <param name="point">The point you want to get the distance to.</param>
        public virtual float GetDistanceTo(Vector3 point)
        {
            point.y = position.y;
            return Vector3.Distance(position, point);
        }

        /// <summary>
        /// Returns the distance to the current assigned target.
        /// Returns zero if the current target is not set (null).
        /// </summary>
        public virtual float GetDistanceToTarget()
        {
            if (!target) return 0;

            return GetDistanceTo(target.position);
        }

        /// <summary>
        /// Returns a vector towards a point in world space without considering its Y position.
        /// </summary>
        /// <param name="point">The point you want the direction.</param>
        public virtual Vector3 GetDirectionTo(Vector3 point)
        {
            point.y = transform.position.y;
            return (point - transform.position).normalized;
        }

        /// <summary>
        /// Returns a vector towards the current target without considering its Y position.
        /// </summary>
        public virtual Vector3 GetDirectionToTarget()
        {
            if (!target) return transform.forward;

            return GetDirectionTo(target.position);
        }

        /// <summary>
        /// Returns the current allowed attack distance, based on the current skill and
        /// the current equipped weapon, if any. Otherwise, it returns the default attack distance.
        /// </summary>
        public virtual float GetAttackDistance()
        {
            if (useSkill && skills.IsAttack()) return skills.GetSkillAttack().minAttackDistance;
            if (items.IsUsingBow()) return items.GetBow().shotDistance;

            return attackDistance;
        }

        /// <summary>
        /// Returns true if the Entity is close enough to attack the assigned target based on
        /// the current attack distance (evaluated by the current skill and equipped weapon).
        /// </summary>
        public virtual bool IsCloseToAttackTarget() => GetDistanceToTarget() <= GetAttackDistance();

        /// <summary>
        /// Moves the Entity along the current NavMesh path while updating the waypoints.
        /// </summary>
        public virtual void HandleWaypointMovement()
        {
            if (m_currentWaypoint < 0) return;

            var waypoint = m_waypoints[m_currentWaypoint + 1];
            var point = new Vector3(waypoint.x, position.y, waypoint.z);
            var direction = point - position;
            var distance = direction.magnitude;

            direction = direction.normalized;
            lateralVelocity = direction * moveSpeed;

            if (distance <= 0.1f)
            {
                m_currentWaypoint++;

                if (m_currentWaypoint >= m_waypointsSize - 1)
                {
                    m_currentWaypoint = -1;
                    lateralVelocity = Vector3.zero;
                }
            }
        }

        /// <summary>
        /// Calculates a NavMesh Path to a given destination to be used by the waypoint movement.
        /// </summary>
        /// <param name="destination">The destination you want to reach.</param>
        /// <returns>Returns true if a complete or partial path was found.</returns>
        public virtual bool TryCalculatePath(Vector3 destination)
        {
            if (NavMesh.CalculatePath(position, destination, NavMesh.AllAreas, m_path))
            {
                m_currentWaypoint = 0;
                m_waypointsSize = m_path.GetCornersNonAlloc(m_waypoints);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Moves the Entity to the current assigned target.
        /// </summary>
        public virtual void MoveToTarget() => MoveTo(target.position);

        /// <summary>
        /// Moves the Entity to a given point in world space.
        /// </summary>
        /// <param name="point">The point you want the Entity to move to.</param>
        public virtual void MoveTo(Vector3 point)
        {
            if (canUpdateDestination &&
                GetDistanceTo(point) > k_minDistanceToMove &&
                TryCalculatePath(point))
            {
                states.ChangeTo<MoveToDestinationEntityState>();
            }
        }

        /// <summary>
        /// Moves the Entity to a given target and performs an attack when reaching it.
        /// </summary>
        /// <param name="target">The target you want to attack.</param>
        public virtual void MoveToAttack(Transform target)
        {
            if (isAttacking || !canUpdateDestination) return;

            SetTarget(target);

            if (!IsCloseToAttackTarget())
            {
                states.ChangeTo<MoveToAttackEntityState>();
                return;
            }

            Attack();
        }

        /// <summary>
        /// Updates the current target of this Entity.
        /// </summary>
        /// <param name="target">The target you want to assign.</param>
        public virtual void SetTarget(Transform target)
        {
            if (this.target == target) return;

            this.target = target;
            onTargetSet.Invoke();
        }

        /// <summary>
        /// Smoothly rotates the Entity to the current movement direction.
        /// </summary>
        public virtual void FaceMoveDirection() => FaceTo(lateralVelocity);

        /// <summary>
        /// Smoothly rotates the Entity to the look direction (interest point).
        /// </summary>
        public virtual void FaceLookDirection() => FaceTo(lookDirection);

        /// <summary>
        /// Smoothly rotates the Entity to face a given direction.
        /// </summary>
        /// <param name="direction">The direction you want the Entity to face.</param>
        public virtual void FaceTo(Vector3 direction)
        {
            if (direction.sqrMagnitude > 0)
            {
                var delta = rotationSpeed * Time.deltaTime;
                var target = Quaternion.LookRotation(direction, Vector3.up);
                var rotation = Quaternion.RotateTowards(transform.rotation, target, delta);
                transform.rotation = rotation;
            }
        }

        /// <summary>
        /// Moves the Entity towards the ground to keep it grounded.
        /// </summary>
        public virtual void SnapToGround() => verticalVelocity = Vector3.down * groundSnapForce;

        /// <summary>
        /// Forces the Entity to enter the Idle state to make it stop any movements or attacks.
        /// </summary>
        public virtual void StandStill() => states.ChangeTo<IdleEntityState>();

        /// <summary>
        /// Makes the Entity attack without a target.
        /// </summary>
        public virtual void FreeAttack()
        {
            SetTarget(null);
            Attack();
        }

        /// <summary>
        /// Makes the Entity attack.
        /// </summary>
        public virtual void Attack()
        {
            if (isAttacking) return;

            if (useSkill)
            {
                if (skills.CanUseSkill())
                {
                    skillDuration = m_animator.GetAttackAnimationLength();
                    states.ChangeTo<UseSkillEntityState>();
                }
            }
            else
            {
                attackDuration = m_animator.GetAttackAnimationLength();
                states.ChangeTo<AttackEntityState>();
            }
        }

        /// <summary>
        /// Performs an attack, while calculating the damage and critical, considering
        /// the current skill, weapon, and stats. It also shoots a projectile if
        /// the Entity is equipping a bow.
        /// </summary>
        public virtual void PerformAttack()
        {
            var attackType = EntityAttackType.Weapon;

            if (useSkill)
            {
                skills.PerformSkill();
                attackType = EntityAttackType.Skill;
            }
            else if (items && items.IsUsingBow())
            {
                items.ShootProjectile();
            }
            else
            {
                var damage = stats.GetDamage(out var critical);

                if (!items || !items.IsUsingBlade())
                    attackType = EntityAttackType.Melee;

                hitbox.SetDamage(damage, critical);
                hitbox.Toggle();
            }

            onPerformAttack.Invoke(attackType);
        }

        /// <summary>
        /// Makes the Entity start moving along random waypoints.
        /// </summary>
        public virtual void StartRandomMovement() => states.ChangeTo<RandomMovementEntityState>();

        /// <summary>
        /// Evaluates if the Player is trying to move the Entity with directional
        /// movement and change the state to the Directional Movement Entity State.
        /// </summary>
        public virtual void EvaluateDirectionalMovement()
        {
            if (!usingInputs) return;

            var moveDirection = inputs.GetMoveDirection();

            if (moveDirection.sqrMagnitude > 0)
                states.ChangeTo<DirectionalMovementEntityState>();
        }

        /// <summary>
        /// Kills the Entity, stopping its moving and disabling its colliders.
        /// This method also notifies other entities which attacked this one that
        /// this Entity was defeated.
        /// </summary>
        public virtual void Die()
        {
            NotifyDefeat();
            SetCollidersEnabled(false);
            states.ChangeTo<IdleEntityState>();
            onDie?.Invoke();
        }

        /// <summary>
        /// Brings the Entity back to life, restoring its movements and colliders.
        /// </summary>
        public virtual void Revive()
        {
            SetCollidersEnabled(true);
            stats.Revitalize();
            states.ChangeTo<IdleEntityState>();
            onRevive.Invoke();
        }

        /// <summary>
        /// Applies damage to this Entity. This method considers the defense calculated
        /// from the current Entity Stats.
        /// </summary>
        /// <param name="other">The Entity that performed the attack.</param>
        /// <param name="amount">The amount of damage.</param>
        /// <param name="critical">Notification if the damage is critical.</param>
        public virtual void Damage(Entity other, int amount, bool critical)
        {
            if (isDead || Time.time <= m_lastHitTime + k_maxHitRate) return;

            var total = Mathf.Max(1, amount - (int)(stats.defense / 2f));
            stats.health -= total;
            m_lastHitTime = Time.time;

            if (!m_damageFrom.Contains(other))
                m_damageFrom.Add(other);

            onDamage?.Invoke(total, other.position, critical);

            if (stats.health == 0) Die();
        }

        protected virtual void NotifyDefeat()
        {
            foreach (var entity in m_damageFrom)
            {
                entity.stats.OnDefeatEntity(this);
            }
        }

        /// <summary>
        /// Disables or enables all the colliders used by the Entity.
        /// </summary>
        public virtual void SetCollidersEnabled(bool value)
        {
            foreach (var collider in m_colliders)
            {
                collider.enabled = value;
            }
        }

        protected virtual void HandleInteractive(Collider other)
        {
            if (targetInteractive && (other.CompareTag(GameTags.Interactive) ||
                other.CompareTag(GameTags.Collectible)))
            {
                if (other.TryGetComponent(out Interactive interactive) &&
                    interactive == targetInteractive)
                {
                    targetInteractive.Interact(this);
                    targetInteractive = null;
                    StandStill();
                }
            }
        }

        protected virtual void Awake()
        {
            InitializeController();
            InitializeRigidbody();
            InitializeStateMachine();
            InitializeStats();
            InitializeSkillManager();
            InitializeInputs();
            InitializeItemManager();
            InitializeInventory();
            InitializePath();
            InitializeColliders();
            InitializeAnimator();
        }

        protected virtual void Update()
        {
            if (controller.enabled)
            {
                states.HandleStep();
                controller.Move(velocity * Time.deltaTime);
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            HandleInteractive(other);
        }
    }
}
