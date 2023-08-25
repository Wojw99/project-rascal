using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.ARPGProject
{
    [AddComponentMenu("PLAYER TWO/ARPG Project/Entity/Entity Stats Manager")]
    public class EntityStatsManager : MonoBehaviour
    {
        public UnityEvent onLevelUp;
        public UnityEvent onRecalculate;
        public UnityEvent onHealthChanged;
        public UnityEvent onManaChanged;
        public UnityEvent onExperienceChanged;

        [Header("General Settings")]
        [Tooltip("The initial level of this Entity.")]
        public int level = 1;

        [Tooltip("The initial strength of this Entity.")]
        public int strength = 20;

        [Tooltip("The initial dexterity of this Entity.")]
        public int dexterity = 15;

        [Tooltip("The initial vitality of this Entity.")]
        public int vitality = 15;

        [Tooltip("The initial energy of this Entity.")]
        public int energy = 10;

        [Header("Bot Settings")]
        [Tooltip("If true, the Entity will be able to gain experience.")]
        public bool canGainExperience = true;

        [Tooltip("If true, the current health will never decrease.")]
        public bool infiniteHealth;

        [Tooltip("If true, the current mana will never decrease.")]
        public bool infiniteMana;

        protected int m_health;
        protected int m_mana;
        protected int m_experience;

        /// <summary>
        /// Returns true if the component was initialized.
        /// </summary>
        public bool initialized { get; protected set; }

        /// <summary>
        /// Returns the amount of experience to get to the next level.
        /// </summary>
        public int nextLevelExp { get; protected set; }

        /// <summary>
        /// Returns the amount of available distribution points.
        /// </summary>
        public int availablePoints { get; protected set; }

        /// <summary>
        /// Returns the maximum health points.
        /// </summary>
        public int maxHealth { get; protected set; }

        /// <summary>
        /// Returns the maximum mana points.
        /// </summary>
        public int maxMana { get; protected set; }

        /// <summary>
        /// Returns the minimum normal attack damage.
        /// </summary>
        public int minDamage { get; protected set; }

        /// <summary>
        /// Returns the minimum normal attack damage.
        /// </summary>
        public int maxDamage { get; protected set; }

        /// <summary>
        /// Returns the minimum magic damage.
        /// </summary>
        public int minMagicDamage { get; protected set; }

        /// <summary>
        /// Returns the maximum magic damage.
        /// </summary>
        public int maxMagicDamage { get; protected set; }

        /// <summary>
        /// Returns the current defense points.
        /// </summary>
        public int defense { get; protected set; }

        /// <summary>
        /// Returns the current attack speed.
        /// </summary>
        public int attackSpeed { get; protected set; }

        /// <summary>
        /// Returns the current critical chance in percentage.
        /// </summary>
        public float criticalChance { get; protected set; }

        /// <summary>
        /// Get or set the experience points.
        /// </summary>
        public int experience
        {
            get { return m_experience; }

            protected set
            {
                m_experience = value;
                onExperienceChanged?.Invoke();
            }
        }

        /// <summary>
        /// Get or set the health points.
        /// </summary>
        public int health
        {
            get
            {
                if (infiniteHealth)
                    return maxHealth;
                return m_health;
            }

            set
            {
                m_health = Mathf.Clamp(value, 0, maxHealth);
                onHealthChanged?.Invoke();
            }
        }

        /// <summary>
        /// Get or set the mana points.
        /// </summary>
        public int mana
        {
            get
            {
                if (infiniteMana)
                    return maxMana;

                return m_mana;
            }

            set
            {
                m_mana = Mathf.Clamp(value, 0, maxMana);
                onManaChanged?.Invoke();
            }
        }

        protected EntityItemManager m_items;
        protected EntitySkillManager m_skills;
        protected ItemFinalAttributes m_attributes;

        /// <summary>
        /// Initializes the Stats Manager.
        /// </summary>
        public virtual void Initialize()
        {
            if (initialized) return;

            InitializeItems();
            InitializeSkills();
            Recalculate();
            Revitalize();

            initialized = true;
        }

        protected virtual void InitializeItems()
        {
            m_items = GetComponent<EntityItemManager>();

            if (m_items)
                m_items.onChanged.AddListener(Recalculate);
        }

        protected virtual void InitializeSkills() => m_skills = GetComponent<EntitySkillManager>();

        /// <summary>
        /// Bulk update all stats points and recalculate the stats.
        /// </summary>
        public virtual void BulkUpdate(int level, int strength, int dexterity,
            int vitality, int energy, int availablePoints, int experience)
        {
            this.level = level;
            this.strength = strength;
            this.dexterity = dexterity;
            this.vitality = vitality;
            this.energy = energy;
            this.availablePoints = availablePoints;
            this.experience = experience;
            Recalculate();
        }

        /// <summary>
        /// Bulk distribute the available distribution points and recalculate the stats.
        /// </summary>
        public virtual void BulkDistribute(int strength, int dexterity, int vitality, int energy)
        {
            this.strength += strength;
            this.dexterity += dexterity;
            this.vitality += vitality;
            this.energy += energy;
            this.availablePoints -= strength + dexterity + vitality + energy;
            Recalculate();
        }

        /// <summary>
        /// Gets the current health in a 0 to 1 range.
        /// </summary>
        public virtual float GetHealthPercent() => health / (float)maxHealth;

        /// <summary>
        /// Gets the current mana in a 0 to 1 range.
        /// </summary>
        public virtual float GetManaPercent() => mana / (float)maxMana;

        /// <summary>
        /// Returns the current experience in a 0 to 1 range.
        /// </summary>
        public virtual float GetExperiencePercent() => experience / (float)nextLevelExp;

        /// <summary>
        /// Calculates the normal attack damage points with the critical multiplier.
        /// </summary>
        /// <param name="critical">If true, the damage is critical.</param>
        public virtual int GetDamage(out bool critical) =>
            (int)(GetCriticalMultiplier(out critical) * GetFinalDamage());

        /// <summary>
        /// Calculates the magic attack damage points using a given skill with the critical multiplier.
        /// </summary>
        /// <param name="skill">The skill you want to calculate damage for.</param>
        /// <param name="critical">If true, the damage is critical.</param>
        public virtual int GetSkillDamage(Skill skill, out bool critical) =>
            (int)(GetCriticalMultiplier(out critical) * GetSkillDamage(skill));

        /// <summary>
        /// Return the attack animation speed multiplier based on the attack speed stat.
        /// </summary>
        public virtual float GetAnimationAttackSpeed() =>
            attackSpeed / (float)Game.instance.maxAttackSpeed;

        /// <summary>
        /// Returns the final normal damage points.
        /// </summary>
        protected virtual int GetFinalDamage() =>
            (int)(Random.Range(minDamage, maxDamage) * m_attributes.damageMultiplier);

        /// <summary>
        /// Returns the final magical damage points.
        /// </summary>
        protected virtual int GetFinalMagicDamage() =>
            (int)(Random.Range(minMagicDamage, maxMagicDamage) * m_attributes.damageMultiplier);

        /// <summary>
        /// Returns the magic damage points given a skill.
        /// </summary>
        /// <param name="skill">The skill you want to calculate magic damage from.</param>
        protected virtual int GetSkillDamage(Skill skill)
        {
            if (!skill || !skill.IsAttack()) return 0;

            var damage = skill.AsAttack().GetDamage();

            switch (skill.AsAttack().damageMode)
            {
                default:
                case SkillAttack.DamageMode.Regular:
                    damage += GetFinalDamage();
                    break;
                case SkillAttack.DamageMode.Magic:
                    damage += GetFinalMagicDamage();
                    break;
            }

            return damage;
        }

        /// <summary>
        /// Returns the minimum and maximum damage calculated from the equipped items.
        /// </summary>
        protected virtual (int minDamage, int maxDamage) GetItemsDamage()
        {
            if (!m_items) return (0, 0);

            return m_items.GetDamage();
        }

        /// <summary>
        /// Returns the defense points calculated from the equipped items.
        /// </summary>
        protected virtual int GetItemsDefense()
        {
            if (!m_items) return 0;

            return m_items.GetDefense();
        }

        /// <summary>
        /// Returns the attack speed points calculated from the equipped items.
        /// </summary>
        protected virtual int GetItemsAttackSpeed()
        {
            if (!m_items) return 0;

            return m_items.GetAttackSpeed();
        }

        /// <summary>
        /// Sets all the attribute points from the equipped items.
        /// </summary>
        protected virtual void SetItemsAttributes()
        {
            if (m_items)
            {
                m_attributes = m_items.GetFinalAttributes();
                return;
            }

            m_attributes = new ItemFinalAttributes();
        }

        /// <summary>
        /// Calculates and return the critical multiplier in percentage.
        /// </summary>
        /// <param name="success">If true, the critical is successful.</param>
        protected virtual float GetCriticalMultiplier(out bool success)
        {
            success = Random.value > 1 - criticalChance;
            return success ? Game.instance.criticalMultiplier : 1;
        }

        /// <summary>
        /// Calculates and return the experience points required to reach the next level.
        /// </summary>
        protected virtual int GetNextLevelExperience() =>
            Game.instance.baseExperience + (Game.instance.experiencePerLevel * (level - 1));

        /// <summary>
        /// Recalculates the points for all the Entity's dynamic stats.
        /// </summary>
        public virtual void Recalculate()
        {
            SetItemsAttributes();

            var weaponDamage = GetItemsDamage();

            nextLevelExp = GetNextLevelExperience();
            maxHealth = (int)(((level * 10 + vitality * 2) + m_attributes.health) * m_attributes.healthMultiplier);
            maxMana = (int)(((level * 5 + energy * 2) + m_attributes.mana) * m_attributes.manaMultiplier);
            minDamage = (strength / 8) + weaponDamage.minDamage + m_attributes.damage;
            maxDamage = (strength / 4) + weaponDamage.maxDamage + m_attributes.damage;
            attackSpeed = Mathf.Min((dexterity + GetItemsAttackSpeed()) / 10 + m_attributes.attackSpeed, Game.instance.maxAttackSpeed);
            minMagicDamage = energy / 4;
            maxMagicDamage = energy / 2;
            criticalChance = (dexterity / 10 + 20) / 100f * m_attributes.criticalChanceMultiplier;
            defense = (int)(((dexterity / 4) + GetItemsDefense() + m_attributes.defense) * m_attributes.defenseMultiplier);
            health = Mathf.Min(health, maxHealth);
            mana = Mathf.Min(mana, maxMana);

            onRecalculate?.Invoke();
        }

        /// <summary>
        /// Restores the current health and mana points to its maximum values.
        /// </summary>
        public virtual void Revitalize()
        {
            ResetHealth();
            ResetMana();
        }

        /// <summary>
        /// Sets the health points to its maximum value.
        /// </summary>
        public virtual void ResetHealth() => health = maxHealth;

        /// <summary>
        /// Sets the mana points to its maximum value.
        /// </summary>
        public virtual void ResetMana() => mana = maxMana;

        /// <summary>
        /// Level Up the Entity, consuming the experience points, and recalculating all its dynamic stats.
        /// </summary>
        public virtual void LevelUp()
        {
            while (experience >= nextLevelExp)
            {
                level++;
                experience -= nextLevelExp;
                availablePoints += Game.instance.levelUpPoints;
                nextLevelExp = GetNextLevelExperience();
            }

            Recalculate();
            Revitalize();

            onLevelUp?.Invoke();
        }

        /// <summary>
        /// Add experience points to the Entity.
        /// </summary>
        /// <param name="amount">The amount of experience points you want to add.</param>
        public virtual void AddExperience(int amount)
        {
            if (!canGainExperience) return;

            experience += amount;

            if (experience >= nextLevelExp)
                LevelUp();
        }

        /// <summary>
        /// Set the current experience points to zero.
        /// </summary>
        public virtual void ResetExperience() => experience = 0;

        /// <summary>
        /// Calculates and sets the experience points acquired by defeating a given Entity.
        /// </summary>
        /// <param name="other">The Entity you want to use as a base to the calculation.</param>
        public virtual void OnDefeatEntity(Entity other) =>
            AddExperience(Game.instance.baseEnemyDefeatExperience * other.stats.level);

        protected virtual void Start() => Initialize();
    }
}
