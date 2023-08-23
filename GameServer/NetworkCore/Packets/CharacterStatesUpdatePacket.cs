using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkData;

namespace NetworkCore.Packets
{
    public class CharacterStatesUpdatePacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_subPacketList)]
        public List<CharacterStateUpdatePacket> PacketCollection { get; set; }

        public CharacterStatesUpdatePacket() : base(PacketType.CHARACTER_STATES_UPDATE_PACKET, false)
        {
            PacketCollection = new List<CharacterStateUpdatePacket>();
        }

        public CharacterStatesUpdatePacket(byte[] data) : base(data) { }
    }
}
