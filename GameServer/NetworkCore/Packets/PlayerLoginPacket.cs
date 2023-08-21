/*using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage.old;

namespace NetworkCore.Packets
{
    public class PlayerLoginPacket : PacketBase
    {
        public string Username { get { return Read<string>("Username"); } }
        public uint Key { get { return Read<uint>("Key"); } }

        public PlayerLoginPacket(string username, uint key) : base(typeof(PlayerLoginPacket))
        {
            Write("Username", username);
            Write("Key", key);
        }

        //public TokenLoginPacket(Packet packet) : base(packet) { }

        public PlayerLoginPacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return base.ToString() + $" Username = {Username}, Key = {Key}";
        }

    }
}
*/