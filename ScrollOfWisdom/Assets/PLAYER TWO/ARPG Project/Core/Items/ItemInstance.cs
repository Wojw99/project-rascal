using UnityEngine;

namespace PLAYERTWO.ARPGProject
{
    [System.Serializable]
    public class ItemInstance
    {
        /// <summary>
        /// Invoked when the durability changed.
        /// </summary>
        public System.Action onChanged;

        /// <summary>
        /// Invoked when the stack size changed.
        /// </summary>
        public System.Action onStackChanged;

        /// <summary>
        /// Invoked when the durability points reach zero.
        /// </summary>
        public System.Action onBreak;

        /// <summary>
        /// The Item data that represents this Item Instance.
        /// </summary>
        public Item data;

        /// <summary>
        /// The additional attributes of this Item Instance.
        /// </summary>
        public ItemAttributes attributes;

        protected int m_stack;

        /// <summary>
        /// The current durability points of this Item Instance.
        /// </summary>
        public int durability { get; protected set; }

        /// <summary>
        /// The size of the item stack.
        /// </summary>
        public int stack
        {
            get { return m_stack; }

            set
            {
                if (!IsStackable()) return;

                m_stack = Mathf.Clamp(value, 0, data.stackCapacity);
                onStackChanged?.Invoke();
            }
        }

        /// <summary>
        /// The amount of rows this Item Instance takes on the Inventory.
        /// </summary>
        public int rows => data.rows;

        /// <summary>
        /// The amount of columns this Item Instance takes on the Inventory.
        /// </summary>
        public int columns => data.columns;

        public ItemInstance(Item data, bool generateAttributes = true,
            int minAttributes = 0, int maxAttributes = 0)
        {
            SetDefaultData(data);

            if (generateAttributes)
                GenerateAdditionalAttributes(minAttributes, maxAttributes);
        }

        public ItemInstance(Item data, ItemAttributes attributes)
        {
            this.data = data;
            this.attributes = attributes;

            if (IsEquippable())
                durability = GetEquippable().maxDurability;

            if (IsStackable())
                stack = 1;
        }

        public ItemInstance(Item data, int durability, int stack, bool generateAttributes = true)
        {
            this.data = data;
            this.durability = durability;
            this.stack = stack;

            if (generateAttributes)
                GenerateAdditionalAttributes();
        }

        public ItemInstance(Item data, ItemAttributes attributes, int durability, int stack)
        {
            this.data = data;
            this.attributes = attributes;
            this.durability = durability;
            this.stack = stack;
        }

        /// <summary>
        /// Tries to stack another item on the stack.
        /// </summary>
        /// <param name="other">The Item Instance you want to try stack.</param>
        /// <returns>Returns true if it was able to stack the item.</returns>
        public virtual bool TryStack(ItemInstance other)
        {
            if (!CanStack(other)) return false;

            stack += other.stack;
            return true;
        }

        /// <summary>
        /// Returns the required minimum level to equip this Item.
        /// </summary>
        public virtual int GetRequiredLevel()
        {
            if (IsEquippable()) return GetEquippable().requiredLevel;
            if (IsSkill()) return GetSkill().requiredLevel;

            return 0;
        }

        /// <summary>
        /// Returns the required minimum strength to equip this Item.
        /// </summary>
        public virtual int GetRequiredStrength()
        {
            if (IsEquippable()) return GetEquippable().requiredStrength;
            if (IsSkill()) return GetSkill().requiredStrength;

            return 0;
        }

        /// <summary>
        /// Returns the required minimum dexterity to equip this Item.
        /// </summary>
        public virtual int GetRequiredDexterity()
        {
            if (IsEquippable()) return GetEquippable().requiredDexterity;

            return 0;
        }

        /// <summary>
        /// Returns the required minimum energy to equip this Item.
        /// </summary>
        public virtual int GetRequiredEnergy()
        {
            if (IsSkill()) return GetSkill().requiredEnergy;

            return 0;
        }

