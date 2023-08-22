using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using NetworkCore.NetworkData.stare;
using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage.old;
using NetworkCore.NetworkMessage;

namespace NetworkCore.Packets
{
    // Values in that packet must be assigned. So we must overload constructor
    // with parameters.
    public class CharacterMovePacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_Int32)]
        public int CharacterVId { get; }

        [Serialization(Type: SerializationType.type_float)]
        public float PosX { get; } 

        [Serialization(Type: SerializationType.type_float)]
        public float PosY { get; } 

        [Serialization(Type: SerializationType.type_float)]
        public float PosZ { get; } 

        [Serialization(Type: SerializationType.type_float)]
        public float Rot { get; } 

        // There is one overload, because we want to always store all values.
        // And also player Virtual Id must be correct.
        public CharacterMovePacket(Character PlayerObj) : base(PacketType.CHARACTER_MOVE_PACKET, false)
        {
            CharacterVId = PlayerObj.Vid;
            PosX = PlayerObj.PositionX;
            PosY = PlayerObj.PositionY;
            PosZ = PlayerObj.PositionZ;
            Rot = PlayerObj.Rotation;
        }

        //public CharacterMovePacket(PacketBase packet) : base(packet) {}

        public CharacterMovePacket(byte[] data ) : base(data) {}

        public override string ToString()
        {
            return "";
            //return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
               // $"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
