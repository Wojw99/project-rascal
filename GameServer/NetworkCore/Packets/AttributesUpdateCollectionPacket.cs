using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkData;

namespace NetworkCore.Packets
{
    public class AttributesUpdateCollectionPacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_subPacketList)]
        public List<AttributesUpdatePacket> PacketCollection { get; set; }

        public AttributesUpdateCollectionPacket() : base(PacketType.ATTRIBUTES_COLLECTION_UPDATE_PACKET, false)
        {
            PacketCollection = new List<AttributesUpdatePacket>();
        }

        public AttributesUpdateCollectionPacket(byte[] data) : base(data) { }

        public override string GetInfo()
        {
            return "ATTRIBUTES COLLECTION UPDATE PACKET, " + base.GetInfo();
        }
    }
}
