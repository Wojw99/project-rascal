using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkData;

namespace NetworkCore.Packets
{
    public class AttributesCollectionPacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_subPacketList)]
        public List<AttributesPacket> PacketCollection { get; set; }

        public AttributesCollectionPacket() : base(PacketType.ATTRIBUTES_COLLECTION_PACKET, false)
        {
            PacketCollection = new List<AttributesPacket>();
        }

        public AttributesCollectionPacket(byte[] data) : base(data) { }

        public override string GetInfo()
        {
            return "ATTRIBUTES COLLECTION PACKET";
        }
    }
}
