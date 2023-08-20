using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkData
{
    public class Character
    {
        // Vid (Virtual ID) has meaning in the form of a unique id.
        // In the future this id must be encrypted while transferring packets with it.
        // This id is assigned to player when he create his character.
        // I think the name of this class shouldn't be called a character?
        public int Vid { get; } // only get - we want to load that id from database.

        public string Name { get; set; }

        public int Health { get; set; }

        public int Mana { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float PositionZ { get; set; }

        public float Rotation { get; set; }


        // Server-side - I need to think about the default constructor. Maybe we want to load Player
        // from database at this point.
        // Client-side - this default constructor is needed, when creating client player object in main Client class.
        public Character()
        {
            Vid = -1; // 
            Name = string.Empty;
            Health = 0;
            Mana = 0;
            PositionX = 0;
            PositionY = 0;
            PositionZ = 0;
            Rotation = 0;
        }

        // Used on client side - when only new player state received.
        // Note that when Player object exists - we want to only change his state.
        public Character(CharacterStatePacket packet)
        {
            Vid = packet.CharacterVId; // Vid in packet cannot be null.
            Name = packet.Name != null ? packet.Name : string.Empty;
            Health = packet.Health != null ? packet.Health.Value : 0;
            Mana = packet.Mana != null ? packet.Mana.Value : 0;
            PositionX = packet.PosX != null ? packet.PosX.Value : 0; // if null assign 0
            PositionY = packet.PosY != null ? packet.PosY.Value : 0;
            PositionZ = packet.PosZ != null ? packet.PosZ.Value : 0;
            Rotation = packet.Rot != null ? packet.Rot.Value : 0;
        }

        // Used on server side to create Player only from database data!
        public Character (int PlayerVid, string Name, int Health, int Mana, float PositionX, float PositionY, float PositionZ, float Rotation)
        {
            this.Vid = PlayerVid;
            this.Name = Name;
            this.Health = Health;
            this.Mana = Mana;
            this.PositionX = PositionX;
            this.PositionY = PositionY;
            this.PositionZ = PositionZ;
            this.Rotation = Rotation;
        }

        public async Task Show()
        {
            await Console.Out.WriteLineAsync($"Id = {Vid}");
            await Console.Out.WriteLineAsync($"Name = {Name}");
            await Console.Out.WriteLineAsync($"Health = {Health}");
            await Console.Out.WriteLineAsync($"Mana = {Mana}");
            await Console.Out.WriteLineAsync($"PositionX = {PositionX}");
            await Console.Out.WriteLineAsync($"PositionY = {PositionY}");
            await Console.Out.WriteLineAsync($"PositionZ = {PositionZ}");
            await Console.Out.WriteLineAsync($"Rotation = {Rotation}");
        }
    }
}
