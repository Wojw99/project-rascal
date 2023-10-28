/*using MemoryPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkData
{
    [MemoryPackable]
    public partial class PlayerTransform : IGamePacket
    {
        public int CharacterVId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }
        public float test { get; private set; }
    }

    [MemoryPackable(GenerateType.Collection)]
    public partial class TransformCollection : List<PlayerTransform> { }
}
*/