using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.NetClient.Attributes
{
    public class TransformData
    {
        public int characterVId;
        public Vector3 position;
        public Vector3 rotation;

        public TransformData(int characterVId)
        {
            this.characterVId = characterVId;
            this.position = Vector3.zero;
            this.rotation = Vector3.zero;
        }

        public TransformData(int characterVId, Vector3 position, Vector3 rotation)
        {
            this.characterVId = characterVId;
            this.position = position;
            this.rotation = rotation;
        }
    }
}
