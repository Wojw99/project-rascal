using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkCommunication;

namespace NetworkCore.NetworkMessage
{
    public struct OwnedPacket
    {
        public IPeer Peer; 
        public PacketBase PeerPacket; 
    }
}
