using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    // Simple packet about client disconnect. I dont know if we need AuthToken in here.
    public class ClientDisconnectPacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_string)]
        public string AuthToken { get; } = string.Empty;

        public ClientDisconnectPacket(string authToken) : base(PacketType.CLIENT_DISCONNECT, false)
        {
            AuthToken = authToken;
        }

        //public ClientDisconnectPacket(PacketBase packet) : base(packet) { } 

        public ClientDisconnectPacket(byte[] data) : base(data) { }

        public override string GetInfo()
        {
            return "CLIENT DISCONNECT PACKET, " + base.GetInfo();
        }
    }
}
