using UnityEngine;
using System.Collections.Generic;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/GUI/GUI Waypoints Window")]
    public class GUIWaypointsWindow : MonoBehaviour
    {
        [Tooltip("A reference to the transform used as the waypoints container.")]
        public RectTransform waypointsContainer;

        [Tooltip("The prefab you want to use as Waypoints.")]
        public GUIWaypoint waypointPrefab;

        protected List<GUIWaypoint> m_buttons = new List<GUIWaypoint>();

        protected LevelWaypoints m_levelWaypoints => LevelWaypoints.instance;

        protected virtual void Awake()
        {
            foreach (var waypoint in m_levelWaypoints.waypoints)
            {
                var button = Instantiate(waypointPrefab, waypointsContainer);
                button.SetWaypoint(waypoint);
                m_buttons.Add(button);
            }
        }

        protected virtual void OnEnable()
        {
            for (int i = 0; i < m_levelWaypoints.waypoints.Count; i++)
            {
                var state = m_levelWaypoints.waypoints[i].active;
                m_buttons[i].gameObject.SetActive(state);
            }
        }
    }
}
