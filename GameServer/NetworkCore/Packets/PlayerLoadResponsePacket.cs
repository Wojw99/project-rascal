using NetworkCore.NetworkMessage;
using NetworkCore.NetworkData;

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace NetworkCore.Packets
{
    public class PlayerLoadResponsePacket : Packet
    {
        public bool Succes { get { return Read<int>("Succes") == 1; } } // 1 - true

        // We can store "Player" class object, because in this packet we want to receive all Player data (All Player State).
        public Player PlayerObj { 
            get { 
                if(Succes)
                {
                   return new Player(Read<int>("Id"), Read<string>("Name"), Read<int>("Health"), Read<int>("Mana"), Read<float>("PositionX"),
                        Read<float>("PositionY"), Read<float>("PositionZ"), Read<float>("Rotation"));
                }
                throw new ArgumentException("Cannot create PlayerObj in PlayerLoadResponsePacket, while succes status is false. ");
            } }

        public PlayerLoadResponsePacket(bool succes) : base(typeof(PlayerLoadResponsePacket))
        {
            Write<int>("Succes", succes ? 1 : 0);
        }

        public PlayerLoadResponsePacket(bool succes, Player player) : base(typeof(PlayerLoadResponsePacket))
        {
            Write<int>("Succes", succes ? 1 : 0);
            Write("Id", player.pVid);
            Write("Name", player.pName);
            Write("Health", player.pHealth);
            Write("Mana", player.pMana);
            Write("PositionX", player.pPositionX);
            Write("PositionY", player.pPositionY);
            Write("PositionZ",player.pPositionZ);
            Write("Rotation",player.pRotation);
        }


        public PlayerLoadResponsePacket(Packet packet) : base(packet) { }

        public PlayerLoadResponsePacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
