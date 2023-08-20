using NetworkCore.NetworkMessage;
using NetworkCore.NetworkData;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    // That packet is dynamic in my sense. Values in here can be null, so
    // we can decide which values to store in that packet. This practice
    // can be helpfull if we dont need to send values, that doesn't change.
    public class CharacterStatePacket : Packet
    {
        // Player VId cannot be null. See the description of this parameter in the Player class.
        public int CharacterVId { get { return Read<int>("CharacterVId"); } }
        
        public string? Name
        {
            get
            {
                if (TryRead("Name", out string name))
                    return name; 
                else return null;
            }
        }

        public int? Health
        {
            get
            {
                if (TryRead("Health", out int health))
                    return health; 
                else return null;
            }
        }

        public int? Mana
        {
            get
            {
                if (TryRead("Mana", out int mana))
                    return mana; 
                else return null;
            }
        }

        public float? PosX { get {
                if (TryRead("PositionX", out float posX))
                    return posX; 
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

        // deleted, we cannot set unique Id in application
        /* public void SetId(int id)
        {
            Write("Id", id);
        }*/

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

        /*public void Init(int id, string name, int health, int mana, float posX, float posY, float posZ, float rot)
        {
            Write("Id", id);
            Write("Name", name);
            Write("Health", health);
            Write("Mana", mana);
            Write("PositionX", posX);
            Write("PositionY", posY);
            Write("PositionZ", posZ);
            Write("Rotation", rot);
        }*/

        /*public void Init(Player player)
        {
            Write("PlayerVid", player.pVid);
            Write("Name", player.pName);
            Write("Health", player.pHealth);
            Write("Mana", player.pMana);
            Write("PositionX", player.pPositionX);
            Write("PositionY", player.pPositionY);
            Write("PositionZ", player.pPositionZ);
            Write("Rotation", player.pRotation);
        } */

        // Be 100% sure this is the correct unique identificator of player.
        public CharacterStatePacket(int playerVId) : base (typeof(CharacterStatePacket))
        {
            Write("CharacterVId", playerVId);
        }

        public CharacterStatePacket(Character player) : base(typeof(CharacterStatePacket))
        {
            Write("CharacterVId", player.Vid);
            Write("Name", player.Name);
            Write("Health", player.Health);
            Write("Mana", player.Mana);
            Write("PositionX", player.PositionX);
            Write("PositionY", player.PositionY);
            Write("PositionZ", player.PositionZ);
            Write("Rotation", player.Rotation);
        }

        public CharacterStatePacket(Packet packet) : base(packet) { }

        public CharacterStatePacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
           // return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
                //$"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
