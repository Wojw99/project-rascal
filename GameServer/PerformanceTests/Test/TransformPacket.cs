using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace PerformanceTests.Test
{
    public class TestPacket
    {
        public int test { get; set; }

        //public TransformPacket transform { get; set; }

        public List<TransformPacket> packets { get; set; }

        public TestPacket() { 
            test = 125; 
            packets = new List<TransformPacket>();
            packets.Add(new TransformPacket());
            //transform = new TransformPacket();
            //packets = new List<TransformPacket>();
        }
    }

    public class SimpleTestPacket
    {
        public int test { get; set; }

        public SimpleTestPacket()
        {
            test = 125;
        }
    }

    public partial class TransformPacket 
    {
        public int CharacterVId { get; set; }
        public float PosX { get; set; } 
        public float PosY { get; set; } 
        public float PosZ { get; set; } 
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }


        public TransformPacket(int characterVId, float posX, float posY, float posZ, 
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

        public TransformPacket() {
            CharacterVId = 1;
            PosX = 5f;
            PosY = 5f;
            PosZ = 5f;
            RotX = 5f;
            RotY = 5f;
            RotZ = 5f;
        }
    }
}
