using NetworkCore.NetworkMessage;
using NetworkCore.NetworkMessage.old;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkCommunication
{
    public abstract class NetworkClient : INetworkBase
    {

        public ConcurrentDictionary<Guid, IPeer> qPeers { get; }

        public ConcurrentQueue<OwnedPacket> qPacketsIn { get; }

        public ConcurrentQueue<OwnedPacket> qPacketsOut { get; }

        public bool IsRunning { get; private set; }

        public NetworkClient ()
        {
            qPeers = new ConcurrentDictionary<Guid, IPeer>();
            qPacketsIn = new ConcurrentQueue<OwnedPacket>();
            qPacketsOut = new ConcurrentQueue<OwnedPacket>();
        }

        public async Task <bool> ConnectTcpServer(string serverIpAddress, int serverTcpPort)
        {
            Socket ServerTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await ServerTcpSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverIpAddress), (int)serverTcpPort));

            if(ServerTcpSocket.Connected)
            {
                Guid newConnectionId = Guid.NewGuid();
                TcpPeer serverPeer = new TcpPeer(this, ServerTcpSocket, newConnectionId, Owner.client);

                if (await OnServerConnect(serverPeer))
                {
                    if (qPeers.TryAdd(newConnectionId, serverPeer))
                    {
                        //Task handleConnectToClient = Task.Run(async () => await peer.ConnectToClient());
                        serverPeer.ConnectToServer();
                        //await Console.Out.WriteLineAsync("Connection approved.");
                    }

                    return true;
                }
                else
                {
                    //await Console.Out.WriteLineAsync("Connection denied.");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ConnectUdpServer(string serverIpAddress, int serverUdpPort)
        {
            Socket ServerUdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
            ServerUdpSocket.Connect(new IPEndPoint(IPAddress.Parse(serverIpAddress), (int)serverUdpPort));

            if (ServerUdpSocket.Connected)
            {
                //UdpPeer serverPeer = new TcpPeer(this, ServerUdpSocket, Guid.NewGuid(), Owner.client);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task Start()
        {

            IsRunning = true;

            //Task handleWaitForTcpConnection = Task.Run(async () => await WaitForTcpClientConnection());
            //Task handleWaitForUdpConnection = Task.Run(async () => await WaitForUdpClientConnection());
            Task handleUpdate = Task.Run(async () => await Update(1000, TimeSpan.FromMilliseconds(100)));
        }

        public async Task Stop()
        {
            IsRunning = false;
        }

        public async Task Update(UInt32 maxPacketCount = 100, TimeSpan packetProcessInterval = default)
        {
            while (IsRunning)
            {
                UInt32 packetCount = 0;

                while (packetCount < maxPacketCount && !qPacketsIn.IsEmpty)
                {
                    if (qPacketsIn.TryDequeue(out var ownedPacket))
                    {
                        Task handleOnPacketReceived = Task.Run(async () => await OnPacketReceived(ownedPacket.Peer, ownedPacket.PeerPacket));
                        packetCount++;
                    }
                }

                if (packetProcessInterval != default)
                {
                    await Task.Delay(packetProcessInterval);
                }
            }
        }

        public void SendPacketToAllServers(Packet packet)
        {
            foreach (var peer in qPeers)
            {
                if (peer.Value.IsConnected)
                {
                    peer.Value.SendPacket(packet);
                }
                else
                {
                    if (qPeers.TryRemove(peer.Value.Id, out IPeer removedPeer))
                    {
                        removedPeer.Disconnect();
                        OnServerDisconnect(removedPeer);
                    }
                }
            }
        }

        public void SendPacketToServer(Packet packet, Guid peerId)
        {
            if (qPeers.TryGetValue(peerId, out IPeer peer))
            {
                if (peer.IsConnected)
                {
                    peer.SendPacket(packet);
                }
                else
                {
                    if (qPeers.TryRemove(peer.Id, out IPeer removedPeer))
                    {
                        removedPeer.Disconnect();
                        OnServerDisconnect(removedPeer);
                    }
                }
            }
        }

        public abstract Task<bool> OnServerConnect(IPeer serverPeer);
        public abstract Task OnServerDisconnect(IPeer serverPeer);
        public abstract Task OnPacketReceived(IPeer serverPeer, Packet packet);
    }
}
