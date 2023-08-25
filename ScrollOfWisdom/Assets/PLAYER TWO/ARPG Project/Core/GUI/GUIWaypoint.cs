using UnityEngine;
using UnityEngine.UI;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(Button))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/GUI/GUI Waypoint")]
    public class GUIWaypoint : MonoBehaviour
    {
        [Tooltip("A reference to the Text component used as the Waypoint title.")]
        public Text title;

        protected Button m_button;
        protected Waypoint m_waypoint;

        protected virtual void InitializeButton()
        {
            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(OnClick);
        }

        /// <summary>
        /// Sets the Waypoint this GUI Waypoint represents.
        /// </summary>
        /// <param name="waypoint">The Waypoint you want to set.</param>
        public virtual void SetWaypoint(Waypoint waypoint)
        {
            if (m_waypoint == waypoint) return;

            m_waypoint = waypoint;
            title.text = m_waypoint.title;
        }

        protected virtual void OnClick()
        {
            if (!m_waypoint) return;

            LevelWaypoints.instance.TravelTo(m_waypoint);
        }

        protected virtual void Awake() => InitializeButton();
    }
}
