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
    public abstract class TcpNetworkServer
    {
        protected bool AllowPhysicalClients { get; }

        protected int MaxClients { get; }

        protected Guid? ServerId { get; }

        protected ServerType _ServerType { get; }

        protected ServerProtocolType _ServerProtocolType { get; }

        protected string ServerName { get; }

        protected Socket TcpSocket { get; }

        protected PacketHandler _PacketHandler { get; private set; }
        protected PacketSender _PacketSender { get; private set; }
        //protected bool Listening { get; private set; }

        protected CancellationTokenSource CancellationSource = new CancellationTokenSource();
        private List<Task> ListeningTasks = new List<Task>();

        protected TcpNetworkServer (bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType, int tcpPort)
        {
            AllowPhysicalClients = allowPhysicalClients;
            MaxClients = maxClients;
            ServerId = Guid.NewGuid();
            ServerName = serverName;
            _ServerType = serverType;
            _ServerProtocolType = ServerProtocolType.protocol_tcp;
            _PacketHandler = new PacketHandler();
            _PacketSender = new PacketSender();

            // Create Socket
            TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpSocket.Bind(new IPEndPoint(IPAddress.Parse(publicIpAdress), tcpPort));
        }

        public void Start()
        {
            TcpSocket.Listen(10); // we can assign that in WaitForTcpClientConnection()
            Console.WriteLine($"Server, with GUID: {ServerId}, Name: {ServerName} started on TCP port.");

            Task handleWaitForTcpConnection = Task.Run(async () => await WaitForTcpClientConnection());

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                ListeningTasks.Add(Task.Run(WaitForTcpClientConnection, CancellationSource.Token));
            }

            Task handleUpdate = Task.Run(async () =>
            {
                while (!CancellationSource.Token.IsCancellationRequested)
                {
                    await OnServerTickUpdate();
                    await Task.Delay(1);
                }
            });

            OnServerStarted();
        }

        public void Stop()
        {
            TcpSocket.Close();

            CancellationSource.Cancel();
            Task.WhenAll(ListeningTasks).Wait();
        }

        private async Task WaitForTcpClientConnection()
        {
            while (!CancellationSource.Token.IsCancellationRequested)
            {
                if (TcpSocket != null)
                {
                    Socket tcpClientSocket = await TcpSocket.AcceptAsync();

                    await Console.Out.WriteLineAsync($"New TCP connection received, info: {tcpClientSocket.RemoteEndPoint}");

                    await OnClientConnect(tcpClientSocket, Guid.NewGuid(), Owner.server);
                }
                else
                {
                    throw new Exception("TcpListenerSocket was null while while waiting for new connections.");
                }
            }
        }

        protected abstract Task OnClientConnect(Socket clientSocket, Guid connId, Owner ownerType);

        protected abstract Task OnClientDisconnect(IPeer clientPeer);

        protected abstract Task OnServerTickUpdate();

        protected abstract Task OnServerStarted();
    }
}
