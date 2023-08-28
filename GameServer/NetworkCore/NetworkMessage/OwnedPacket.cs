using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkMessage
{
    public struct OwnedPacket
    {
        public IPeer Peer; // storing the reference to Peer
        public PacketBase PeerPacket; 
    }
}
