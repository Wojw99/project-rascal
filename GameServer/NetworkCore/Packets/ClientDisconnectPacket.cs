using NetworkCore.NetworkMessage;
using NetworkCore.NetworkMessage.old;
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

        public ClientDisconnectPacket(string authToken) : base(PacketType.CLIENT_DISCONNECT)
        {
            AuthToken = authToken;
        }

        //public ClientDisconnectPacket(PacketBase packet) : base(packet) { } 

        public ClientDisconnectPacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
