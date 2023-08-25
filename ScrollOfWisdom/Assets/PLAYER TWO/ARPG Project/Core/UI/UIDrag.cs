using UnityEngine;
using UnityEngine.EventSystems;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/UI/UI Drag")]
    public class UIDrag : MonoBehaviour, IDragHandler
    {
        public enum DragMode { Self, Parent }

        [Tooltip("The dragging mode of this UI element. You can either drag itself or its parent.")]
        public DragMode mode;

        /// <summary>
        /// Returns the target Rect Transform that will be moved.
        /// </summary>
        public RectTransform targetRect
        {
            get
            {
                switch (mode)
                {
                    default:
                    case DragMode.Self:
                        return (RectTransform)transform;
                    case DragMode.Parent:
                        return (RectTransform)transform.parent;
                }
            }
        }

        protected Canvas m_canvas;

        protected virtual void Start()
        {
            m_canvas = GetComponentInParent<Canvas>();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            targetRect.anchoredPosition += eventData.delta / m_canvas.scaleFactor;
        }
    }
}
