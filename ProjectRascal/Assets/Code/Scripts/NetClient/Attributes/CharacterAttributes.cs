using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.NetClient
{
    public class CharacterAttributes
    {
        public int characterVId;
        public string name;
        public float currentHealth;
        public float currentMana;
        public float maxHealth;
        public float maxMana;
        public float attack;
        public float magic;

        public CharacterAttributes(int characterVId)
        {
            this.characterVId = characterVId;
            this.name = string.Empty;
            this.currentHealth = 0;
            this.currentMana = 0;
            this.maxHealth = 0;
            this.maxMana = 0;
            this.attack = 0;
            this.magic = 0;
        }

        public CharacterAttributes(int characterVId, string name, float currentHealth, float currentMana, float maxHealth, float maxMana, float attack, float magic) : this(characterVId)
        {
            this.characterVId = characterVId;
            this.name = name;
            this.currentHealth = currentHealth;
            this.currentMana = currentMana;
            this.maxHealth = maxHealth;
            this.maxMana = maxMana;
            this.attack = attack;
            this.magic = magic;
        }
    }
}
