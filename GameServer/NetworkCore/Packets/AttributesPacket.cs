using NetworkCore.NetworkData;
using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage;
using System.Numerics;
using NetworkCore.NetworkUtility;

namespace NetworkCore.Packets
{
    // That packet is dynamic in my sense. Values in here can be null, so
    // we can decide which values to store in that packet. This practice
    // can be helpfull if we dont need to send values, that doesn't change.
    public class AttributesPacket : PacketBase
    {
        // Player VId cannot be null. See the description of this parameter in the Player class.

        [Serialization(Type: SerializationType.type_Int32)]
        public int CharacterVId { get; set; }

        [Serialization(Type: SerializationType.type_string)]
        public string Name { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float CurrentHealth { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float MaxHealth { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float CurrentMana { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float MaxMana { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float MoveSpeed { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float AttackSpeed { get; set; }

        [Serialization(Type: SerializationType.type_Int32)]
        public AdventurerState State { get; set; }

        // Be 100% sure this is the correct unique identificator of player.
        public AttributesPacket(int characterVId) : base(PacketType.ATTRIBUTES_PACKET, false)
        {
            CharacterVId = characterVId;
        }

        public AttributesPacket(int characterVId, string name, float currentHealth, 
            float maxHealth, float currentMana, float maxMana, float moveSpeed, 
            float attackSpeed, AdventurerState state) 
            : base(PacketType.ATTRIBUTES_PACKET, false)
        {
            CharacterVId = characterVId;
            Name = name;
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            CurrentMana = currentMana;
            MaxMana = maxMana;
            MoveSpeed = moveSpeed;
            AttackSpeed = attackSpeed;
            State = state;
        }

        public AttributesPacket(byte[] data) : base(data) { }

        /*public Character GetCharacter()
        {
            return new Character(CharacterVId, Name, CurrentHealth, MaxHealth, CurrentMana, MaxMana);
        }*/

        public override string GetInfo()
        {
            return "ATTRIBUTES PACKET, " + base.GetInfo();
            // return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
            //$"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
