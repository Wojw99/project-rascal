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
    public class AttributesUpdatePacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_Int32)]
        public int CharacterVId { get; set; }

        [Serialization(Type: SerializationType.type_string)]
        public string? Name { get; set; }

        [Serialization(Type: SerializationType.type_Int32)]
        public float? CurrentHealth { get; set; } = -1;

        [Serialization(Type: SerializationType.type_Int32)]
        public float? MaxHealth { get; set; } = -1;

        [Serialization(Type: SerializationType.type_Int32)]
        public float? CurrentMana { get; set; } = -1;

        [Serialization(Type: SerializationType.type_Int32)]
        public float? MaxMana { get; set; } = -1;

        public void Clear()
        {
            // We doesnt clear the id! Because it is unique for all characters.
            Name = null;
            CurrentHealth = null;
            MaxHealth = null;
            CurrentMana = null;
            MaxMana = null;
        }
        public AttributesUpdatePacket(int characterVId) : base(PacketType.ATTRIBUTES_UPDATE_PACKET, false) { CharacterVId = characterVId; }

        public AttributesUpdatePacket(byte[] data) : base(data) { }

        public override string GetInfo()
        {
            return "ATTRIBUTES UPDATE PACKET";
            // return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
            //$"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
