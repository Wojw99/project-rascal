using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkData
{
    public class Enemy
    {
        public int Vid { get; } // only get - we want to load that id from database.

        public string Name { get; set; }

        public int Health { get; set; }

        public int Mana { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float PositionZ { get; set; }

        public float Rotation { get; set; }

        public Enemy()
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

        // Used on server side to create Player only from database data!
        public Enemy(int PlayerVid, string Name, int Health, int Mana, float PositionX, float PositionY, float PositionZ, float Rotation)
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
