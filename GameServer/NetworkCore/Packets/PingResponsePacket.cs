using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class PingResponsePacket : PacketBase
    {
        public PingResponsePacket() : base(PacketType.PING_RESPONSE, true) { }

        public PingResponsePacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
