using NetworkCore.NetworkMessage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkCommunication
{
    public abstract class TcpNetworkClient : NetworkBase
    {
        public TcpNetworkClient() : base() { }

        public TcpNetworkClient (UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval) 
        : base(maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval) { }

        public async Task ConnectTcpServer(string serverIpAddress, int serverTcpPort)
        {
            Socket ServerTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await Console.Out.WriteLineAsync("Trying to connect to server...");
            await ServerTcpSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverIpAddress), (int)serverTcpPort));

            if(ServerTcpSocket.Connected)
            {
                IsRunning = true;
                await RunPacketProcessingInBackground();
                await OnNewConnection(ServerTcpSocket, Guid.NewGuid(), Owner.client);
            }
        }

        public async Task Stop()
        {
            IsRunning = false;
        }

        //public override abstract Task<bool> OnServerConnect(IPeer serverPeer);
        public abstract Task OnNewConnection(Socket ServerTcpSocket, Guid newConnectionId, Owner ownerType);
        public abstract Task OnServerDisconnect(IPeer serverPeer);
        public override abstract Task OnPacketReceived(IPeer serverPeer, PacketBase packet);
    }
}
