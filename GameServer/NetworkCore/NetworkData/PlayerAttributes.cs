/*using MemoryPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkData
{
    [MemoryPackable]
    public partial class PlayerAttributes :IGamePacket
    {
        public int CharacterVId { get; set; }
        public string Name { get; set; }
        public float CurrentHealth { get; set; }
        public float MaxHealth { get; set; }
        public float CurrentMana { get; set; }
        public float MaxMana { get; set; }
        public float MoveSpeed { get; set; }
        public float AttackSpeed { get; set; }
    }

    [MemoryPackable(GenerateType.Collection)]
    public partial class AttributesCollection : List<PlayerAttributes> { }
}
*/