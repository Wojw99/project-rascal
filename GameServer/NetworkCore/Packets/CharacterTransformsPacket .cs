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
    public class CharacterTransformsPacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_subPacketList)]
        public List<CharacterTransformPacket> PacketCollection { get; set; } 

        public CharacterTransformsPacket() : base(PacketType.CHARACTER_TRANSFORMS_PACKET, false) {
            PacketCollection = new List<CharacterTransformPacket>();
        }

        public CharacterTransformsPacket(byte[] data ) : base(data) { 
            PacketCollection = new List<CharacterTransformPacket>(); 
        }

        public override string ToString()
        {
            return "";
        }
    }
}
