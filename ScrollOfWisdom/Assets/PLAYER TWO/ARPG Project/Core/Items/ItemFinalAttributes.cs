namespace PLAYERTWO.ARPGProject
{
    public class ItemFinalAttributes
    {
        public int damage = 0;
        public int attackSpeed = 0;
        public int defense = 0;
        public int mana = 0;
        public int health = 0;
        public float criticalChanceMultiplier = 1f;
        public float damageMultiplier = 1f;
        public float defenseMultiplier = 1f;
        public float manaMultiplier = 1f;
        public float healthMultiplier = 1f;

        public ItemFinalAttributes() { }

        public ItemFinalAttributes(
            int damage,
            int attackSpeed,
            int defense,
            int mana,
            int health,
            float criticalChanceMultiplier,
            float damageMultiplier,
            float defenseMultiplier,
            float manaMultiplier,
            float healthMultiplier)
        {
            this.damage = damage;
            this.attackSpeed = attackSpeed;
            this.defense = defense;
            this.mana = mana;
            this.health = health;
            this.criticalChanceMultiplier = criticalChanceMultiplier;
            this.damageMultiplier = damageMultiplier;
            this.defenseMultiplier = defenseMultiplier;
            this.manaMultiplier = manaMultiplier;
            this.healthMultiplier = healthMultiplier;
        }
    }
}