        /// <summary>
        /// Returns true if this Item Instance can stack another given Item Instance.
        /// </summary>
        /// <param name="other">The Item Instance you want to check.</param>
        public virtual bool CanStack(ItemInstance other) =>
            IsStackable() && other.data == data && stack + other.stack <= data.stackCapacity;

        /// <summary>
        /// Returns true if this Item allows stacking.
        /// </summary>
        public virtual bool IsStackable() => data.canStack;

        /// <summary>
        /// Returns true if this Item is equippable.
        /// </summary>
        public virtual bool IsEquippable() => data is ItemEquippable;

        /// <summary>
        /// Returns true if this Item represents a Skill.
        /// </summary>
        public virtual bool IsSkill() => data is ItemSkill;

        /// <summary>
        /// Returns true if this Item is an Armor.
        /// </summary>
        public virtual bool IsArmor() => data is ItemArmor;

        /// <summary>
        /// Returns true if this Item is Consumable.
        /// </summary>
        public virtual bool IsConsumable() => data is ItemConsumable;

        /// <summary>
        /// Returns true if this Item is a Potion.
        /// </summary>
        public virtual bool IsPotion() => data is ItemPotion;

        /// <summary>
        /// Returns true if this Item is a Weapon.
        /// </summary>
        public virtual bool IsWeapon() => data is ItemWeapon;

        /// <summary>
        /// Returns true if this Item is a Shield.
        /// </summary>
        public virtual bool IsShield() => data is ItemShield;

        /// <summary>
        /// Returns true if this Item is a Blade.
        /// </summary>
        public virtual bool IsBlade() => data is ItemBlade;

        /// <summary>
        /// Returns true if this Item is a Bow.
        /// </summary>
        public virtual bool IsBow() => data is ItemBow;

        /// <summary>
        /// Returns true if the durability points of this Item Instance is zero.
        /// </summary>
        public virtual bool IsBroken() => durability == 0;

        /// <summary>
        /// Returns true if the durability of this Item Instance is at half.
        /// </summary>
        public virtual bool IsAboutToBreak()
        {
            if (!IsEquippable()) return false;

            return durability <= GetEquippable().maxDurability / 2f;
        }

        /// <summary>
        /// Returns true if this Item Instance has additional attributes.
        /// </summary>
        public virtual bool ContainAttributes() => IsEquippable() && attributes != null;

        /// <summary>
        /// Returns true if it's allowed to read the additional attributes from this Item Instance.
        /// </summary>
        public virtual bool UseAttributes() => ContainAttributes() && !IsBroken();

        /// <summary>
        /// Returns the data of this Item Instance as an Item Equippable.
        /// </summary>
        public virtual ItemEquippable GetEquippable() => GetData<ItemEquippable>();

        /// <summary>
        /// Returns the data of this Item Instance as an Item Skill.
        /// </summary>
        public virtual ItemSkill GetSkill() => GetData<ItemSkill>();

        /// <summary>
        /// Returns the data of this Item Instance as an Item Armor.
        /// </summary>
        public virtual ItemArmor GetArmor() => GetData<ItemArmor>();

        /// <summary>
        /// Returns the data of this Item Instance as an Item Potion.
        /// </summary>
        public virtual ItemPotion GetPotion() => GetData<ItemPotion>();

        /// <summary>
        /// Returns the data of this Item Instance as an Item Weapon.
        /// </summary>
        public virtual ItemWeapon GetWeapon() => GetData<ItemWeapon>();

        /// <summary>
        /// Returns the data of this Item Instance as an Item Shield.
        /// </summary>
        public virtual ItemShield GetShield() => GetData<ItemShield>();

        /// <summary>
        /// Returns the data of this Item Instance as an Item Blade.
        /// </summary>
        public virtual ItemBlade GetBlade() => GetData<ItemBlade>();

        /// <summary>
        /// Returns the data of this Item Instance as an Item Bow.
        /// </summary>
        public virtual ItemBow GetBow() => GetData<ItemBow>();

        /// <summary>
        /// Returns the data of this Item Instance casted to a given type.
        /// </summary>
        public virtual T GetData<T>() where T : Item => data as T;

