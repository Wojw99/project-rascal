using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class PlayerLoadRequestPacket : Packet
    {
        //public int VId { get { return Read<int>("VId"); } }

        /*public PlayerLoadRequestPacket(int VId) : base(typeof(PlayerLoadRequestPacket))
        {
            Write("VId", VId);
        }*/

        public string AuthToken { get { return Read<string>("AuthToken"); } }

        public PlayerLoadRequestPacket(string authToken) : base(typeof(PlayerLoadRequestPacket))
        {
            Write("AuthToken", authToken);
        }

        public PlayerLoadRequestPacket(Packet packet) : base(packet) { } // packet is PlayerLoadRequestPacket - trzeba dodac

        public PlayerLoadRequestPacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
