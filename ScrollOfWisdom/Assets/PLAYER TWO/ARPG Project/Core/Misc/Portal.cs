using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(Collider))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/Misc/Portal")]
    public class Portal : MonoBehaviour
    {
        [Tooltip("The name of the scene to teleport the Entity.")]
        public string scene;

        protected Collider m_collider;

        protected virtual void Start()
        {
            m_collider = GetComponent<Collider>();
            m_collider.isTrigger = true;
        }

        protected virtual void OnTriggerEnter()
        {
            GameScenes.instance.LoadScene(scene);
        }
    }
}
