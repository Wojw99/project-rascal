using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Level/Level Waypoints")]
    public class LevelWaypoints : Singleton<LevelWaypoints>
    {
        [Tooltip("The list of all waypoints the Player can use in this Level.")]
        public List<Waypoint> waypoints;

        [Header("Audio Settings")]
        [Tooltip("An Audio Clip that plays whenever the Player uses a Waypoint.")]
        public AudioClip travelClip;

        protected const float k_fadeInDelay = 0.2f;

        /// <summary>
        /// Returns true if the Player is traveling between waypoints.
        /// </summary>
        public bool traveling { get; protected set; }

        protected Entity m_player => Level.instance.player;

        protected GUIWindow m_waypointWindow =>
            GUIWindowsManager.instance.waypointsWindow;

        /// <summary>
        /// Teleports the current Player to a given Waypoint.
        /// </summary>
        /// <param name="waypoint">The Waypoint you want to teleport the Player to.</param>
        public virtual void TravelTo(Waypoint waypoint)
        {
            var position = waypoint.transform.position;
            var rotation = waypoint.transform.rotation;

            traveling = true;
            m_player.controller.enabled = false;
            m_player.inputs.enabled = false;
            position += Vector3.up * m_player.controller.height * 0.5f;
            m_waypointWindow.gameObject.SetActive(false);
            GameAudio.instance.PlayEffect(travelClip);

            StopAllCoroutines();

            Fader.instance.FadeOut(() =>
                StartCoroutine(TravelRoutine(position, rotation)));
        }

        protected IEnumerator TravelRoutine(Vector3 position, Quaternion rotation)
        {
            m_player.Teleport(position, rotation);

            yield return new WaitForSeconds(k_fadeInDelay);

            m_waypointWindow.gameObject.SetActive(false);

            Fader.instance.FadeIn(() =>
            {
                m_player.controller.enabled = true;
                m_player.inputs.enabled = true;
                traveling = false;
            });
        }
    }
}
