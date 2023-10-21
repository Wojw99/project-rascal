using NetworkCore.NetworkMessage;
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
            string serverName, ServerType serverType, int tcpPort)
            : base()
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

        public bool IsRunning()
        {
            return IsRunningFlag;
        }
        public void StartListen()
        {
            TcpSocket.Listen(10); // we can assign that in WaitForTcpClientConnection()
            IsRunningFlag = true;
            Console.WriteLine($"Server, with GUID: {ServerId}, Name: {ServerName} started on TCP port.");
            Task handleWaitForTcpConnection = Task.Run(async () => await WaitForTcpClientConnection());
        }

        public void StartUpdate(TimeSpan interval)
        {
            Task handleUpdate = Task.Run(async () =>
            {
                while(IsRunningFlag)
                {
                    await Update();
                    await Task.Delay(interval);
                }
            }); 
        }

        public async Task Stop()
        {
            IsRunningFlag = false;
            TcpSocket.Close(); 
        }

        public async Task Ping()
        {

        }

        private async Task WaitForTcpClientConnection()
        {
            while(IsRunningFlag)
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

        //public override abstract Task OnPacketReceived(IPeer clientPeer, PacketBase packet);

        protected abstract Task Update();
    }
}
