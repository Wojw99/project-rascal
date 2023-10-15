using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class ClientLoginResponsePacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_string)]
        public string AuthToken { get; private set; }

        public ClientLoginResponsePacket (string authToken) : base(PacketType.LOGIN_RESPONSE, true)
        {
            AuthToken = authToken;
        }

        public ClientLoginResponsePacket(byte[] data) : base(data) { }
        public override string GetInfo()
        {
            return "LOGIN RESPONSE PACKET, " + base.GetInfo();
        }
    }
}
