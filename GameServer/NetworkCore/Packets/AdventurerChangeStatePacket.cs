/*// I wonder we need calculating AdventuerState on server-side

using NetworkCore.NetworkMessage;
using NetworkCore.NetworkUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class AdventurerChangeStatePacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_Int32)]
        public AdventurerState State { get; set; }

        public AdventurerChangeStatePacket(int characterVId) : base(PacketType.CHARACTER_EXIT_PACKET, false) { CharacterVId = characterVId; }
        public AdventurerChangeStatePacket(byte[] data) : base(data) { }
        public override string GetInfo()
        {
            return "CHARACTER EXIT PACKET, " + base.GetInfo();
        }

    }
}
*/