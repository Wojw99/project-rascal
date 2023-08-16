using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkData
{
    public interface IPosition
    {
        public int PlayerId { get; set; }

        public float PosX { get; set; }

        public float PosY { get; set; }

        public float PosZ { get; set; }

        public float Rotation { get; set; }
    }
}
