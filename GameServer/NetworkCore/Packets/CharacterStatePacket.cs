using NetworkCore.NetworkData;
using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage.old;
using NetworkCore.NetworkMessage;
using NetworkCore.NetworkData.stare;
using System.Numerics;

namespace NetworkCore.Packets
{
    // That packet is dynamic in my sense. Values in here can be null, so
    // we can decide which values to store in that packet. This practice
    // can be helpfull if we dont need to send values, that doesn't change.
    public class CharacterStatePacket : PacketBase
    {
        // Player VId cannot be null. See the description of this parameter in the Player class.

        [Serialization(Type: SerializationType.type_Int32)]
        public int CharacterVId { get; private set; }

        [Serialization(Type: SerializationType.type_string)]
        public string? Name { get; set; }

        [Serialization(Type: SerializationType.type_Int32)]
        public int? Health { get; set; }

        [Serialization(Type: SerializationType.type_Int32)]
        public int? Mana { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float? PosX { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float? PosY { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float? PosZ { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float? Rot { get; set; }

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
        public CharacterStatePacket(int characterVId) : base(PacketType.CHARACTER_STATE_PACKET)
        {
            CharacterVId = characterVId;
        }

        public CharacterStatePacket(Character player) : base(PacketType.CHARACTER_STATE_PACKET)
        {
            CharacterVId = player.Vid;
            Name = player.Name;
            Health = player.Health;
            Mana = player.Mana;
            PosX =  player.PositionX;
            PosY = player.PositionY;
            PosZ = player.PositionZ;
            Rot = player.Rotation;
        }

        //public CharacterStatePacket(PacketBase packet) : base(packet) { }

        public CharacterStatePacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
            // return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
            //$"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