        /// <summary>
        /// Reduces the durability of this Item Instance by a given amount.
        /// </summary>
        /// <param name="amount">The amount of points to decrease from the durability.</param>
        public virtual void ApplyDamage(int amount)
        {
            if (!IsEquippable()) return;

            var maxDurability = GetEquippable().maxDurability;
            durability = Mathf.Clamp(durability - amount, 0, maxDurability);

            if (durability <= 0)
                onBreak?.Invoke();

            onChanged?.Invoke();
        }

        /// <summary>
        /// Returns the minimum and maximum damage of this Item Instance. If the Item is broken or if its
        /// not a Weapon, the damage will always be zero. If it's about to break, the damage is reduced by half.
        /// </summary>
        public virtual (int minimum, int maximum) GetDamage()
        {
            if (!IsWeapon() || IsBroken()) return (0, 0);

            var minDamage = GetWeapon().minDamage;
            var maxDamage = GetWeapon().maxDamage;

            if (IsAboutToBreak())
                return ((int)(minDamage / 2f), (int)(maxDamage / 2f));

            return (minDamage, maxDamage);
        }

        /// <summary>
        /// Returns the defense points of this Item Instance. If it's broken, the defense is zero.
        /// If the Item Instance is about to break, the defense is reduced by half.
        /// </summary>
        public virtual int GetDefense()
        {
            if (IsBroken()) return 0;

            var defense = 0;

            if (IsArmor())
                defense = GetArmor().defense;
            else if (IsShield())
                defense = GetShield().defense;

            return IsAboutToBreak() ? (int)(defense / 2f) : defense;
        }

        /// <summary>
        /// Returns the amount of additional attributes.
        /// </summary>
        public virtual int GetAttributesCount() => ContainAttributes() ? attributes.GetAttributesCount() : 0;

        /// <summary>
        /// Returns the additional damage points.
        /// </summary>
        public virtual int GetAdditionalDamage() => UseAttributes() ? attributes.damage : 0;

        /// <summary>
        /// Returns the additional attack speed points.
        /// </summary>
        public virtual int GetAttackSpeed() => UseAttributes() ? attributes.attackSpeed : 0;

        /// <summary>
        /// Returns the additional defense points.
        /// </summary>
        public virtual int GetAdditionalDefense() => UseAttributes() ? attributes.defense : 0;

        /// <summary>
        /// Returns the additional mana points.
        /// </summary>
        public virtual int GetAdditionalMana() => UseAttributes() ? attributes.mana : 0;

        /// <summary>
        /// Returns the additional health points.
        /// </summary>
        public virtual int GetAdditionalHealth() => UseAttributes() ? attributes.health : 0;

        /// <summary>
        /// Returns the additional damage multiplier.
        /// </summary>
        public virtual float GetDamageMultiplier() => UseAttributes() ? attributes.GetDamageMultiplier() : 0;

        /// <summary>
        /// Returns the additional critical chance multiplier.
        /// </summary>
        public virtual float GetCriticalChanceMultiplier() => UseAttributes() ? attributes.GetCriticalMultiplier() : 0;

        /// <summary>
        /// Returns the additional defense multiplier.
        /// </summary>
        public virtual float GetDefenseMultiplier() => UseAttributes() ? attributes.GetDefenseMultiplier() : 0;

        /// <summary>
        /// Returns the additional mana multiplier.
        /// </summary>
        public virtual float GetManaMultiplier() => UseAttributes() ? attributes.GetManaMultiplier() : 0;

        /// <summary>
        /// Returns the additional health multiplier.
        /// </summary>
        public virtual float GetHealthMultiplier() => UseAttributes() ? attributes.GetHealthMultiplier() : 0;

        /// <summary>
        /// Sets this Item Instance durability to its maximum points.
        /// </summary>
        public virtual void Repair()
        {
            if (!IsEquippable()) return;

            durability = GetEquippable().maxDurability;
            onChanged?.Invoke();
        }

        /// <summary>
        /// Returns the current durability in a rate of zero to one.
        /// </summary>
        public virtual float GetDurabilityRate()
        {
            if (!IsEquippable()) return 1;

            return durability / (float)GetEquippable().maxDurability;
        }

