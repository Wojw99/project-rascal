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
        public int CharacterVId { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float PosX { get; set; } 

        [Serialization(Type: SerializationType.type_float)]
        public float PosY { get; set; } 

        [Serialization(Type: SerializationType.type_float)]
        public float PosZ { get; set; } 

        [Serialization(Type: SerializationType.type_float)]
        public float Rot { get; set; } 

        // There is one overload, because we want to always store all values.
        // And also player Virtual Id must be correct.
        public CharacterMovePacket(int characterVId, float posX, float posY, float posZ, float rotation) : base(PacketType.CHARACTER_MOVE_PACKET, false)
        {
            CharacterVId = characterVId;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
            Rot = rotation;
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
