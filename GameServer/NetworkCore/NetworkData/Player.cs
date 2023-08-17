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
        public int pId { get; set; }

        public string pName { get; set; }

        public int pHealth { get; set; }

        public int pMana { get; set; }

        public float pPositionX { get; set; }

        public float pPositionY { get; set; }

        public float pPositionZ { get; set; }

        public float pRotation { get; set; }


        public Player()
        {
            pId = 0;
            pName = string.Empty;
            pHealth = 0;
            pMana = 0;
            pPositionX = 0;
            pPositionY = 0;
            pPositionZ = 0;
            pRotation = 0;
        }

        public Player(PlayerStatePacket packet)
        {
            pName = packet.Name != null ? packet.Name : string.Empty;
            pHealth = packet.Health != null ? packet.Health.Value : 0;
            pMana = packet.Mana != null ? packet.Mana.Value : 0;
            pPositionX = packet.PosX != null ? packet.PosX.Value : 0; // if null assign 0
            pPositionY = packet.PosY != null ? packet.PosY.Value : 0;
            pPositionZ = packet.PosZ != null ? packet.PosZ.Value : 0;
            pRotation = packet.Rot != null ? packet.Rot.Value : 0;
        }

        public Player (int Id, string Name, int Health, int Mana, float PositionX, float PositionY, float PositionZ, float Rotation)
        {
            this.pId = Id;
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
            await Console.Out.WriteLineAsync($"Id = {pId}");
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
