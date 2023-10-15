using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    // Packet with Succes operation flag of loading character on client-side.
    public class CharacterLoadSuccesPacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_bool)]
        public bool Succes { get; private set; } 

        public CharacterLoadSuccesPacket(bool succes) : base(PacketType.CHARACTER_LOAD_SUCCES, false)
        {
            Succes = succes;
        }

        public CharacterLoadSuccesPacket(byte[] data) : base(data) { }

        public override string GetInfo()
        {
            return "CHARACTER LOAD SUCCES PACKET, " + base.GetInfo();
        }
    }
}
