using NetworkCore.NetworkMessage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace NetworkCore.NetworkCommunication.stare
{
    public interface INetworkBase
    {
        //public Socket? TcpSocket { get; }

        //public Socket? UdpSocket { get; }

        // public ConcurrentDictionary<Guid, IPeer> qPeers { get; }

        public ConcurrentQueue<OwnedPacket> qPacketsIn { get; }

        public ConcurrentQueue<OwnedPacket> qPacketsOut { get; }

        //public PacketHandlerManager _PacketHandlerManager { get; }

        public bool IsRunning { get; }
    }
}
