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

        public Player PlayerObj { 
            get { 
                Player player = new Player();
                player.pId = Read<int>("Id");
                player.pName = Read<string>("Name");
                player.pHealth = Read<int>("Health");
                player.pMana = Read<int>("Mana");
                player.pPositionX = Read<float>("PositionX");
                player.pPositionY = Read<float>("PositionY");
                player.pPositionZ = Read<float>("PositionZ");
                player.pRotation = Read<float>("Rotation");
                return player; 
            } }

        public PlayerLoadResponsePacket(bool succes) : base(typeof(PlayerLoadResponsePacket))
        {
            Write<int>("Succes", succes ? 1 : 0);
        }

        public PlayerLoadResponsePacket(bool succes, Player player) : base(typeof(PlayerLoadResponsePacket))
        {
            Write<int>("Succes", succes ? 1 : 0);
            Write("Id", player.pId);
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
