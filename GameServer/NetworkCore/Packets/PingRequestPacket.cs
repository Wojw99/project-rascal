using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage;

namespace NetworkCore.Packets
{
    public class PingRequestPacket : Packet
    {
        public PingRequestPacket() : base(typeof(PingRequestPacket)) {}

        public PingRequestPacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
