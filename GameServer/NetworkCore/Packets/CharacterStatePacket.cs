using NetworkCore.NetworkData;
using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage;
using System.Numerics;

namespace NetworkCore.Packets
{
    // That packet is dynamic in my sense. Values in here can be null, so
    // we can decide which values to store in that packet. This practice
    // can be helpfull if we dont need to send values, that doesn't change.
    public class CharacterStatePacket : PacketBase
    {
        // Player VId cannot be null. See the description of this parameter in the Player class.

        [Serialization(Type: SerializationType.type_Int32)]
        public int CharacterVId { get; set; } = -1;

        [Serialization(Type: SerializationType.type_string)]
        public string Name { get; set; } = string.Empty;

        [Serialization(Type: SerializationType.type_Int32)]
        public float CurrentHealth { get; set; } = -1;

        [Serialization(Type: SerializationType.type_Int32)]
        public float MaxHealth { get; set; } = -1;

        [Serialization(Type: SerializationType.type_Int32)]
        public float CurrentMana { get; set; } = -1;

        [Serialization(Type: SerializationType.type_Int32)]
        public float MaxMana { get; set; } = -1;

        [Serialization(Type: SerializationType.type_float)]
        public float PosX { get; set; } = -1;

        [Serialization(Type: SerializationType.type_float)]
        public float PosY { get; set; } = -1;

        [Serialization(Type: SerializationType.type_float)]
        public float PosZ { get; set; } = -1;

        [Serialization(Type: SerializationType.type_float)]
        public float Rot { get; set; } = -1;

        // Be 100% sure this is the correct unique identificator of player.
        public CharacterStatePacket(int characterVId) : base(PacketType.CHARACTER_STATE_PACKET, false)
        {
            CharacterVId = characterVId;
        }

        public CharacterStatePacket(Character player) : base(PacketType.CHARACTER_STATE_PACKET, false)
        {
            CharacterVId = player.Vid;
            Name = player.Name;
            CurrentHealth = player.CurrentHealth;
            MaxHealth = player.MaxHealth;
            CurrentMana = player.CurrentMana;
            MaxMana = player.MaxMana;
            CurrentMana = player.CurrentMana;
            PosX =  player.PositionX;
            PosY = player.PositionY;
            PosZ = player.PositionZ;
            Rot = player.Rotation;
        }

        public CharacterStatePacket(byte[] data) : base(data) { }

        public Character GetCharacter()
        {
            return new Character(CharacterVId, Name, CurrentHealth, MaxHealth, CurrentMana, MaxMana, PosX, PosY, PosZ, Rot);
        }

        public override string ToString()
        {
            return "";
            // return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
            //$"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
