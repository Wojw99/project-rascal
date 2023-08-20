using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class PingResponsePacket : Packet
    {
        public PingResponsePacket() : base(typeof(PingRequestPacket)) { }

        public PingResponsePacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
