/*using NetworkCore.NetworkMessage;
using NetworkCore.NetworkMessage.old;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkCore.NetworkCommunication
{
    public abstract class NetworkServer : NetworkBase
    {
        protected bool AllowPhysicalClients { get; }

        protected int MaxClients { get; }

        protected Guid? ServerId { get; }

        protected ServerType _ServerType { get; }

        protected ServerProtocolType _ServerProtocolType { get; }

        protected string ServerName { get; }

        protected Socket? TcpSocket { get; }

        // protected Socket? UdpSocket { get; }

        protected NetworkServer(bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType,
            UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval,
            int? tcpPort = null, int? udpPort = null)
            : base(maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval)
        {
            AllowPhysicalClients = allowPhysicalClients;
            MaxClients = maxClients;
            ServerId = Guid.NewGuid();
            ServerName = serverName;
            _ServerType = serverType;



            //_PacketHandlerManager = new PacketHandlerManager();

            if (tcpPort.HasValue)
            {
                TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                TcpSocket.Bind(new IPEndPoint(IPAddress.Parse(publicIpAdress), (int)tcpPort));
                _ServerProtocolType = ServerProtocolType.protocol_tcp;
            }

            if (udpPort.HasValue)
            {
                UdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
                UdpSocket.Bind(new IPEndPoint(IPAddress.Parse(publicIpAdress), (int)udpPort));
                _ServerProtocolType = ServerProtocolType.protocol_udp;
            }

            if (tcpPort.HasValue && udpPort.HasValue)
                _ServerProtocolType = ServerProtocolType.protocol_bothTcpUdp;

            //qPeers = new ConcurrentDictionary<Guid, IPeer>();
            qPacketsIn = new ConcurrentQueue<OwnedPacket>();
            qPacketsOut = new ConcurrentQueue<OwnedPacket>();

        }

        protected NetworkServer(string configFileName)
        {
            // read from file and initialize attributes.
        }

        public void InitFromFile(string configFileName)
        {
            // read from file and initialize attributes.
        }

        *//*public void SendPacketToAllClients(Packet packet)
        {
            foreach(var peer in qPeers)
            {
                if(peer.Value.IsConnected)
                {
                    peer.Value.SendPacket(packet);
                }
                else
                {
                    if (qPeers.TryRemove(peer.Value.Id, out IPeer removedPeer))
                    {
                        removedPeer.Disconnect();
                        OnClientDisconnect(removedPeer);
                    }
                }
            }
        }

        public void SendPacketToClient(Packet packet, Guid peerId)
        {
            if(qPeers.TryGetValue(peerId, out IPeer peer))
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
                        OnClientDisconnect(removedPeer);
                    }
                }
            } 
        }*//*

        public async Task Start()
        {
            if (TcpSocket != null && UdpSocket != null)
            {
                TcpSocket.Listen(10);
                UdpSocket.Listen(10);
                await Console.Out.WriteLineAsync($"Server, with GUID: {ServerId}, Name: {ServerName} started on TCP port, and UDP port.");
            }
            *//*else if (UdpListenerSocket != null && TcpListenerSocket == null)
            {
                UdpListenerSocket.Listen(10);
                await Console.Out.WriteLineAsync($"Server, with GUID: {ServerId}, Name: {ServerName} started on UDP port");
            }*//*
            else if (TcpSocket != null && UdpSocket == null)
            {
                TcpSocket.Listen(10);
                await Console.Out.WriteLineAsync($"Server, with GUID: {ServerId}, Name: {ServerName} started on TCP port");
            }
            else
            {
                throw new Exception("Cannot start server.");  // throw exception maybe
            }

            IsRunning = true;

            Task handleWaitForTcpConnection = Task.Run(async () => await WaitForTcpClientConnection());
            //Task handleWaitForUdpConnection = Task.Run(async () => await WaitForUdpClientConnection());
            //Task handleUpdate = Task.Run(async () => await Update(100, TimeSpan.FromMilliseconds(100)));
            *//*while (IsRunning)
            {
                //await WaitForClientConnection();
                // main loop on main thread
                Console.WriteLine("Server running...");
                Thread.Sleep(10000);
            }*//*
        }

        public async Task Stop()
        {
            IsRunning = false;

            TcpSocket?.Close(); // instead of: if (TcpListenerSocket != null) {TcpListenerSocket.Close(); }

            UdpSocket?.Close();
        }

        public override async Task SendOutgoingPacket(OwnedPacket receiver)
        {
            byte[] dataToSend = receiver.PeerPacket.SerializePacket();
            await receiver.Peer.PeerSocket.SendAsync(new ArraySegment<byte>(dataToSend), SocketFlags.None);
            await Console.Out.WriteLineAsync($"[SEND] packed with type: {receiver.PeerPacket.PacketType} from peer with Guid: {receiver.Peer.Id}");
        }

        private async Task WaitForTcpClientConnection()
        {
            while (IsRunning)
            {
                if (TcpSocket != null)
                {
                    Socket tcpClientSocket = await TcpSocket.AcceptAsync();

                    await Console.Out.WriteLineAsync($"New TCP connection received, info: {tcpClientSocket.RemoteEndPoint}");

                    await OnNewConnection(tcpClientSocket, Guid.NewGuid(), Owner.server);
                }
                else
                {
                    throw new Exception("TcpListenerSocket was null while while waiting for new connections.");
                }
            }
        }

        private async Task WaitForUdpClientConnection()
        {
            while (IsRunning)
            {
                *//*if (UdpListenerSocket != null)
                {
                    while (UdpListenerSocket.Connected)
                    {
                        Socket udpClientSocket = await UdpListenerSocket.AcceptAsync();

                        await Console.Out.WriteLineAsync($"New UDP connection received, info: {udpClientSocket.RemoteEndPoint}");

                        UdpPeer peer = new UdpPeer(..jakies wlasciwosci..);

                        if(await OnPeerConnect(peer))
                        {
                            if (qPeers.TryAdd(Guid.NewGuid(), peer))
                            {
                                peer.ConnectToClient();
                                await Console.Out.WriteLineAsync("Connection approved.");
                            }
                        }
                        else
                        {
                            await Console.Out.WriteLineAsync("Connection denied.");
                        }
                    }
                }*//*
            }
        }


        protected abstract Task OnNewConnection(Socket clientSocket, Guid connId, Owner ownerType);

        protected abstract Task OnClientDisconnect(IPeer clientPeer);

        public override abstract Task OnPacketReceived(IPeer clientPeer, Packet packet);

        //protected abstract Task SendPacketToClient(Guid clientId, Packet packet);
        //protected abstract Task SendPacketToAllClients(Packet packet);
    }
}
*/