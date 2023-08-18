using NetworkCore.Packets;
using NetworkCore.Packets.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkData
{
    public class Player
    {
        // Vid (Virtual ID) has meaning in the form of a unique id.
        // In the future this id must be encrypted while transferring packets with it.
        // This id is assigned to player when he create his character.
        // I think the name of this class shouldn't be called a character?
        public int pVid { get; } // only get - we want to load that id from database.

        public string pName { get; set; }

        public int pHealth { get; set; }

        public int pMana { get; set; }

        public float pPositionX { get; set; }

        public float pPositionY { get; set; }

        public float pPositionZ { get; set; }

        public float pRotation { get; set; }


        // Server-side - I need to think about the default constructor. Maybe we want to load Player
        // from database at this point.
        // Client-side - this default constructor is needed, when creating client player object in main Client class.
        public Player()
        {
            pVid = -1; // 
            pName = string.Empty;
            pHealth = 0;
            pMana = 0;
            pPositionX = 0;
            pPositionY = 0;
            pPositionZ = 0;
            pRotation = 0;
        }

        // Used on client side - when only new player state received.
        // Note that when Player object exists - we want to only change his state.
        public Player(PlayerStatePacket packet)
        {
            pVid = packet.PlayerVId; // Vid in packet cannot be null.
            pName = packet.Name != null ? packet.Name : string.Empty;
            pHealth = packet.Health != null ? packet.Health.Value : 0;
            pMana = packet.Mana != null ? packet.Mana.Value : 0;
            pPositionX = packet.PosX != null ? packet.PosX.Value : 0; // if null assign 0
            pPositionY = packet.PosY != null ? packet.PosY.Value : 0;
            pPositionZ = packet.PosZ != null ? packet.PosZ.Value : 0;
            pRotation = packet.Rot != null ? packet.Rot.Value : 0;
        }

        // Used on server side to create Player only from database data!
        public Player (int PlayerVid, string Name, int Health, int Mana, float PositionX, float PositionY, float PositionZ, float Rotation)
        {
            this.pVid = PlayerVid;
            this.pName = Name;
            this.pHealth = Health;
            this.pMana = Mana;
            this.pPositionX = PositionX;
            this.pPositionY = PositionY;
            this.pPositionZ = PositionZ;
            this.pRotation = Rotation;
        }

        public async Task Show()
        {
            await Console.Out.WriteLineAsync($"Id = {pVid}");
            await Console.Out.WriteLineAsync($"Name = {pName}");
            await Console.Out.WriteLineAsync($"Health = {pHealth}");
            await Console.Out.WriteLineAsync($"Mana = {pMana}");
            await Console.Out.WriteLineAsync($"PositionX = {pPositionX}");
            await Console.Out.WriteLineAsync($"PositionY = {pPositionY}");
            await Console.Out.WriteLineAsync($"PositionZ = {pPositionZ}");
            await Console.Out.WriteLineAsync($"Rotation = {pRotation}");
        }
    }
}
