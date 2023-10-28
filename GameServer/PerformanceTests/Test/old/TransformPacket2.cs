/*using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace PerformanceTests.Test.old
{
    public class TestPacket2
    {
        [Serialization(Type: SerializationType.type_Int32)]
        public int test { get; set; }

        public TestPacket2() { test = 125; }

    }

    public partial class TransformPacket2
    {
        [Serialization(Type: SerializationType.type_Int32)]
        public int CharacterVId { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float PosX { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float PosY { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float PosZ { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float RotX { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float RotY { get; set; }

        [Serialization(Type: SerializationType.type_float)]
        public float RotZ { get; set; }

        public TransformPacket2(int characterVId, float posX, float posY, float posZ,
            float rotX, float rotY, float rotZ)
        {
            CharacterVId = characterVId;
            PosX = posX;
            PosY = posY;
            PosZ = posZ;
            RotX = rotX;
            RotY = rotY;
            RotZ = rotZ;
        }

        public TransformPacket2()
        {
            CharacterVId = 0;
            PosX = 0f;
            PosY = 0f;
            PosZ = 0f;
            RotX = 0f;
            RotY = 0f;
            RotZ = 0f;
        }
    }
}
*/