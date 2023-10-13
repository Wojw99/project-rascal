using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class AdventurerLoadPacket : PacketBase
    {
        //[Serialization(Type: SerializationType.type_bool)]
        //public bool Success { get; private set; } // 1 - true

        [Serialization(Type: SerializationType.type_subPacket)]
        public AttributesPacket AttributesPacket { get; set; }

        [Serialization(Type: SerializationType.type_subPacket)]
        public TransformPacket TransformPacket { get; set; }


        public AdventurerLoadPacket() : base(PacketType.ADVENTURER_LOAD_PACKET, false)
        {
            AttributesPacket = new AttributesPacket(-1);
            TransformPacket = new TransformPacket(-1);
        }

        public AdventurerLoadPacket(byte[] data) : base(data) 
        {
            AttributesPacket = new AttributesPacket(-1);
            TransformPacket = new TransformPacket(-1);
        }
    }
}
