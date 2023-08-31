using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkData;

namespace NetworkCore.Packets
{
    public class CharactersAttrsUpdatePacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_subPacketList)]
        public List<CharacterAttrUpdatePacket> PacketCollection { get; set; }

        public CharactersAttrsUpdatePacket() : base(PacketType.CHARACTERS_ATTRS_UPDATE_PACKET, false)
        {
            PacketCollection = new List<CharacterAttrUpdatePacket>();
        }

        public CharactersAttrsUpdatePacket(byte[] data) : base(data) { }
    }
}
