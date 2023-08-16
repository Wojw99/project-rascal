/*using NetworkCore.NetworkMessage;
using NetworkCore.NetworkData;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    public class GlobalPlayerPositionPacket : Packet
    {
        public List<PlayerPosition> PlayerPos { get { return Read<List<PlayerPosition>>("PlayerPosition"); } }

        public GlobalPlayerPositionPacket(List<int> playerId, List<int> posX, List<int> posY, List<int> posZ) : base(PacketType.packet_player_move)
        {
            Write("PlayerId", playerId);
            Write("posX", posX);
            Write("posY", posY);
            Write("posZ", posZ);
        }

        public GlobalPlayerPositionPacket(Packet packet) : base(packet) { }

        public GlobalPlayerPositionPacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return base.ToString() + $" posX = {PosX}, posY = {PosY}, posZ = {PosZ}";
        }
    }
}
*/