// Move this class to ServerApplication project.

using NetworkCore.NetworkMessage;
using NetworkCore.NetworkUtility;
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

        public float CurrentHealth { get; set; }

        public float MaxHealth { get; set; }

        public float CurrentMana { get; set; }

        public float MaxMana { get; set; }

        public float PositionX { get; set; }

        public float PositionY { get; set; }

        public float PositionZ { get; set; }

        public float RotationX { get; set; }

        public float RotationY { get; set; }

        public float RotationZ { get; set; }

        public float MoveSpeed { get; set; }

        public float AttackSpeed { get; set; }
        public AdventurerState State { get; set; }

        // Server-side - I need to think about the default constructor. Maybe we want to load Player
        // from database at this point.
        // Client-side - this default constructor is needed, when creating client player object in main Client class.
        public Character()
        {
            Vid = -1; // 
            Name = string.Empty;
            CurrentHealth = 0;
            MaxHealth = 0;
            CurrentMana = 0;
            MaxMana = 0;
            PositionX = 0;
            PositionY = 0;
            PositionZ = 0;
            RotationX = 0;
            RotationY = 0;
            RotationZ = 0;
            MoveSpeed = 0;
            State = AdventurerState.Idle;
        }

        // Used on server side to create Player only from database data!
        public Character (int PlayerVid, string Name, float CurrentHealth, float MaxHealth, 
            float CurrentMana, float MaxMana, float PositionX, float PositionY, float PositionZ, 
            float RotationX, float RotationY, float RotationZ, float MoveSpeed, float AttackSpeed,
            AdventurerState State)
        {
            this.Vid = PlayerVid;
            this.Name = Name;
            this.CurrentHealth = CurrentHealth;
            this.MaxHealth = MaxHealth;
            this.CurrentMana = CurrentMana;
            this.MaxMana = MaxMana;
            this.PositionX = PositionX;
            this.PositionY = PositionY;
            this.PositionZ = PositionZ;
            this.RotationX = RotationX;
            this.RotationY = RotationY;
            this.RotationZ = RotationZ;
            this.MoveSpeed = MoveSpeed;
            this.AttackSpeed = AttackSpeed;
            this.State = State;
        }

        public async Task Show()
        {
            await Console.Out.WriteLineAsync($"Id = {Vid}");
            await Console.Out.WriteLineAsync($"Name = {Name}");
            await Console.Out.WriteLineAsync($"CurrentHealth = {CurrentHealth}");
            await Console.Out.WriteLineAsync($"MaxHealth = {MaxHealth}");
            await Console.Out.WriteLineAsync($"CurrentMana = {CurrentMana}");
            await Console.Out.WriteLineAsync($"MaxMana = {MaxMana}");
            await Console.Out.WriteLineAsync($"PositionX = {PositionX}");
            await Console.Out.WriteLineAsync($"PositionY = {PositionY}");
            await Console.Out.WriteLineAsync($"PositionZ = {PositionZ}");
            await Console.Out.WriteLineAsync($"RotationX = {RotationX}");
            await Console.Out.WriteLineAsync($"RotationY = {RotationY}");
            await Console.Out.WriteLineAsync($"RotationZ = {RotationZ}");
        }
    }
}
