using NetworkCore.NetworkMessage;
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
    public abstract class TcpNetworkServer : NetworkBase
    {
        protected bool AllowPhysicalClients { get; }

        protected int MaxClients { get; }

        protected Guid? ServerId { get; }

        protected ServerType _ServerType { get; }

        protected ServerProtocolType _ServerProtocolType { get; }

        protected string ServerName { get; }

        protected Socket TcpSocket { get; }

        protected TcpNetworkServer (bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType,
            UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval,
            int tcpPort)
            : base(maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval)
        {
            AllowPhysicalClients = allowPhysicalClients;
            MaxClients = maxClients;
            ServerId = Guid.NewGuid();
            ServerName = serverName;
            _ServerType = serverType;
            _ServerProtocolType = ServerProtocolType.protocol_tcp;

            // Create Socket
            TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpSocket.Bind(new IPEndPoint(IPAddress.Parse(publicIpAdress), tcpPort));
        }

        /*protected TcpNetworkServer(string configFileName)
        {
            // read from file and initialize attributes.
        }

        public void InitFromFile(string configFileName)
        {
            // read from file and initialize attributes.
        }*/

        // Start listening, setting IsRunning flag.
        public async Task StartListen()
        {
            TcpSocket.Listen(10); // we can assign that in WaitForTcpClientConnection()
            IsRunning = true;
            await Console.Out.WriteLineAsync($"Server, with GUID: {ServerId}, Name: {ServerName} started on TCP port.");
            Task handleWaitForTcpConnection = Task.Run(async () => await WaitForTcpClientConnection());
        }

        public async Task Stop()
        {
            IsRunning = false;
            TcpSocket.Close(); 
        }

        public async Task Ping()
        {

        }

        private async Task WaitForTcpClientConnection()
        {
            while(IsRunning)
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

        protected abstract Task OnNewConnection(Socket clientSocket, Guid connId, Owner ownerType);

        protected abstract Task OnClientDisconnect(IPeer clientPeer);

        public override abstract Task OnPacketReceived(IPeer clientPeer, Packet packet);
    }
}
