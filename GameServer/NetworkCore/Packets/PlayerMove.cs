using NetworkCore.NetworkMessage;
using NetworkCore.NetworkData;
using NetworkCore.Packets.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;


namespace NetworkCore.Packets
{
    public class PlayerMovePacket : Packet
    {
        public PlayerPosition Position
        {
            get
            {
                int playerId = Read<int>("PlayerId");
                float posX = Read<float>("PosX");
                float posY = Read<float>("PosY");
                float posZ = Read<float>("PosZ");
                float rotation = Read<float>("Rotation");
                return new PlayerPosition(playerId, posX, posY, posZ, rotation);
            }
        }
       /* public float PosX { get { return Read<float>("posX"); } }

        public float PosY { get { return Read<float>("posY"); }  }

        public float PosZ { get { return Read<float>("posZ"); }  }*/

        public PlayerMovePacket(PlayerPosition position, Dictionary<string, (PlayerAttribute, bool)> ChangedVariables) : base(typeof(PlayerMovePacket))
        {
            foreach(var value in ChangedVariables)
            {
                if(value.Value.Item2) 
                    Write(value.Key, value.Value.Item1);

            }

           /* Write("PlayerId", position.PlayerId);
            Write("PosX", position.PosX);
            Write("PosY", position.PosY);
            Write("PosZ", position.PosZ);
            Write("Rotation", position.Rotation);*/
        }

        public PlayerMovePacket(Packet packet) : base(typeof(PlayerMovePacket)) {}

        public PlayerMovePacket(byte[] data ) : base(data) {}

        public override string ToString()
        {
            return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
                $"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
