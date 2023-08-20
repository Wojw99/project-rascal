using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using NetworkCore.NetworkData.stare;
using NetworkCore.NetworkData;

namespace NetworkCore.Packets
{
    // Values in that packet must be assigned. So we must overload constructor
    // with parameters.
    public class CharacterMovePacket : Packet
    {
        public int CharacterVid { get { return Read<int>("CharacterVId"); } }
        public float PosX { get { return Read<float>("PositionX"); } }
        public float PosY { get { return Read<float>("PositionY"); } }
        public float PosZ { get { return Read<float>("PositionZ"); } }
        public float Rot { get { return Read<float>("Rotation"); } }

        // There is one overload, because we want to always store all values.
        // And also player Virtual Id must be correct.
        public CharacterMovePacket(Character PlayerObj) : base(typeof(CharacterMovePacket))
        {
            Write("CharacterVId", PlayerObj.Vid);
            Write("PositionX", PlayerObj.PositionX);
            Write("PositionY", PlayerObj.PositionY);
            Write("PositionZ", PlayerObj.PositionZ);
            Write("Rotation", PlayerObj.Rotation);
        }

        public CharacterMovePacket(Packet packet) : base(packet) {}

        public CharacterMovePacket(byte[] data ) : base(data) {}

        public override string ToString()
        {
            return "";
            //return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
               // $"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
