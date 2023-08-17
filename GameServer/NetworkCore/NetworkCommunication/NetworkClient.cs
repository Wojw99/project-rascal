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
    public abstract class NetworkClient : NetworkBase
    {

        //public ConcurrentDictionary<Guid, IPeer> qPeers { get; }

        // public ConcurrentQueue<OwnedPacket> qPacketsIn { get; }

        //public ConcurrentQueue<OwnedPacket> qPacketsOut { get; }

        //public bool IsRunning { get; private set; }

        public NetworkClient() : base() { }

        public NetworkClient (UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval) 
        : base(maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval) { }

        public async Task ConnectTcpServer(string serverIpAddress, int serverTcpPort)
        {
            Socket ServerTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await ServerTcpSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverIpAddress), (int)serverTcpPort));

            if(ServerTcpSocket.Connected)
            {
                await OnNewConnection(ServerTcpSocket, Guid.NewGuid(), Owner.client);

                /*if (await OnServerConnect(serverPeer))
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
                }*/
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
        }

        public async Task Stop()
        {
            IsRunning = false;
        }

        public override async Task SendOutgoingPacket(OwnedPacket receiver)
        {
            byte[] dataToSend = receiver.PeerPacket.SerializePacket();
            await receiver.Peer.PeerSocket.SendAsync(new ArraySegment<byte>(dataToSend), SocketFlags.None);
            Watch.Stop();
            await Console.Out.WriteLineAsync($"[SEND] packed with type: {receiver.PeerPacket.PacketType} from peer with Guid: {receiver.Peer.Id}");
            await Console.Out.WriteLineAsync($"With time = {Watch.ElapsedMilliseconds} ms");
            Watch.Reset();
        }

        /*public void SendPacketToAllServers(Packet packet)
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
        }*/

        //public override abstract Task<bool> OnServerConnect(IPeer serverPeer);
        public abstract Task OnNewConnection(Socket ServerTcpSocket, Guid newConnectionId, Owner ownerType);
        public abstract Task OnServerDisconnect(IPeer serverPeer);
        public override abstract Task OnPacketReceived(IPeer serverPeer, Packet packet);
    }
}
