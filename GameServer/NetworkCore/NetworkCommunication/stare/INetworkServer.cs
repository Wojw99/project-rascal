/*// not yet implemented

using NetworkCore.NetworkConfig;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using NetworkCore.NetworkMessage;
using System.Threading.Tasks;

namespace NetworkCore.NetworkCommunication
{
    public interface INetworkServer
    {
        *//*event EventHandler<PeerConnectedEventArgs> PeerConnected;
        event EventHandler<PeerDisconnectedEventArgs> PeerDisconnected;
        event EventHandler<MessageReceivedEventArgs> MessageReceived;*//*
        bool AllowPhysicalClients { get; }

        int MaxClients { get; }

        //string PublicIpAdress { get; }

        //int? TcpPort { get; }

        //int? UdpPort { get; }

        Guid? ServerId { get; }

        ServerType _ServerType { get; }

        string ServerName { get; }

        bool IsRunning { get; }

        Socket? TcpListenerSocket { get; }
        Socket? UdpListenerSocket { get; }

        ConcurrentQueue<IPeer> qPeers{ get; }

        ConcurrentQueue<Packet> qPacketsIn { get; }

        ConcurrentQueue<Packet> qPacketsOut { get; }

        PacketHandlerManager _PacketHandlerManager { get; }

        //UInt32 IdCounter { get; } 

        Task Start();

        Task Stop();

        Task Update();

        Task WaitForClientConnection();

        Task SendPacketToClient(Packet packet, IPeer client);

        Task SendPacketToAllClients(Packet packet, IPeer IgnoreClient);

        void InitFromFile(string configFileName);

        //virtual bool OnPeerConnect(IPeer peer);
        //virtual void OnPeerDisconnect(IPeer peer);
        //virtual void OnPacketReceived(IPeer peer, Packet);

    }
   
    *//*public class PeerConnectedEventArgs : EventArgs
    {
        public IPeer Peer { get; set; }
    }

    public class PeerDisconnectedEventArgs : EventArgs
    {
        public IPeer Peer { get; set; }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public IPeer Peer { get; set; } // IPeer zamiast ClientId
        public byte[] Data { get; set; }

    }*//*
}
*/