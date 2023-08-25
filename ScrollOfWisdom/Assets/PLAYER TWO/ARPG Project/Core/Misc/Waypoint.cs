using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(Collider))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/Misc/Waypoint")]
    public class Waypoint : MonoBehaviour
    {
        public UnityEvent onActive;

        [Header("Waypoint Settings")]
        [Tooltip("If true, this Waypoint will automatic activate when the Player gets close.")]
        public bool autoActive = true;

        [Tooltip("The minimum distance from the Waypoint to activate it.")]
        public float activationRadius = 10f;

        [Tooltip("The title of this Waypoint.")]
        public string title = "New Waypoint";

        protected bool m_active = false;
        protected Collider m_collider;

        /// <summary>
        /// Returns true if this Waypoint is activated.
        /// </summary>
        public bool active
        {
            get { return m_active; }

            set
            {
                if (!m_active && value)
                    onActive.Invoke();

                m_active = value;
            }
        }

        protected Entity m_player => Level.instance.player;

        protected virtual void Start()
        {
            m_collider = GetComponent<Collider>();
            m_collider.isTrigger = true;
        }

        protected virtual void Update()
        {
            if (active || !autoActive) return;

            var distance = Vector3.Distance(m_player.position, transform.position);

            if (distance <= activationRadius)
                active = true;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!active || !other.CompareTag(GameTags.Player)) return;
            if (LevelWaypoints.instance.traveling) return;

            m_player.StandStill();
            m_player.inputs.LockMoveDirection();
            GUIWindowsManager.instance.waypointsWindow.Show();
        }
    }
}
