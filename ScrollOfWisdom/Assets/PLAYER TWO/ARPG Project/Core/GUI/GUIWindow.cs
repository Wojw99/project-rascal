using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PLAYERTWO.ARPGProject
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("PLAYER TWO/ARPG Project/GUI/GUI Window")]
    public class GUIWindow : MonoBehaviour, IPointerDownHandler
    {
        [Header("Window Settings")]
        [Tooltip("If true, the Window will automatically close when the Player starts moving.")]
        public bool closeWhenPlayerMove;

        [Header("Audio Settings")]
        [Tooltip("The Audio Clip that plays when the Window is opened.")]
        public AudioClip openClip;

        [Tooltip("The Audio Clip that plays when the Window is closed.")]
        public AudioClip closeClip;

        [Header("Window Events")]
        public UnityEvent onOpen;
        public UnityEvent onClose;

        protected RectTransform m_rect;
        protected Entity m_player;

        protected RectTransform rect
        {
            get
            {
                if (!m_rect)
                    m_rect = GetComponent<RectTransform>();

                return m_rect;
            }
        }

        /// <summary>
        /// Returns true if the Window is open.
        /// </summary>
        public bool isOpen => gameObject.activeSelf;

        protected GameAudio m_audio => GameAudio.instance;

        /// <summary>
        /// Toggles the Window visibility.
        /// </summary>
        public virtual void Toggle()
        {
            rect.SetAsLastSibling();

            if (isOpen)
                Hide();
            else
                Show();
        }

        /// <summary>
        /// Shows the Window.
        /// </summary>
        public virtual void Show()
        {
            if (isOpen) return;

            m_audio.PlayUiEffect(openClip);
            gameObject.SetActive(true);
            rect.SetAsLastSibling();
            OnOpen();
            onOpen.Invoke();
        }

        /// <summary>
        /// Hides the Window.
        /// </summary>
        public virtual void Hide()
        {
            if (!isOpen) return;

            m_audio?.PlayUiEffect(closeClip);
            gameObject.SetActive(false);
            OnClose();
            onClose.Invoke();
        }

        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }

        public void OnPointerDown(PointerEventData eventData) => rect.SetAsLastSibling();

        protected virtual void Start()
        {
            m_player = Level.instance.player;
        }

        protected virtual void LateUpdate()
        {
            if (closeWhenPlayerMove && m_player &&
                m_player.lateralVelocity.sqrMagnitude > 0)
            {
                Hide();
            }
        }
    }
}
