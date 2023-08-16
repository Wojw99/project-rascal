using NetworkCore.NetworkMessage;
using NetworkCore.Packets.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class PlayerStatePacket : Packet
    {
        public List<PlayerAttribute> PlayerAttributes { 
            get 
            {
                List<PlayerAttribute> attributes = new List<PlayerAttribute>();

              
                if(TryRead("PositionX", out float posX))
                    attributes.Add(new PositionX(posX));

                if (TryRead("PositionY", out float posY))
                    attributes.Add(new PositionY(posY));

                if (TryRead("PositionZ", out float posZ))
                    attributes.Add(new PositionZ(posZ));

                if (TryRead("Rotation", out float rot))
                    attributes.Add(new Rotation(rot));

                return attributes;
            
            } 
        }

        public PlayerStatePacket(List<PlayerAttribute> attributes) : base (typeof(PlayerStatePacket))
        {
            
            foreach(var attr in attributes)
            {
                if (attr is PositionX)
                {
                    Write("PositionX", (attr as PositionX).Value);
                }
                else if (attr is PositionY)
                {
                    Write("PositionY", (attr as PositionY).Value);
                }
                else if (attr is PositionZ)
                {
                    Write("PositionZ", (attr as PositionY).Value);
                }
                else if (attr is Rotation)
                {
                    Write("Rotation", (attr as PositionY).Value);
                }
            }
        }

        //public PlayerStatePacket(Packet packet) : base(packet) { }

        public PlayerStatePacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
           // return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
                //$"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
