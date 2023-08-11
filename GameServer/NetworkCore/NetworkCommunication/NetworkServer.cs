using NetworkCore.NetworkMessage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkCore.NetworkCommunication
{
    public abstract class NetworkServer : INetworkBase
    {
        public bool AllowPhysicalClients { get; }

        public int MaxClients { get; }

        public Guid? ServerId { get; }

        public ServerType _ServerType { get; }

        public ServerProtocolType _ServerProtocolType { get; }

        public string ServerName { get; }

        public Socket? TcpSocket { get; }

        public Socket? UdpSocket { get; }

        public bool IsRunning { get; private set; }

        public ConcurrentDictionary<Guid, IPeer> qPeers { get; }

        public ConcurrentQueue<OwnedPacket> qPacketsIn { get; }

        public ConcurrentQueue<OwnedPacket> qPacketsOut { get; }

        //public PacketHandlerManager _PacketHandlerManager { get; private set; }

        protected NetworkServer (bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType, int? tcpPort = null, int? udpPort = null)
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

            qPeers = new ConcurrentDictionary<Guid, IPeer>();
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

        public void SendPacketToAllClients(Packet packet)
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
        }

        public async Task Start()
        {
            if (TcpSocket != null && UdpSocket != null)
            {
                TcpSocket.Listen(10);
                UdpSocket.Listen(10);
                await Console.Out.WriteLineAsync($"Server, with GUID: {ServerId}, Name: {ServerName} started on TCP port, and UDP port.");
            }
            /*else if (UdpListenerSocket != null && TcpListenerSocket == null)
            {
                UdpListenerSocket.Listen(10);
                await Console.Out.WriteLineAsync($"Server, with GUID: {ServerId}, Name: {ServerName} started on UDP port");
            }*/
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
            Task handleWaitForUdpConnection = Task.Run(async () => await WaitForUdpClientConnection());
            Task handleUpdate = Task.Run(async () => await Update());
            /*while (IsRunning)
            {
                //await WaitForClientConnection();
                // main loop on main thread
                Console.WriteLine("Server running...");
                Thread.Sleep(10000);
            }*/
        }

        public async Task Stop()
        {
            IsRunning = false;

            TcpSocket?.Close(); // instead of: if (TcpListenerSocket != null) {TcpListenerSocket.Close(); }

            UdpSocket?.Close();
        }

        public async Task Update(UInt32 maxPacketCount = 100)
        {
            while(IsRunning)
            {
                UInt32 packetCount = 0;

                while(packetCount < maxPacketCount && !qPacketsIn.IsEmpty)
                {
                    if(qPacketsIn.TryPeek(out var ownedPacket))
                    {
                        Task handleOnPacketReceived = Task.Run(async () => await OnPacketReceived(ownedPacket.Peer, ownedPacket.Packet));
                        packetCount++;
                    }

                }
            }
        }

        private async Task WaitForTcpClientConnection()
        {
            while(IsRunning)
            {
                if (TcpSocket != null)
                {
                    Socket tcpClientSocket = await TcpSocket.AcceptAsync();

                    await Console.Out.WriteLineAsync($"New TCP connection received, info: {tcpClientSocket.RemoteEndPoint}");

                    Guid newConnectionId = Guid.NewGuid();

                    /// ref Socket TcpListenerReference = ref TcpListenerSocket;
                    //Socket TempTcpListenerSocket = TcpListenerSocket;
                    //ConcurrentQueue<OwnedPacket> TempPacketsIn = qPacketsIn;
                    
                    TcpPeer peer = new TcpPeer(this, tcpClientSocket, newConnectionId, Owner.server);

                    if (await OnClientConnect(peer))
                    {
                        if (qPeers.TryAdd(newConnectionId, peer))
                        {
                            //Task handleConnectToClient = Task.Run(async () => await peer.ConnectToClient());
                            peer.ConnectToClient();
                            await Console.Out.WriteLineAsync("Connection approved.");
                        }
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("Connection denied.");
                    }
                }
                else
                {
                    throw new Exception("TcpListenerSocket was null while while waiting for new connections.");
                }
            }
        }

        private async Task WaitForUdpClientConnection()
        {
            while(IsRunning)
            {
                /*if (UdpListenerSocket != null)
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
                }*/
            }
        }

        public abstract Task <bool>OnClientConnect(IPeer clientPeer);
        public abstract Task OnClientDisconnect(IPeer clientPeer);
        public abstract Task OnPacketReceived(IPeer clientPeer, Packet packet);
    }
}
