using UnityEngine;
using System.Collections;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(Entity))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity AI")]
    public class EntityAI : MonoBehaviour
    {
        [Header("Detection Settings")]
        [Tooltip("The tag of the Game Object that this Entity will identify as its target.")]
        public string targetTag = "Entity/Player";

        [Tooltip("The minimum radius to spot a new target.")]
        public float spotRadius = 5f;

        [Tooltip("The minimum distance to space from the Entity view sight after being detected.")]
        public float fleeRadius = 10f;

        [Header("Attack Settings")]
        [Tooltip("If true, the Entity will always attack with the current Skill.")]
        public bool useSkill;

        [Tooltip("A delay in seconds before the Entity starts patrolling.")]
        public float resetMoveDelay = 1f;

        [Tooltip("A delay in seconds between attacks.")]
        public float attackCoolDown = 0.5f;

        [Header("Search Settings")]
        [Tooltip("If true, this Entity will search the origin of the last damage it take.")]
        public bool searchDamageSource = true;

        [Tooltip("A delay in seconds before starting to look to the last damage origin.")]
        public float searchDamageSourceDelay = 0.3f;

        [Tooltip("The duration in seconds in which this Entity will search for the damage origin.")]
        public float searchDamageSourceDuration = 5f;

        protected Entity m_entity;
        protected Camera m_camera;

        protected int m_totalTargetsInSight;
        protected float m_lastAttackTime;
        protected float m_waitingToSearchTime;
        protected float m_nextTargetRefreshTime;
        protected bool m_waitingToSearch;

        protected Plane[] m_frustumPlanes = new Plane[6];
        protected Collider[] m_targetsInSight = new Collider[128];

        protected const float k_targetRefreshRate = 0.2f;

        protected virtual void InitializeCamera() => m_camera = Camera.main;

        protected virtual void InitializeEntity()
        {
            m_entity = GetComponent<Entity>();
            m_entity.states.ChangeTo<RandomMovementEntityState>();
            m_entity.useSkill = useSkill;
        }

        protected virtual void InitializeCallback()
        {
            m_entity.onDamage.AddListener(OnDamage);
            m_entity.onDie.AddListener(OnDie);
        }

        protected virtual void HandleEntityOptimization()
        {
            GeometryUtility.CalculateFrustumPlanes(m_camera, m_frustumPlanes);
            m_entity.enabled = GeometryUtility.TestPlanesAABB(m_frustumPlanes, m_entity.controller.bounds);
        }

        protected virtual void HandleViewSight()
        {
            if (m_entity.target || Time.time < m_nextTargetRefreshTime) return;

            m_nextTargetRefreshTime = Time.time + k_targetRefreshRate;
            m_totalTargetsInSight = Physics.OverlapSphereNonAlloc
                (transform.position, spotRadius, m_targetsInSight);

            for (int i = 0; i < m_totalTargetsInSight; i++)
            {
                if (m_targetsInSight[i].CompareTag(targetTag))
                {
                    SpotTarget(m_targetsInSight[i].GetComponent<Entity>());
                }
            }
        }

        /// <summary>
        /// Assign a target to the Entity.
        /// </summary>
        /// <param name="target">The target you want to assign.</param>
        public virtual void SpotTarget(Entity target)
        {
            StopAllCoroutines();
            m_entity.SetTarget(target.transform);
        }

        protected virtual void HandleTargetFlee()
        {
            if (!m_entity.target) return;

            if (m_entity.GetDistanceToTarget() > fleeRadius) StopAttack();
        }

        protected virtual void HandleAttack()
        {
            if (!m_entity.target || m_entity.isAttacking) return;

            if (m_entity.IsCloseToAttackTarget())
            {
                if (Time.time - m_lastAttackTime > attackCoolDown)
                {
                    m_lastAttackTime = Time.time + m_entity.skillDuration;
                    m_entity.Attack();

                    if (!m_entity.target ||
                        m_entity.target.GetComponent<Entity>().isDead)
                        StopAttack();
                }
                else
                {
                    m_entity.StandStill();
                }
            }
            else
            {
                m_entity.MoveToTarget();
            }
        }

        protected virtual void OnDamage(int amount, Vector3 source, bool critical)
        {
            if (!m_entity.target && searchDamageSource)
            {
                StopAllCoroutines();
                StartCoroutine(SearchDamageSourceRoutine(source));
            }
        }

        protected virtual void OnDie() => StopAllCoroutines();

        /// <summary>
        /// Makes the Entity stop attacking its assigned target.
        /// </summary>
        public virtual void StopAttack()
        {
            StopAllCoroutines();
            StartCoroutine(StopAttackRoutine());
        }

        protected virtual IEnumerator StopAttackRoutine()
        {
            m_entity.StandStill();
            m_entity.SetTarget(null);
            yield return new WaitForSeconds(resetMoveDelay);
            m_entity.StartRandomMovement();
        }

        protected virtual IEnumerator SearchDamageSourceRoutine(Vector3 source)
        {
            m_entity.StandStill();

            if (!m_waitingToSearch)
            {
                m_waitingToSearch = true;
                m_waitingToSearchTime = Time.time;
            }

            while (Time.time - m_waitingToSearchTime < searchDamageSourceDelay)
            {
                yield return null;
            }

            m_entity.MoveTo(source);
            yield return new WaitForSeconds(searchDamageSourceDuration);
            m_entity.StartRandomMovement();
            m_waitingToSearch = false;
        }

        protected virtual bool CanUpdateAI() => m_entity.enabled && m_entity.isActive;

        protected virtual void Start()
        {
            InitializeCamera();
            InitializeEntity();
            InitializeCallback();
        }

        protected virtual void Update()
        {
            HandleEntityOptimization();

            if (CanUpdateAI())
            {
                HandleViewSight();
                HandleTargetFlee();
                HandleAttack();
            }
        }
    }
}
