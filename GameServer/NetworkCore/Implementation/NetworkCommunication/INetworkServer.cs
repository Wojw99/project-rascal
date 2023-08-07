// not yet implemented

using NetworkCore.NetworkConfig;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkCore.NetworkCommunication
{
    public interface INetworkServer
    {
        /*event EventHandler<PeerConnectedEventArgs> PeerConnected;
        event EventHandler<PeerDisconnectedEventArgs> PeerDisconnected;
        event EventHandler<MessageReceivedEventArgs> MessageReceived;*/
        bool AllowPhysicalClients { get; }
        int MaxClients { get; }
        string PublicIpAdress { get; }
        int? TcpPort { get; }
        int? UdpPort { get; }
        Guid? ServerId { get; }
        ServerType _ServerType { get; }
        string ServerName { get; }
        bool IsRunning { get; }
        void Start();
        void Stop();
        void InitFromFile(string configFileName);

    }
   
    /*public class PeerConnectedEventArgs : EventArgs
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

    }*/
}
