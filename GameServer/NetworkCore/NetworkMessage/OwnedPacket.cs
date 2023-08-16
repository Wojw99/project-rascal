using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage.old;

namespace NetworkCore.NetworkMessage
{
    public struct OwnedPacket
    {
        public IPeer Peer; // storing the reference to Peer
        public Packet PeerPacket; 
    }
}
