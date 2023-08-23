using NetworkCore.NetworkData;
using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage.old;
using NetworkCore.NetworkMessage;
using NetworkCore.NetworkData.stare;
using System.Numerics;

namespace NetworkCore.Packets
{
    // That packet is dynamic in my sense. Values in here can be null, so
    // we can decide which values to store in that packet. This practice
    // can be helpfull if we dont need to send values, that doesn't change.
    public class CharacterStateUpdatePacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_string)]
        public string? Name { get; set; }

        [Serialization(Type: SerializationType.type_Int32)]
        public int? Health { get; set; }

        [Serialization(Type: SerializationType.type_Int32)]
        public int? Mana { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float? PosX { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float? PosY { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float? PosZ { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float? Rot { get; set; }

        public void Clear()
        {
            Name = null;
            Health = null;
            Mana = null;
            PosX = null;
            PosY = null;
            PosZ = null;
            Rot = null;
        }
        public CharacterStateUpdatePacket() : base(PacketType.CHARACTER_STATE_UPDATE_PACKET, false) { }

        public CharacterStateUpdatePacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
            // return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
            //$"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