        protected virtual void SetDefaultData(Item data)
        {
            this.data = data;

            if (IsEquippable())
                durability = GetEquippable().maxDurability;

            if (IsStackable())
                stack = 1;
        }

        protected virtual void GenerateAdditionalAttributes(int minAttributes = 0, int maxAttributes = 0)
        {
            if (!IsEquippable()) return;

            if (IsWeapon())
                attributes = new WeaponAttributes(minAttributes, maxAttributes);
            else if (IsArmor() || IsShield())
                attributes = new ArmorAttributes(minAttributes, maxAttributes);
        }

        /// <summary>
        /// Returns the selling price of this Item Instance.
        /// </summary>
        public virtual int GetSellPrice() => (int)(GetPrice() / 2f);

        /// <summary>
        /// Returns the price of this Item Instance. If it's a stack, the price is multiplier
        /// by the stack size. The durability rate of the Item Instance is multiplied by its final price.
        /// </summary>
        public virtual int GetPrice()
        {
            var price = data.price;

            if (IsStackable()) price *= stack;

            if (IsEquippable())
            {
                if (attributes != null)
                {
                    var totalAttr = attributes.GetAttributesCount();
                    price += totalAttr * Game.instance.pricePerAttribute;
                }

                price = (int)(price * GetDurabilityRate());
            }

            return price;
        }

        protected string InspectRequired(string name, int required, int current, Color error, bool breakLine)
        {
            var lineBreak = breakLine ? "\n" : "";
            var attr = $"Required {name}: {required}";

            if (current < required)
                return lineBreak + StringUtils.StringWithColor(attr, error);

            return lineBreak + attr;
        }

        /// <summary>
        /// Returns a string with the Item's general attributes.
        /// </summary>
        /// <param name="stats">The Entity Stats to compare against.</param>
        /// <param name="warning">The color of warning texts.</param>
        /// <param name="error">The color of the error texts.</param>
        public virtual string Inspect(EntityStatsManager stats, Color warning, Color error)
        {
            var text = "";

            if (IsArmor())
                text += $"Defense: {GetArmor().defense}";
            else if (IsShield())
                text += $"Defense: {GetShield().defense}";
            else if (IsWeapon())
            {
                text += $"Damage: {GetWeapon().minDamage} ~ {GetWeapon().maxDamage}";
                text += $"\nAttack Speed: {GetWeapon().attackSpeed}";
            }

            if (IsEquippable())
            {
                var lineBreak = text.Length > 0 ? "\n" : "";
                var attr = $"Durability: {durability} of {GetEquippable().maxDurability}";

                if (IsAboutToBreak())
                    text += lineBreak + StringUtils.StringWithColor(attr, warning);
                else if (IsBroken())
                    text += lineBreak + StringUtils.StringWithColor(attr, error);
                else
                    text += lineBreak + attr;
            }

            if (GetRequiredLevel() > 1)
                text += InspectRequired("Level", GetRequiredLevel(), stats.level, error, text.Length > 0);

            if (GetRequiredStrength() > 0)
                text += InspectRequired("Strength", GetRequiredStrength(), stats.strength, error, text.Length > 0);

            if (GetRequiredDexterity() > 0)
                text += InspectRequired("Dexterity", GetRequiredDexterity(), stats.dexterity, error, text.Length > 0);

            if (GetRequiredEnergy() > 0)
                text += InspectRequired("Energy", GetRequiredEnergy(), stats.energy, error, text.Length > 0);

            return text;
        }

        /// <summary>
        /// Returns a new Item Instance from the Item Serializer.
        /// </summary>
        /// <param name="serializer">The Item Serializer to create the Item Instance from.</param>
        public static ItemInstance CreateFromSerializer(ItemSerializer serializer)
        {
            if (serializer == null || serializer.itemId < 0) return null;

            var item = GameDatabase.instance.FindElementById<Item>(serializer.itemId);
            var attributes = ItemAttributes.CreateFromSerializer(serializer.attributes);

            return new ItemInstance(item, attributes, serializer.durability, serializer.stack);
        }
    }
}
