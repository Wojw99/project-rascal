using UnityEngine;
using System.Collections.Generic;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(Entity))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity Area Scanner")]
    public class EntityAreaScanner : MonoBehaviour
    {
        [Tooltip("The maximum radius to scan for objects.")]
        public float scanRadius = 10;

        protected List<Transform> m_targets = new List<Transform>();
        protected List<Interactive> m_interactives = new List<Interactive>();

        protected Collider[] m_scanBuffer = new Collider[256];

        protected Entity m_entity;

        /// <summary>
        /// Returns the closest target to the Entity in the current frame.
        /// </summary>
        public virtual Transform GetClosestTarget()
        {
            return GetClosestObjectFromList<Transform>(m_targets);
        }

        /// <summary>
        /// Returns the closes interactive object from the Entity in the current frame.
        /// </summary>
        public virtual Interactive GetClosestInteractiveObject()
        {
            return GetClosestObjectFromList<Interactive>(m_interactives);
        }

        protected virtual void AddTarget(Transform target)
        {
            if (!target) return;

            m_targets.Add(target);
        }

        protected virtual void AddInteractive(Interactive interactive)
        {
            if (!interactive) return;

            m_interactives.Add(interactive);
        }

        protected virtual T GetClosestObjectFromList<T>(List<T> list) where T : Component
        {
            var totalInteractives = list.Count;

            if (totalInteractives == 0) return null;
            if (totalInteractives == 1) return list[0];

            var closestId = 0;
            var closestDistance = 0f;

            for (int i = 0; i < totalInteractives; i++)
            {
                var distance = m_entity.GetDistanceTo(list[i].transform.position);

                if (closestDistance == 0 || distance < closestDistance)
                {
                    closestId = i;
                    closestDistance = distance;
                }
            }

            return list[closestId];
        }

        protected virtual void Start()
        {
            m_entity = GetComponent<Entity>();
        }

        protected virtual void Update()
        {
            m_targets.Clear();
            m_interactives.Clear();

            var overlaps = Physics.OverlapSphereNonAlloc(transform.position,
                scanRadius, m_scanBuffer, Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Collide);

            for (int i = 0; i < overlaps; i++)
            {
                if (GameTags.IsTarget(m_scanBuffer[i].gameObject))
                    AddTarget(m_scanBuffer[i].transform);
                else if (GameTags.IsInteractive(m_scanBuffer[i].gameObject))
                    AddInteractive(m_scanBuffer[i].GetComponent<Interactive>());
            }
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, scanRadius);
        }
#endif
    }
}
