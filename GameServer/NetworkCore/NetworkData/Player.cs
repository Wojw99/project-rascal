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


        public Player(int playerId)
        {
            pId = playerId;
            pName = string.Empty;
            pHealth = 0;
            pMana = 0;
            pPositionX = 0;
            pPositionY = 0;
            pPositionZ = 0;
            pRotation = 0;
        }

        public async Task Show()
        {
            await Console.Out.WriteLineAsync($"Id = {pId}");
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
