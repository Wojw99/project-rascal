using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage;

namespace NetworkCore.Packets
{
    public class TokenLoginPacket : Packet
    {
        public string Username { get { return Read<string>("Username"); } }
        public uint Key { get { return Read<uint>("Key"); } }

        public TokenLoginPacket(string username, uint key) : base(typeof(TokenLoginPacket))
        {
            Write("Username", username);
            Write("Key", key);
        }

        //public TokenLoginPacket(Packet packet) : base(packet) { }

        public TokenLoginPacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return base.ToString() + $" Username = {Username}, Key = {Key}";
        }

    }
}
