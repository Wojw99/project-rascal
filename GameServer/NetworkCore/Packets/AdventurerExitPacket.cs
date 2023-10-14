using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage;

namespace NetworkCore.Packets
{
    // We send this packet to all connected players, when client send to
    // server ClientDisconnectPacket. We must delete disconnecting player
    // character from clients player collections.
    public class AdventurerExitPacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_Int32)]
        public int CharacterVId { get; set; }

        public AdventurerExitPacket(int characterVId) : base (PacketType.CHARACTER_EXIT_PACKET, false) { CharacterVId = characterVId; }
        public AdventurerExitPacket(byte[] data) : base(data) { }
        public override string GetInfo()
        {
            return "CHARACTER EXIT PACKET";
        }
    }
}
