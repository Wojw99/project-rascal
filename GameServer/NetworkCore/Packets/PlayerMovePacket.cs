using NetworkCore.NetworkMessage;
using NetworkCore.Packets.Attributes;
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
    public class PlayerMovePacket : Packet
    {
        public int PlayerVid { get { return Read<int>("PlayerVid"); } }
        public float PosX { get { return Read<float>("PositionX"); } }
        public float PosY { get { return Read<float>("PositionY"); } }
        public float PosZ { get { return Read<float>("PositionZ"); } }
        public float Rot { get { return Read<float>("Rotation"); } }

        // There is one overload, because we want to always store all values.
        // And also player Virtual Id must be correct.
        public PlayerMovePacket(Player PlayerObj) : base(typeof(PlayerMovePacket))
        {
            Write("PlayerVid", PlayerObj.pVid);
            Write("PositionX", PlayerObj.pPositionX);
            Write("PositionY", PlayerObj.pPositionY);
            Write("PositionZ", PlayerObj.pPositionZ);
            Write("Rotation", PlayerObj.pRotation);
        }

        public PlayerMovePacket(Packet packet) : base(packet) {}

        public PlayerMovePacket(byte[] data ) : base(data) {}

        public override string ToString()
        {
            return "";
            //return base.ToString() + $"PlayerId = {Position.PlayerId}, PosX = {Position.PosX}, " +
               // $"PosY = {Position.PosY}, PosZ = {Position.PosZ}, Rotation = {Position.Rotation}";
        }
    }
}
