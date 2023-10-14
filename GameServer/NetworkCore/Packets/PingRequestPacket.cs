using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage;

namespace NetworkCore.Packets
{
    public class PingRequestPacket : PacketBase
    {
        public PingRequestPacket() : base(PacketType.PING_REQUEST, false) {}

        public PingRequestPacket(byte[] data) : base(data) { }

        public override string GetInfo()
        {
            return "PING REQUEST PACKET";
        }
    }
}
