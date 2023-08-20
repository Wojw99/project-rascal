using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    // Simple packet about client disconnect. I dont know if we need AuthToken in here.
    public class ClientDisconnectPacket : Packet
    {
        public string AuthToken { get { return Read<string>("AuthToken"); } }

        public ClientDisconnectPacket(string authToken) : base(typeof(ClientDisconnectPacket))
        {
            Write("AuthToken", authToken);
        }

        public ClientDisconnectPacket(Packet packet) : base(packet) { } 

        public ClientDisconnectPacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
