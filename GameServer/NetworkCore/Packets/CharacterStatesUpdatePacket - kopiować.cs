using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkData;

namespace NetworkCore.Packets
{
    public class CharacterStatesPacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_subPacketList)]
        public List<CharacterStatePacket> PacketCollection { get; set; }

        public CharacterStatesPacket() : base(PacketType.CHARACTER_STATES_PACKET, false)
        {
            PacketCollection = new List<CharacterStatePacket>();
        }

        public CharacterStatesPacket(byte[] data) : base(data) { }
    }
}
