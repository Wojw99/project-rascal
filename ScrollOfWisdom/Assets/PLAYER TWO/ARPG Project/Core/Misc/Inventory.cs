using System;
using System.Collections.Generic;
using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    public class Inventory
    {
        public Action<ItemInstance, int, int> onItemAdded;
        public Action<ItemInstance, int, int> onItemInserted;
        public Action onMoneyChanged;
        public Action onRemoved;

        protected ItemInstance[,] m_grid;
        protected int m_money;

        [Tooltip("The size in pixels of each cells from this Inventory.")]
        public static int CellSize = 52;

        /// <summary>
        /// Returns the amount of rows of this Inventory.
        /// </summary>
        public int rows { get; protected set; }

        /// <summary>
        /// Returns the amount of columns of this Inventory.
        /// </summary>
        /// <value></value>
        public int columns { get; protected set; }

        /// <summary>
        /// Returns the dictionary with all the Item Instances and their index.
        /// </summary>
        public Dictionary<ItemInstance, (int row, int column)> items =
            new Dictionary<ItemInstance, (int, int)>();

        /// <summary>
        /// Returns the X and Y size of the Inventory grid in pixels.
        /// </summary>
        public virtual Vector2 gridSize => new Vector2(columns, rows) * CellSize;

        /// <summary>
        /// The current amount of money on this Inventory.
        /// </summary>
        public int money
        {
            get { return m_money; }

            set
            {
                m_money = Mathf.Max(0, value);
                onMoneyChanged?.Invoke();
            }
        }

        public Inventory(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            m_grid = new ItemInstance[this.rows, this.columns];
        }

        /// <summary>
        /// Returns true if a given area of the Inventory is empty.
        /// </summary>
        /// <param name="row">The index of the row you want to check.</param>
        /// <param name="column">The index of the column you want to check.</param>
        /// <param name="width">The amount of cells to check availability from the first column.</param>
        /// <param name="height">The amount of cells to check availability from the first row.</param>
        public virtual bool IsAreaEmpty(int row, int column, int width, int height)
        {
            for (int i = row; i < row + height; i++)
            {
                for (int j = column; j < column + width; j++)
                {
                    if (m_grid[i, j] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if the a given area is valid.
        /// </summary>
        /// <param name="row">The index of the row you want to check.</param>
        /// <param name="column">The index of the column you want to check.</param>
        /// <param name="width">The amount of cells to check the existence from the first column.</param>
        /// <param name="height">The amount of cells to check the existence from the first row.</param>
        public virtual bool IsAreaValid(int row, int column, int width, int height)
        {
            return row >= 0 && column >= 0 && row + height <= rows && column + width <= columns;
        }

        /// <summary>
        /// Tries to add or stack an Item Instance on the Inventory.
        /// </summary>
        /// <param name="item">The Item Instance you want to add or stack.</param>
        /// <returns>Returns true if it successfully added or stacked the item.</returns>
        public virtual bool TryAddOrStack(ItemInstance item)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (TryInsertItem(item, i, j))
                    {
                        onItemAdded?.Invoke(item, i, j);
                        return true;
                    }
                    else if (TryStackAt(item, i, j))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to add an Item Instance on the Inventory in the first available space.
        /// </summary>
        /// <param name="item">The Item Instance you want to add.</param>
        /// <returns>Returns true if it successfully added the item.</returns>
        public virtual bool TryAddItem(ItemInstance item)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (TryInsertItem(item, i, j))
                    {
                        onItemAdded?.Invoke(item, i, j);
                        return true;
                    }
                }
            }

            item = null;
            return false;
        }

        /// <summary>
        /// Tries stack an Item Instance in a given row and column.
        /// </summary>
        /// <param name="item">The Item Instance you want to stack.</param>
        /// <param name="row">The index of the Inventory row you want to stack on.</param>
        /// <param name="column">The index of the Inventory column you want to stack on.</param>
        /// <returns>Returns true if it successfully stacked the item.</returns>
        public virtual bool TryStackAt(ItemInstance item, int row, int column)
        {
            if (m_grid[row, column] == null) return false;

            return m_grid[row, column].TryStack(item);
        }

        /// <summary>
        /// Tries to insert an Item Instance on a given row and column.
        /// </summary>
        /// <param name="item">The Item Instance you want to insert on the Inventory.</param>
        /// <param name="row">The row you want to add the Item Instance.</param>
        /// <param name="column">The column you want to add the Item Instance.</param>
        /// <returns>Returns true if the item was successfully inserted.</returns>
        public virtual bool TryInsertItem(ItemInstance item, int row, int column)
        {
            if (!IsAreaValid(row, column, item.data.columns, item.data.rows) ||
                !IsAreaEmpty(row, column, item.data.columns, item.data.rows))
            {
                return false;
            }

            items.Add(item, (row, column));

            for (int i = row; i < row + item.rows; i++)
            {
                for (int j = column; j < column + item.columns; j++)
                {
                    m_grid[i, j] = item;
                }
            }

            onItemInserted?.Invoke(item, row, column);
            return true;
        }

        /// <summary>
        /// Tries to remove an Item Instance from the Inventory
        /// </summary>
        /// <param name="item">The Item Instance you want to remove.</param>
        /// <returns>Returns true if the Item Instance was successfully removed.</returns>
        public virtual bool TryRemoveItem(ItemInstance item)
        {
            if (!items.ContainsKey(item)) return false;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (m_grid[i, j] == item)
                    {
                        m_grid[i, j] = null;
                    }
                }
            }

            items.Remove(item);
            onRemoved?.Invoke();
            return true;
        }

        /// <summary>
        /// Returns an Item Instance from the Inventory based on its row and column.
        /// </summary>
        /// <param name="row">The row you want to get the Item Instance from.</param>
        /// <param name="column">The column you want to get Item Instance from.</param>
        public virtual ItemInstance GetItem(int row, int column) => m_grid[row, column];

        /// <summary>
        /// Returns true if the Inventory contains a given Item.
        /// </summary>
        /// <param name="item">The Item you want to check.</param>
        public virtual bool Contains(ItemInstance item) => items.ContainsKey(item);

        public virtual (int row, int column) FindPosition(ItemInstance item)
        {
            if (!items.ContainsKey(item)) return (-1, -1);

            return items[item];
        }
    }
}
