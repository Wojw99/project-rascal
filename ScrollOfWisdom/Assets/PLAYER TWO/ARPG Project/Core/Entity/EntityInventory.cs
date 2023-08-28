using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity Inventory")]
    public class EntityInventory : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("The amount of rows available on the Inventory.")]
        public int rows = 6;

        [Tooltip("The amount of columns available on the Inventory.")]
        public int columns = 10;

        [Header("Inventory Events")]
        public UnityEvent<ItemInstance> onItemAdded;
        public UnityEvent<ItemInstance> onItemInserted;
        public UnityEvent onItemRemoved;

        protected Inventory m_inventory;

        /// <summary>
        /// Returns the instance of the Inventory.
        /// </summary>
        public Inventory instance
        {
            get
            {
                if (m_inventory == null)
                {
                    m_inventory = new Inventory(rows, columns);
                    m_inventory.onItemAdded += (item, _, _) => onItemAdded.Invoke(item);
                    m_inventory.onItemInserted += (item, _, _) => onItemInserted.Invoke(item);
                    m_inventory.onRemoved += () => onItemRemoved.Invoke();
                }

                return m_inventory;
            }
        }
    }
}
