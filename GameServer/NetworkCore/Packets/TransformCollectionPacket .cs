using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;

namespace NetworkCore.Packets
{
    // Values in that packet must be assigned. So we must overload constructor
    // with parameters.
    public class TransformCollectionPacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_subPacketList)]
        public List<TransformPacket> PacketCollection { get; set; } 

        public TransformCollectionPacket() : base(PacketType.TRANSFORM_COLLECTION_PACKET, false) {
            PacketCollection = new List<TransformPacket>();
        }

        public TransformCollectionPacket(byte[] data ) : base(data) { 

        }

        public override string GetInfo()
        {
            return "TRANSFORM_COLLECTION_PACKET, " + base.GetInfo();
        }
    }
}
