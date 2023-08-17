using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class PlayerLoadSuccesPacket : Packet
    {
        public bool Succes { get { return Read<int>("Succes") == 1; } } // 1 - true

        public PlayerLoadSuccesPacket(bool succes) : base(typeof(PlayerLoadSuccesPacket))
        {
            Write<int>("Succes", succes ? 1 : 0);
        }

        public PlayerLoadSuccesPacket(Packet packet) : base(packet) { }

        public PlayerLoadSuccesPacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
