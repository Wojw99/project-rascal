using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.Packets;

namespace Assets.Code.Scripts.NetClient.Attributes
{
    public class TransformData
    {
        public int CharacterVId;
        public Vector3 Position;
        public Vector3 Rotation;

        public TransformData(int characterVId)
        {
            this.CharacterVId = characterVId;
            this.Position = Vector3.zero;
            this.Rotation = Vector3.zero;
        }

        public TransformData(int characterVId, Vector3 position, Vector3 rotation)
        {
            this.CharacterVId = characterVId;
            this.Position = position;
            this.Rotation = rotation;
        }

        public TransformData(TransformPacket transformPacket)
        {
            this.CharacterVId = transformPacket.CharacterVId;
            this.Position = new Vector3(transformPacket.PosX, transformPacket.PosY, transformPacket.PosZ);
            this.Rotation = new Vector3(transformPacket.RotX, transformPacket.RotY, transformPacket.RotZ);
        }
    }
}
