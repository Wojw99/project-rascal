using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/GUI/GUI Inventory")]
    public class GUIInventory : MonoBehaviour, IPointerDownHandler, IDropHandler
    {
        [Header("Inventory Settings")]
        [Tooltip("A reference to the Text component used to represent the coins.")]
        public Text moneyText;

        [Tooltip("The prefab of the slot used to represent cells.")]
        public GUIInventorySlot inventorySlot;

        [Header("Containers")]
        [Tooltip("The Rect Transform that will be used as a container for all the Inventory's cells.")]
        public RectTransform gridContainer;

        [Tooltip("The Rect Transform that will be used as container for all the Inventory's GUI Items.")]
        public RectTransform itemsContainer;

        [Header("Audio Settings")]
        [Tooltip("The Audio Clip that plays when a GUI Item is placed in the Inventory.")]
        public AudioClip placeClip;

        [Tooltip("The Audio Clip that plays when a GUI Item is removed from the Inventory.")]
        public AudioClip removeClip;

        protected Inventory m_inventory;
        protected GUIInventorySlot[,] m_slots;
        protected float m_initializationTime;

        /// <summary>
        /// Returns the center of the grid container.
        /// </summary>
        public Vector2 position => gridContainer.position;

        /// <summary>
        /// Returns the size of the grid container.
        /// </summary>
        public Vector2 size => gridContainer.sizeDelta;

        protected GameAudio m_audio => GameAudio.instance;

        /// <summary>
        /// Returns a reference to the equipment slots.
        /// </summary>
        public GUIEquipments equipments { get; protected set; }

        protected virtual void InitializeGrid() =>
            gridContainer.sizeDelta = m_inventory.gridSize;

        protected virtual void InitializeSlots()
        {
            m_slots = new GUIInventorySlot[m_inventory.rows, m_inventory.columns];

            for (int i = 0; i < m_inventory.rows; i++)
            {
                for (int j = 0; j < m_inventory.columns; j++)
                {
                    m_slots[i, j] = Instantiate(inventorySlot, gridContainer);
                }
            }

            UpdateSlots();
        }

        protected virtual void InitializeItems()
        {
            foreach (var item in m_inventory.items)
            {
                var row = item.Value.Item1;
                var column = item.Value.Item2;
                CreateAndPlace(item.Key, row, column);
            }
        }

        protected virtual void InitializeEquipments()
        {
            equipments = GetComponent<GUIEquipments>();
        }

        protected virtual void InitializeCallbacks()
        {
            GUI.instance.onSelectItem.AddListener((item) => TryRemove(item));
            m_inventory.onItemAdded += CreateAndPlace;
            m_inventory.onMoneyChanged += UpdateMoney;
        }

        /// <summary>
        /// Initializes the GUI Inventory grid and components.
        /// </summary>
        public virtual void InitializeInventory()
        {
            InitializeGrid();
            InitializeSlots();
            InitializeItems();
            InitializeEquipments();
            InitializeCallbacks();
            UpdateMoney();
        }

        /// <summary>
        /// Sets the Inventory this GUI Inventory handles.
        /// </summary>
        /// <param name="inventory">The instance of the Inventory.</param>
        public virtual void SetInventory(Inventory inventory) => m_inventory = inventory;

        /// <summary>
        /// Creates a new GUI Item from an Item Instance and place it on the Inventory.
        /// </summary>
        /// <param name="item">The Item Instance you want to place on the Inventory.</param>
        /// <param name="row">The row you want to place the item.</param>
        /// <param name="column">The column you want to place the item.</param>
        public virtual void CreateAndPlace(ItemInstance item, int row, int column)
        {
            Place(GUI.instance.CreateGUIItem(item, itemsContainer), row, column);
        }

        /// <summary>
        /// Places a GUI Item on the Inventory.
        /// </summary>
        /// <param name="item">The GUI Item you want to place.</param>
        /// <param name="rowId">The row you want to place the item.</param>
        /// <param name="columnId">The column you want to place the item.</param>
        public virtual void Place(GUIItem item, int rowId, int columnId)
        {
            var posX = columnId * Inventory.CellSize - size.x / 2 + item.size.x / 2;
            var posY = size.y / 2 - item.size.y / 2 - rowId * Inventory.CellSize;
            item.transform.SetParent(itemsContainer);
            ((RectTransform)item.transform).anchoredPosition = new Vector2(posX, posY);
            item.interactable = true;
            PlayAudio(placeClip);
            UpdateSlots();
        }

        /// <summary>
        /// Tries removing a GUI Item from the Inventory.
        /// </summary>
        /// <param name="item">The GUI Item you want to remove.</param>
        /// <returns>Returns true if the Inventory was able to remove the item.</returns>
        public virtual bool TryRemove(GUIItem item)
        {
            var position = m_inventory.FindPosition(item.item);

            if (item.item != null && m_inventory.TryRemoveItem(item.item))
            {
                item.SetLastPosition(this, position);
                UpdateSlots();
                PlayAudio(removeClip);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the Inventory contains a given GUI Item.
        /// </summary>
        /// <param name="item">The GUI Item you want to check.</param>
        public virtual bool Contains(GUIItem item) => m_inventory.Contains(item.item);

        /// <summary>
        /// Returns the closest cell to a given GUI Item.
        /// </summary>
        /// <param name="item">The GUI Item you want to get the closest position.</param>
        public virtual Vector2Int FindClosestCell(GUIItem item)
        {
            var mouse = gridContainer.InverseTransformPoint(Mouse.current.position.ReadValue());
            var point = (Vector2)mouse + new Vector2(-1, 1) * item.size / 2;
            var row = Mathf.RoundToInt(point.y - 10 - size.y / 2) / Inventory.CellSize * -1;
            var column = Mathf.RoundToInt(point.x + 10 + size.x / 2) / Inventory.CellSize;
            return new Vector2Int(row, column);
        }

        /// <summary>
        /// Tries to place a GUI Item in the Inventory.
        /// </summary>
        /// <param name="item">The GUI Item you want to place.</param>
        /// <returns>Returns true if the item was placed.</returns>
        public virtual bool TryPlace(GUIItem item)
        {
            var cell = FindClosestCell(item);

            if (m_inventory.TryInsertItem(item.item, cell.x, cell.y))
            {
                Place(item, cell.x, cell.y);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries insert a GUI Item in a given position on the Inventory.
        /// </summary>
        /// <param name="item">The GUI Item you want to insert.</param>
        /// <param name="position">The position you want to insert the item.</param>
        /// <returns>Returns true if the item was inserted.</returns>
        public virtual bool TryInsert(GUIItem item, (int row, int column) position)
        {
            if (m_inventory.TryInsertItem(item.item, position.row, position.column))
            {
                Place(item, position.row, position.column);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to insert a GUI Item on the first available position.
        /// </summary>
        /// <param name="item">The GUI Item you want to insert.</param>
        /// <returns>Returns true if the item was inserted.</returns>
        public virtual bool TryAutoInsert(GUIItem item)
        {
            for (int i = 0; i < m_inventory.rows; i++)
            {
                for (int j = 0; j < m_inventory.columns; j++)
                {
                    if (TryInsert(item, (i, j)))
                        return true;
                }
            }

            return false;
        }

        protected virtual void HighlightSlots()
        {
            var cell = FindClosestCell(GUI.instance.selected);
            var validArea = m_inventory.IsAreaValid(cell.x, cell.y,
                GUI.instance.selected.item.columns, GUI.instance.selected.item.rows);

            for (int i = 0; i < m_inventory.rows; i++)
            {
                for (int j = 0; j < m_inventory.columns; j++)
                {
                    m_slots[i, j].Reset();

                    if (validArea && i >= cell.x && i < cell.x + GUI.instance.selected.item.rows &&
                        j >= cell.y && j < cell.y + GUI.instance.selected.item.columns)
                    {
                        m_slots[i, j].HighlightInvalid();

                        if (m_inventory.GetItem(i, j) == null ||
                            m_inventory.GetItem(i, j).CanStack(GUI.instance.selected.item))
                        {
                            m_slots[i, j].HighlightValid();
                        }
                    }
                }
            }
        }

        protected virtual void UpdateSlots()
        {
            for (int i = 0; i < m_inventory.rows; i++)
            {
                for (int j = 0; j < m_inventory.columns; j++)
                {
                    m_slots[i, j].SetFree();

                    if (m_inventory.GetItem(i, j) != null)
                    {
                        m_slots[i, j].SetOccupied();
                    }

                    m_slots[i, j].Reset();
                }
            }
        }

        protected virtual void UpdateMoney()
        {
            if (!moneyText) return;

            moneyText.text = m_inventory.money.ToString();
        }

        protected virtual void PlayAudio(AudioClip audio)
        {
            if (Time.time > m_initializationTime)
                m_audio?.PlayUiEffect(audio);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
#if UNITY_STANDALONE || UNITY_WEBGL
            if (!GUI.instance.selected) return;

            if (TryPlace(GUI.instance.selected))
                GUI.instance.Deselect();
            else
                GameAudio.instance.PlayDeniedSound();
#endif
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (!GUI.instance.selected) return;

            if (TryPlace(GUI.instance.selected))
                GUI.instance.Deselect();
            else
            {
                GUI.instance.selected.TryMoveToLastPosition();
                GameAudio.instance.PlayDeniedSound();
            }

            UpdateSlots();
#endif
        }

        protected virtual void Awake() => m_initializationTime = Time.time;

        protected virtual void LateUpdate()
        {
            if (GUI.instance.selected)
            {
                HighlightSlots();
            }
        }

        protected virtual void OnDestroy()
        {
            if (m_inventory == null) return;

            m_inventory.onItemAdded -= CreateAndPlace;
            m_inventory.onMoneyChanged -= UpdateMoney;
        }
    }
}
