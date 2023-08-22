using NetworkCore.NetworkData;

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using NetworkCore.NetworkMessage.old;
using NetworkCore.NetworkMessage;
using NetworkCore.NetworkData.stare;

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

        [Serialization(Type: SerializationType.type_Int32)]
        public int CharacterVId { get; private set; }

        [Serialization(Type: SerializationType.type_string)]
        public string Name { get; set; }

        [Serialization(Type: SerializationType.type_Int32)]
        public int Health { get; set; }

        [Serialization(Type: SerializationType.type_Int32)]
        public int Mana { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float PosX { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float PosY { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float PosZ { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float Rot { get; set; }

        public Character GetCharacter()
        {
            return new Character(CharacterVId, Name, Health, Mana, PosX, PosY, PosZ, Rot);
        }


        public CharacterLoadResponsePacket(Character characterObj) : base(PacketType.CHARACTER_LOAD_RESPONSE, true)
        {
            Success = true;
            CharacterVId = characterObj.Vid;
            Name = characterObj.Name;
            Health = characterObj.Health;
            Mana = characterObj.Mana;
            PosX = characterObj.PositionX;
            PosY = characterObj.PositionY;
            PosZ = characterObj.PositionZ;
            Rot = characterObj.Rotation;
        }

        public CharacterLoadResponsePacket() : base(PacketType.CHARACTER_LOAD_RESPONSE, true)
        {
            Success = false;
            CharacterVId = -1;
            Name = string.Empty;
            Health = -1;
            Mana = -1;
            PosX = -1;
            PosY = -1;
            PosZ = -1;
            Rot = -1;
        }

        public CharacterLoadResponsePacket(byte[] data) : base(data)
        {

        }

        public override string ToString()
        {
            // Include Success field in the ToString() representation.
            return base.ToString() + $", Success = {Success}";
        }

        // We can store "Player" class object, because in this packet we want to receive all Player data (All Player State).

    }
}
