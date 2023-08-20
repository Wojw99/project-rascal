using NetworkCore.NetworkMessage;
using NetworkCore.NetworkData;

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace NetworkCore.Packets
{
    // This is response packet for CharacterLoadRequestPacket. We storing here succes flag of operation,
    // and also Character object, which is the current state of Player.
    // It is important to initialize this packet with data from the database.
    public class CharacterLoadResponsePacket : Packet
    {
        public bool Succes { get { return Read<int>("Succes") == 1; } } // 1 - true

        // We can store "Player" class object, because in this packet we want to receive all Player data (All Player State).
        public Character CharacterObj { 
            get { 
                if(Succes)
                {
                   return new Character(Read<int>("VId"), Read<string>("Name"), Read<int>("Health"), Read<int>("Mana"), Read<float>("PositionX"),
                        Read<float>("PositionY"), Read<float>("PositionZ"), Read<float>("Rotation"));
                }
                throw new ArgumentException("Cannot create CharacterObj in CharacterLoadResponsePacket, while succes status is false. ");
            } }

        public CharacterLoadResponsePacket(bool succes) : base(typeof(CharacterLoadResponsePacket))
        {
            Write<int>("Succes", succes ? 1 : 0);
        }

        public CharacterLoadResponsePacket(bool succes, Character player) : base(typeof(CharacterLoadResponsePacket))
        {
            Write<int>("Succes", succes ? 1 : 0);
            Write("VId", player.Vid);
            Write("Name", player.Name);
            Write("Health", player.Health);
            Write("Mana", player.Mana);
            Write("PositionX", player.PositionX);
            Write("PositionY", player.PositionY);
            Write("PositionZ",player.PositionZ);
            Write("Rotation",player.Rotation);
        }


        public CharacterLoadResponsePacket(Packet packet) : base(packet) { }

        public CharacterLoadResponsePacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
