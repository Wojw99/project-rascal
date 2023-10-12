using NetworkCore.NetworkData;

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using NetworkCore.NetworkMessage;
using System.Xml.Linq;

namespace NetworkCore.Packets
{
    // This is response packet for CharacterLoadRequestPacket. We storing here succes flag of operation,
    // and also Character object, which is the current state of Player.
    // It is important to initialize this packet with data from the database.
    // We are setting success flag to false by default
    public class CharacterLoadResponsePacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_bool)]
        public bool Success { get; private set; } // 1 - true

        [Serialization(Type: SerializationType.type_subPacket)]
        public AttributesPacket AttributesPacket { get; set; }

        [Serialization(Type: SerializationType.type_subPacket)]
        public TransformPacket TransformPacket { get; set; }

        public CharacterLoadResponsePacket(Character characterObj) : base(PacketType.CHARACTER_LOAD_RESPONSE, true)
        {
            AttributesPacket = new AttributesPacket(characterObj.Vid);
            TransformPacket = new TransformPacket(characterObj.Vid);
            Success = true;

            AttributesPacket.Name = characterObj.Name;
            AttributesPacket.CurrentHealth = characterObj.CurrentHealth;
            AttributesPacket.MaxHealth = characterObj.MaxHealth;
            AttributesPacket.CurrentMana = characterObj.CurrentMana;
            AttributesPacket.MaxMana = characterObj.MaxMana;
            TransformPacket.PosX = characterObj.PositionX;
            TransformPacket.PosY = characterObj.PositionY;
            TransformPacket.PosZ = characterObj.PositionZ;
        }

        public CharacterLoadResponsePacket() : base(PacketType.CHARACTER_LOAD_RESPONSE, true)
        {
            Success = false;
            AttributesPacket = new AttributesPacket(-1);
            TransformPacket = new TransformPacket(-1);
        }

        public CharacterLoadResponsePacket(byte[] data) : base(data)
        {
            AttributesPacket = new AttributesPacket(-1);
            TransformPacket = new TransformPacket(-1);
        }

        public override string ToString()
        {
            // Include Success field in the ToString() representation.
            return base.ToString() + $", Success = {Success}";
        }

        // We can store "Player" class object, because in this packet we want to receive all Player data (All Player State).

    }
}
