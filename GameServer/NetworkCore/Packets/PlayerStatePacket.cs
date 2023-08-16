using NetworkCore.NetworkMessage;
using NetworkCore.Packets.Attributes;
using NetworkCore.NetworkData;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class PlayerStatePacket : Packet
    {
        public int? Id
        {
            get
            {
                if (TryRead("Id", out int id))
                    return id; //new PositionX(posX);
                else return null;
            }
        }

        public string? Name
        {
            get
            {
                if (TryRead("Name", out string name))
                    return name; //new PositionX(posX);
                else return null;
            }
        }

        public int? Health
        {
            get
            {
                if (TryRead("Health", out int health))
                    return health; //new PositionX(posX);
                else return null;
            }
        }

        public int? Mana
        {
            get
            {
                if (TryRead("Mana", out int mana))
                    return mana; //new PositionX(posX);
                else return null;
            }
        }

        public float? PosX { get {
                if (TryRead("PositionX", out float posX))
                    return posX; //new PositionX(posX);
                else return null;
        }}

        public float? PosY { get { 
            if(TryRead("PositionY", out float posY))
                return posY;
            else return null;
        }}

        public float? PosZ { get {
            if (TryRead("PositionZ", out float posZ))
                return posZ;
            else return null;
        }}

        public float? Rot { get { 
            if(TryRead("Rotation", out float rotX)) 
                return rotX; 
            else return null;
        }}

        public void SetId(int id)
        {
            Write("Id", id);
        }

        public void SetName(string name)
        {
            Write("Name", name);
        }

        public void SetHealth(int health)
        {
            Write("Health", health);
        }

        public void SetMana(int mana)
        {
            Write("Mana", mana);
        }

        public void SetPositionX(float posX)
        {
            Write("PositionX", posX);
        }

        public void SetPositionY(float posY)
        {
            Write("PositionY", posY);
        }

        public void SetPositionZ(float posZ)
        {
            Write("PositionZ", posZ);
        }

        public void SetRotation(float rot)
        {
            Write("Rotation", rot);
        }

        public void Init(int id, string name, int health, int mana, float posX, float posY, float posZ, float rot)
        {
            Write("Id", id);
            Write("Name", name);
            Write("Health", health);
            Write("Mana", mana);
            Write("PositionX", posX);
            Write("PositionY", posY);
            Write("PositionZ", posZ);
            Write("Rotation", rot);
        }

        public void Init(Player player)
        {
            Write("Id", player.pId);
            Write("Name", player.pName);
            Write("Health", player.pHealth);
            Write("Mana", player.pMana);
            Write("PositionX", player.pPositionX);
            Write("PositionY", player.pPositionY);
            Write("PositionZ", player.pPositionZ);
            Write("Rotation", player.pRotation);
        }

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

        public PlayerStatePacket() : base (typeof(PlayerStatePacket))
        {
            
           /* foreach(var attr in attributes)
            {
                if (attr is PositionX posX)
                {
                    Write("PositionY", (posX).Value);
                    //Write("PositionX", (attr as PositionX).Value);
                }
                else if (attr is PositionY posY)
                {
                    Write("PositionY", (posY).Value);
                }
                else if (attr is PositionZ posZ)
                {
                    Write("PositionY", (posZ).Value);
                }
                else if (attr is Rotation rot)
                {
                    Write("PositionY", (rot).Value);
                }
            }*/
        }

        public PlayerStatePacket(Packet packet) : base(packet) { }

        public PlayerStatePacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
           // return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
                //$"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
