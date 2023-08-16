using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkData.stare
{
    public struct EnemyPosition : IPosition
    {
        public int PlayerId { get; set; }

        public float PosX { get; set; }

        public float PosY { get; set; }

        public float PosZ { get; set; }

        public float Rotation { get; set; }

        public EnemyPosition(int playerId, float posX, float posY, float posZ, float rotation)
        {
            PlayerId = playerId;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
            Rotation = rotation;
        }
    }
}
