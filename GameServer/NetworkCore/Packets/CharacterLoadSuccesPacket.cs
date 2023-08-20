using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    // Packet with Succes operation flag of loading character on client-side.
    public class CharacterLoadSuccesPacket : Packet
    {
        public bool Succes { get { return Read<int>("Succes") == 1; } } // 1 - true

        public CharacterLoadSuccesPacket(bool succes) : base(typeof(CharacterLoadSuccesPacket))
        {
            Write<int>("Succes", succes ? 1 : 0);
        }

        public CharacterLoadSuccesPacket(Packet packet) : base(packet) { }

        public CharacterLoadSuccesPacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
