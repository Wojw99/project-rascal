using NetworkCore.NetworkUtility;
using NetworkCore.Packets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.NetClient
{
    public class AdventurerAttributesData
    {
        public int CharacterVId;
        public string Name;
        public float CurrentHealth;
        public float CurrentMana;
        public float MaxHealth;
        public float MaxMana;
        public float Attack;
        public float Magic;
        public float MoveSpeed;
        public float AttackSpeed;
        public AdventurerState State;

        public AdventurerAttributesData(int characterVId)
        {
            this.CharacterVId = characterVId;
            this.Name = string.Empty;
            this.CurrentHealth = 0;
            this.CurrentMana = 0;
            this.MaxHealth = 0;
            this.MaxMana = 0;
            this.Attack = 0;
            this.Magic = 0;
            this.MoveSpeed = 0;
            this.AttackSpeed = 0;
            State = AdventurerState.Idle;
        }

        public AdventurerAttributesData(int characterVId, string name, float currentHealth, 
            float currentMana, float maxHealth, float maxMana, float attack, float magic, 
            float moveSpeed, float attackSpeed, AdventurerState state) : this(characterVId)
        {
            this.CharacterVId = characterVId;
            this.Name = name;
            this.CurrentHealth = currentHealth;
            this.CurrentMana = currentMana;
            this.MaxHealth = maxHealth;
            this.MaxMana = maxMana;
            this.Attack = attack;
            this.Magic = magic;
            this.MoveSpeed = moveSpeed; 
            this.AttackSpeed = attackSpeed;
            this.State = state;
        }

        public AdventurerAttributesData(AttributesPacket AttrPacket)
        {
            this.CharacterVId = AttrPacket.CharacterVId;
            this.Name = AttrPacket.Name;
            this.CurrentHealth = AttrPacket.CurrentHealth;
            this.CurrentMana = AttrPacket.CurrentMana;
            this.MaxHealth = AttrPacket.MaxHealth;
            this.MaxMana = AttrPacket.MaxMana;
            this.Attack = 0;//AttrPacket.Attack;
            this.Magic = 0;//AttrPacket.Magic;
            this.MoveSpeed = AttrPacket.MoveSpeed; 
            this.AttackSpeed = AttrPacket.AttackSpeed;
            this.State = AttrPacket.State;
        }
    }
}
