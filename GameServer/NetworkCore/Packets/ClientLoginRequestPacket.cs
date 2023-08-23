using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class ClientLoginRequestPacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_string)]
        public string Login { get; private set; } 

        [Serialization(Type: SerializationType.type_string)]
        public string Password { get; private set; } 

        public ClientLoginRequestPacket (string login, string password) : base(PacketType.LOGIN_REQUEST, false)
        {
            Login = login;
            Password = password;
        }

        public ClientLoginRequestPacket(byte[] data) : base(data) { }
    }
}
