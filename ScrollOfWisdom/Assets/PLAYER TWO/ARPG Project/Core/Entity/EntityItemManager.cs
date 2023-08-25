using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity Item Manager")]
    public class EntityItemManager : MonoBehaviour
    {
        public UnityEvent onChanged;
        public UnityEvent<int> onConsumeItem;

        [Header("Item Slots")]
        [Tooltip("A transform used as a weapon slot on the right hand.")]
        public Transform rightHandSlot;

        [Tooltip("A transform used as a weapon slot on the left hand.")]
        public Transform leftHandSlot;

        [Tooltip("A transform used as a shield slot.")]
        public Transform leftHandShieldSlot;

        [Tooltip("A transform used as the origin to instantiate projectiles from.")]
        public Transform projectileOrigin;

        [Header("Item Renderers")]
        [Tooltip("The skinned mesh renderer corresponding to the Character's head.")]
        public SkinnedMeshRenderer helmRenderer;

        [Tooltip("The skinned mesh renderer corresponding to the Character's chest and abdomen.")]
        public SkinnedMeshRenderer chestRenderer;

        [Tooltip("The skinned mesh renderer corresponding to the Character's hips and thighs.")]
        public SkinnedMeshRenderer pantsRenderer;

        [Tooltip("The skinned mesh renderer corresponding to the Character's hands and forearms.")]
        public SkinnedMeshRenderer glovesRenderer;

        [Tooltip("The skinned mesh renderer corresponding to the Character's feet and calfs.")]
        public SkinnedMeshRenderer bootsRenderer;

        [Header("Initial Items")]
        [Tooltip("The initial item to be equipped in the right hand.")]
        public Item rightHandItem;

        [Tooltip("The initial item to be equipped in the left hand.")]
        public Item leftHandItem;

        [Header("Durability Settings")]
        [Range(0, 1f)]
        [Tooltip("The chance of decreasing the durability points of the equipped items after receiving damage.")]
        public float onDamageDecreaseChance = 0.1f;

        [Tooltip("The amount of durability points lost after receiving a damage.")]
        public int onDamageDecreaseAmount = 1;

        protected GameObject m_rightHandObject;
        protected GameObject m_leftHandObject;

        protected Dictionary<ItemSlots, ItemInstance> m_items = new Dictionary<ItemSlots, ItemInstance>();
        protected Dictionary<ItemSlots, GameObject> m_activeInstances = new Dictionary<ItemSlots, GameObject>();
        protected Dictionary<ItemSlots, Dictionary<ItemInstance, GameObject>> m_itemInstances = new Dictionary<ItemSlots, Dictionary<ItemInstance, GameObject>>();
        protected Dictionary<ItemSlots, SkinnedMeshRenderer> m_itemRenderers = new Dictionary<ItemSlots, SkinnedMeshRenderer>();
        protected Dictionary<ItemSlots, Material[]> m_itemMaterials = new Dictionary<ItemSlots, Material[]>();
        protected Dictionary<ItemSlots, Mesh> m_defaultItemMeshes = new Dictionary<ItemSlots, Mesh>();
        protected Dictionary<ItemSlots, Material[]> m_defaultItemMaterials = new Dictionary<ItemSlots, Material[]>();
        protected List<ItemInstance> m_consumables = new List<ItemInstance>();

        protected Entity m_entity;

        /// <summary>
        /// The Entity assigned to this Item Manager.
        /// </summary>
        public Entity entity
        {
            get
            {
                if (!m_entity)
                    m_entity = GetComponent<Entity>();

                return m_entity;
            }
        }

        protected virtual void InitializeRenderers()
        {
            InitializeSlotRenderer(ItemSlots.Helm, ref helmRenderer);
            InitializeSlotRenderer(ItemSlots.Chest, ref chestRenderer);
            InitializeSlotRenderer(ItemSlots.Pants, ref pantsRenderer);
            InitializeSlotRenderer(ItemSlots.Gloves, ref glovesRenderer);
            InitializeSlotRenderer(ItemSlots.Boots, ref bootsRenderer);
        }

        protected virtual void InitializeCallbacks()
        {
            entity.onDamage.AddListener((amount, origin, critical) => ApplyDamage());
        }

        protected virtual void InitializeItems()
        {
            if (rightHandItem)
            {
                var instance = new ItemInstance(rightHandItem);
                Equip(instance, ItemSlots.RightHand);
            }

            if (leftHandItem)
            {
                var instance = new ItemInstance(leftHandItem);
                Equip(instance, ItemSlots.LeftHand);
            }
        }

        protected virtual void InitializeSlotRenderer(ItemSlots slot, ref SkinnedMeshRenderer renderer)
        {
            if (!renderer) return;

            var defaultMaterials = new List<Material>();
            m_itemRenderers.Add(slot, renderer);
            m_defaultItemMeshes.Add(slot, renderer.sharedMesh);
            renderer.GetSharedMaterials(defaultMaterials);
            m_itemMaterials.Add(slot, defaultMaterials.ToArray());
            m_defaultItemMaterials.Add(slot, defaultMaterials.ToArray());
        }

        /// <summary>
        /// Returns an array containing all the equipped items.
        /// </summary>
        public virtual ItemInstance[] GetEquippedItems()
        {
            var items = m_items.Select(item => item.Value);

            return items.Where(item => item != null).ToArray();
        }

        /// <summary>
        /// Returns true if the Entity can equip a given Item Instance in a given slot.
        /// </summary>
        /// <param name="item">The Item Instance you want to equip.</param>
        /// <param name="slot">The slot in which you want to equip the item to.</param>
        public virtual bool CanEquip(ItemInstance item, ItemSlots slot)
        {
            if (item == null || !item.IsEquippable()) return false;

            if (entity.stats.level < item.GetEquippable().requiredLevel) return false;
            if (entity.stats.strength < item.GetEquippable().requiredStrength) return false;
            if (entity.stats.dexterity < item.GetEquippable().requiredDexterity) return false;

            if (item.IsArmor() && item.GetArmor().slot != slot) return false;
            if (item.IsWeapon() && slot != ItemSlots.RightHand && slot != ItemSlots.LeftHand) return false;

            if (item.IsShield())
            {
                if (slot != ItemSlots.LeftHand) return false;

                if (IsUsingWeaponRight())
                {
                    if (!IsUsingBlade() || GetRightBlade().IsTwoHanded())
                        return false;
                }
            }

            if (item.IsBlade())
            {
                if (item.GetBlade().IsTwoHanded())
                {
                    if (slot != ItemSlots.RightHand) return false;
                    if (IsUsingWeaponLeft() || IsUsingShield()) return false;
                }
                else if (slot == ItemSlots.LeftHand)
                {
                    if (!IsUsingBlade() || GetRightBlade().IsTwoHanded())
                        return false;
                }
            }

            if (item.IsBow())
            {
                if (slot != ItemSlots.RightHand) return false;
                if (IsUsingWeaponLeft() || IsUsingShield()) return false;
            }

            return true;
        }

        /// <summary>
        /// Tries to equip a given Item Instance in a given slot.
        /// </summary>
        /// <param name="item">The Item Instance you want to equip.</param>
        /// <param name="slot">The slot in which you want to equip the Item to.</param>
        /// <returns>Returns true if the Entity was able to equip the Item.</returns>
        public virtual bool TryEquip(ItemInstance item, ItemSlots slot)
        {
            if (!CanEquip(item, slot)) return false;

            Equip(item, slot);
            return true;
        }

        /// <summary>
        /// Equips an Item Instance to a given slot.
        /// </summary>
        /// <param name="item">The Item Instance you want to equip.</param>
        /// <param name="slot">The slot in which you want to equip the Item to.</param>
        protected virtual void Equip(ItemInstance item, ItemSlots slot)
        {
            if (!m_items.ContainsKey(slot))
                m_items.Add(slot, null);

            m_items[slot] = item;
            m_items[slot].onChanged += OnItemChanged;

            if (item.IsWeapon() || item.IsShield())
                EquipAndInstantiate(item, slot);
            else if (item.IsArmor())
                EquipAndUpdateRenderer(item, slot);

            onChanged?.Invoke();
        }

        /// <summary>
        /// Equips and instantiates the item Game Object from an Item Instance.
        /// The item's parent will be the transform slot corresponding to the item slot.
        /// </summary>
        /// <param name="item">The Item Instance you want to equip and instantiate the Game Object from.</param>
        /// <param name="slot">The slot in which you want to equip the item.</param>
        protected virtual void EquipAndInstantiate(ItemInstance item, ItemSlots slot)
        {
            if (!m_activeInstances.ContainsKey(slot))
                m_activeInstances.Add(slot, null);

            m_activeInstances[slot]?.SetActive(false);

            if (!m_itemInstances.ContainsKey(slot))
                m_itemInstances.Add(slot, new Dictionary<ItemInstance, GameObject>());

            if (!m_itemInstances[slot].ContainsKey(item))
            {
                var instance = InstantiateItem(item.data, slot);
                m_itemInstances[slot].Add(item, instance);
                m_activeInstances[slot] = instance;
            }
            else
            {
                m_activeInstances[slot] = m_itemInstances[slot][item];
                m_activeInstances[slot].SetActive(true);
            }
        }

        /// <summary>
        /// Equips and updates the item Skinned Mesh Renderer from an Item Instance.
        /// The renderer will be correspondent to the Item's slot.
        /// </summary>
        /// <param name="item">The Item Instance you want to equip and update the renderer from.</param>
        /// <param name="slot">The slot in which you want to equip the item.</param>
        protected virtual void EquipAndUpdateRenderer(ItemInstance item, ItemSlots slot)
        {
            if (!m_itemRenderers.ContainsKey(slot)) return;

            m_itemRenderers[slot].sharedMesh = item.GetArmor().mesh;

            if (item.GetArmor().HasMaterials())
                m_itemRenderers[slot].sharedMaterials = item.GetArmor().materials;
        }

        /// <summary>
        /// Removes the item from a given slot, if any is equipped. It also hides the item
        /// Game Object or restores the Skinned Mesh Renderer to its unequipped state.
        /// </summary>
        /// <param name="slot">The slot you want to remove the item from.</param>
        public virtual void RemoveItem(ItemSlots slot)
        {
            if (!m_items.TryGetValue(slot, out var item)) return;

            if (item.IsWeapon() || item.IsShield())
            {
                m_itemInstances[slot][m_items[slot]]?.SetActive(false);
            }
            else if (item.IsArmor())
            {
                m_itemRenderers[slot].sharedMesh = m_defaultItemMeshes[slot];

                if (item.GetArmor().HasMaterials())
                    m_itemRenderers[slot].sharedMaterials = m_defaultItemMaterials[slot];
            }

            m_items[slot].onChanged -= OnItemChanged;
            m_items[slot] = null;
            onChanged?.Invoke();
        }

        /// <summary>
        /// Returns the Item Instance equipped in a given slot or initializes the slot.
        /// </summary>
        /// <param name="slot">The slot you want to get the Item Instance from or initialize.</param>
        public virtual ItemInstance GetOrInitializeItem(ItemSlots slot)
        {
            if (!m_items.ContainsKey(slot))
                m_items.Add(slot, null);

            return m_items[slot];
        }

        /// <summary>
        /// Sets the list of available consumable items.
        /// </summary>
        /// <param name="items">The array of Item Instance to assign to the consumable list.</param>
        public virtual void SetConsumables(ItemInstance[] items)
        {
            if (items == null) return;

            m_consumables = new List<ItemInstance>(items);
        }

        /// <summary>
        /// Sets a consumable item in the consumables list based on its index.
        /// </summary>
        /// <param name="index">The index of the slot in the consumables list.</param>
        /// <param name="item">The Item Instance you want to assign to the consumables slots.</param>
        public virtual void SetConsumable(int index, ItemInstance item) => m_consumables[index] = item;

        /// <summary>
        /// Returns an array with all the consumables items.
        /// </summary>
        public virtual ItemInstance[] GetConsumables() => m_consumables.ToArray();

        public virtual void ConsumeItem(ItemInstance item)
        {
            var index = m_consumables.FindIndex(c => c == item);
            ConsumeItem(index);
        }

        /// <summary>
        /// Consumes an item from the consumable items list and applies its effects.
        /// </summary>
        /// <param name="index">The index from the consumables list.</param>
        public virtual void ConsumeItem(int index)
        {
            if (index < 0 || index >= m_consumables.Count || m_consumables[index] == null)
                return;

            if (m_consumables[index].stack > 0)
            {
                m_consumables[index].stack--;

                if (m_consumables[index].GetPotion().healthAmount > 0)
                    entity.stats.health += m_consumables[index].GetPotion().healthAmount;

                if (m_consumables[index].GetPotion().manaAmount > 0)
                    entity.stats.mana += m_consumables[index].GetPotion().manaAmount;

                if (m_consumables[index].stack == 0)
                    m_consumables[index] = null;
            }

            onConsumeItem?.Invoke(index);
        }

        /// <summary>
        /// Applies damage to all the items if the random chance was met.
        /// </summary>
        public virtual void ApplyDamage()
        {
            foreach (var item in m_items)
            {
                if (item.Value == null) continue;

                if (Random.Range(0, 1f) < onDamageDecreaseChance)
                    item.Value.ApplyDamage(onDamageDecreaseAmount);
            }
        }

        /// <summary>
        /// Instantiates the item's Game Object on a given slot.
        /// </summary>
        /// <param name="item">The Item you want to instantiate the Game Object from.</param>
        /// <param name="slot">The slot in which you want to instantiate the item to.</param>
        /// <returns>The instance of the newly instantiated Game Object.</returns>
        protected virtual GameObject InstantiateItem(Item item, ItemSlots slot)
        {
            GameObject instance = null;

            if (item is ItemBlade)
            {
                if (slot == ItemSlots.RightHand)
                    instance = ((ItemBlade)item).InstantiateRightHand(rightHandSlot);
                else if (slot == ItemSlots.LeftHand)
                    instance = ((ItemBlade)item).InstantiateLeftHand(leftHandSlot);
            }
            else if (item is ItemBow)
                instance = ((ItemBow)item).Instantiate(rightHandSlot, leftHandSlot);
            else if (item is ItemShield)
                instance = item.Instantiate(leftHandShieldSlot);

            return instance;
        }

        /// <summary>
        /// Returns the accumulated defense points from all the equipped items.
        /// </summary>
        public virtual int GetDefense()
        {
            var total = 0;

            foreach (var item in m_items)
            {
                if (item.Value == null) continue;

                total += item.Value.GetDefense();
            }

            return total;
        }

        /// <summary>
        /// Returns the accumulated minimum and maximum damage from all the equipped items.
        /// </summary>
        public virtual (int, int) GetDamage()
        {
            var total = (0, 0);

            foreach (var item in m_items)
            {
                if (item.Value == null) continue;

                var damage = item.Value.GetDamage();
                total.Item1 += damage.Item1;
                total.Item2 += damage.Item2;
            }

            return total;
        }

        /// <summary>
        /// Returns the attack speed of the equipped weapons. If the Entity is equipping
        /// two weapons, the attack speed will increase by half of the left weapon's attack speed.
        /// </summary>
        public virtual int GetAttackSpeed()
        {
            if (!IsUsingWeaponRight()) return 0;

            var total = GetRightWeapon().attackSpeed;

            if (IsUsingWeaponLeft())
                total += (int)(GetLeftWeapon().attackSpeed * 0.5f);

            return total;
        }

        /// <summary>
        /// Returns the accumulated points from all additional attributes (the 'blue' attributes).
        /// </summary>
        public virtual ItemFinalAttributes GetFinalAttributes()
        {
            int damage = 0, attackSpeed = 0, defense = 0, health = 0, mana = 0;

            float damageMultiplier = 1f, criticalChanceMultiplier = 1f,
                defenseMultiplier = 1f, manaMultiplier = 1f, healthMultiplier = 1f;

            foreach (var item in m_items)
            {
                if (item.Value == null) continue;

                damage += item.Value.GetAdditionalDamage();
                attackSpeed += item.Value.GetAttackSpeed();
                defense += item.Value.GetAdditionalDefense();
                mana += item.Value.GetAdditionalMana();
                health += item.Value.GetAdditionalHealth();
                damageMultiplier += item.Value.GetDamageMultiplier();
                criticalChanceMultiplier += item.Value.GetCriticalChanceMultiplier();
                defenseMultiplier += item.Value.GetDefenseMultiplier();
                manaMultiplier += item.Value.GetManaMultiplier();
                healthMultiplier += item.Value.GetHealthMultiplier();
            }

            return new ItemFinalAttributes(
                damage,
                attackSpeed,
                defense,
                mana,
                health,
                damageMultiplier,
                criticalChanceMultiplier,
                defenseMultiplier,
                manaMultiplier,
                healthMultiplier
            );
        }

        /// <summary>
        /// Instantiates, calculates the damage, and configure the projectile form the current weapon.
        /// </summary>
        public virtual Projectile ShootProjectile()
        {
            if (!IsUsingBow()) return null;

            var damage = entity.stats.GetDamage(out var critical);
            var projectile = Instantiate(GetBow().projectile,
                projectileOrigin.position, projectileOrigin.rotation);

            projectile.SetDamage(entity, damage, critical, entity.targetTag);
            return projectile;
        }

        protected virtual void OnItemChanged() => onChanged.Invoke();

        /// <summary>
        /// Returns true if the Entity is equipping a weapon in any of its hands.
        /// </summary>
        public virtual bool IsUsingWeapon() => IsUsingWeaponLeft() || IsUsingWeaponRight();

        /// <summary>
        /// Returns true if the Entity is equipping a weapon in the right hand.
        /// </summary>
        public virtual bool IsUsingWeaponRight() => GetOrInitializeItem(ItemSlots.RightHand)?.data is ItemWeapon;

        /// <summary>
        /// Returns true if the Entity is equipping a weapon in the left hand.
        /// </summary>
        public virtual bool IsUsingWeaponLeft() => GetOrInitializeItem(ItemSlots.LeftHand)?.data is ItemWeapon;

        /// <summary>
        /// Returns true if the equipped weapon in the right hand is a blade.
        /// </summary>
        public virtual bool IsUsingBlade() => GetOrInitializeItem(ItemSlots.RightHand)?.data is ItemBlade;

        /// <summary>
        /// Returns true if the equipped weapon in the left hand is a blade.
        /// </summary>
        public virtual bool IsUsingBladeLeft() => GetOrInitializeItem(ItemSlots.LeftHand)?.data is ItemBlade;

        /// <summary>
        /// Returns true if the Entity is equipping a shield.
        /// </summary>
        public virtual bool IsUsingShield() => GetOrInitializeItem(ItemSlots.LeftHand)?.data is ItemShield;

        /// <summary>
        /// Returns true if the Entity is equipping a bow or crossbow.
        /// </summary>
        public virtual bool IsUsingBow() => GetOrInitializeItem(ItemSlots.RightHand)?.data is ItemBow;

        /// <summary>
        /// Returns the Item Instance of the item equipped on the right hand slot.
        /// </summary>
        public virtual ItemInstance GetRightHand() => GetOrInitializeItem(ItemSlots.RightHand);

        /// <summary>
        /// Returns the Item Instance of the item equipped on the left hand slot.
        /// </summary>
        public virtual ItemInstance GetLeftHand() => GetOrInitializeItem(ItemSlots.LeftHand);

        /// <summary>
        /// Returns the Item Instance of the item equipped on the helm slot.
        /// </summary>
        public virtual ItemInstance GetHelm() => GetOrInitializeItem(ItemSlots.Helm);

        /// <summary>
        /// Returns the Item Instance of the item equipped on the chest slot.
        /// </summary>
        public virtual ItemInstance GetChest() => GetOrInitializeItem(ItemSlots.Chest);

        /// <summary>
        /// Returns the Item Instance of the item equipped on the pants slot.
        /// </summary>
        public virtual ItemInstance GetPants() => GetOrInitializeItem(ItemSlots.Pants);

        /// <summary>
        /// Returns the Item Instance of the item equipped on the gloves slot.
        /// </summary>
        public virtual ItemInstance GetGloves() => GetOrInitializeItem(ItemSlots.Gloves);

        /// <summary>
        /// Returns the Item Instance of the item equipped on the boots slot.
        /// </summary>
        public virtual ItemInstance GetBoots() => GetOrInitializeItem(ItemSlots.Boots);

        /// <summary>
        /// Returns the Item Weapon of the item equipped on the right hand slot.
        /// </summary>
        public virtual ItemWeapon GetRightWeapon() => GetOrInitializeItem(ItemSlots.RightHand).GetData<ItemWeapon>();

        /// <summary>
        /// Returns the Item Weapon of the item equipped on the left hand slot.
        /// </summary>
        public virtual ItemWeapon GetLeftWeapon() => GetOrInitializeItem(ItemSlots.LeftHand).GetData<ItemWeapon>();

        /// <summary>
        /// Returns the Item Weapon of the item equipped on the right or left hand slots.
        /// </summary>
        public virtual ItemWeapon GetWeapon() => IsUsingWeaponRight() ? GetRightWeapon() : GetLeftWeapon();

        /// <summary>
        /// Returns the Item Blade of the item equipped on the right hand slot.
        /// </summary>
        public virtual ItemBlade GetRightBlade() => GetOrInitializeItem(ItemSlots.RightHand).GetData<ItemBlade>();

        /// <summary>
        /// Returns the Item Blade of the item equipped on the left hand slot.
        /// </summary>
        public virtual ItemBlade GetLeftBlade() => GetOrInitializeItem(ItemSlots.LeftHand).GetData<ItemBlade>();

        /// <summary>
        /// Returns the Item Shield of the item equipped on the shield slot.
        /// </summary>
        public virtual ItemShield GetShield() => GetOrInitializeItem(ItemSlots.LeftHand).GetData<ItemShield>();

        /// <summary>
        /// Returns the Item Bow of the item equipped on the right hand slot.
        /// </summary>
        public virtual ItemBow GetBow() => GetOrInitializeItem(ItemSlots.RightHand).GetData<ItemBow>();

        protected virtual void Awake()
        {
            InitializeRenderers();
            InitializeCallbacks();
            InitializeItems();
        }
    }
}
