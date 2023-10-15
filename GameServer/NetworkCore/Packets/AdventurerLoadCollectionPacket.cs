using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkData;

namespace NetworkCore.Packets
{
    // This packet is used for sending state of all players to player, which has logged in.
    public class AdventurerLoadCollectionPacket : PacketBase
    {
        [Serialization(Type: SerializationType.type_subPacketList)]
        public List<AdventurerLoadPacket> PacketCollection { get; set; }

        public AdventurerLoadCollectionPacket() : base(PacketType.ADVENTURER_LOAD_COLLECTION_PACKET, false)
        {
            PacketCollection = new List<AdventurerLoadPacket>();
        }

        public AdventurerLoadCollectionPacket(byte[] data) : base(data) { }
        public override string GetInfo()
        {
            return "ADVENTURER LOAD COLLECTION PACKET, " + base.GetInfo();
        }
    }
}
