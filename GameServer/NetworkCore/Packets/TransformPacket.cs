using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;

namespace NetworkCore.Packets
{
    // Values in that packet must be assigned. So we must overload constructor
    // with parameters.
    public class TransformPacket : PacketBase
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
        public float RotX { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float RotY { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float RotZ { get; set; }

        public TransformPacket(int characterVId) : base(PacketType.TRANSFORM_PACKET, false)
        {
            CharacterVId = characterVId;
            PosX = 0;
            PosY = 0;
            PosZ = 0;
            RotX = 0;
            RotY = 0;
            RotZ = 0;
        }

        // There is one overload, because we want to always store all values.
        // And also player Virtual Id must be correct.
        public TransformPacket(int characterVId, float posX, float posY, float posZ, float rotX, float rotY, float rotZ) : base(PacketType.TRANSFORM_PACKET, false)
        {
            CharacterVId = characterVId;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
            RotX = rotX;
            RotY = rotY;
            RotZ = rotZ;
        }

        //public CharacterMovePacket(PacketBase packet) : base(packet) {}

        public TransformPacket(byte[] data ) : base(data) {}

        public override string GetInfo()
        {
            return "TRANSFORM PACKET, " + base.GetInfo();
            //return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
            // $"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
