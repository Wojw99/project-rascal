/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;

namespace ServerApplication.Game
{
    public class GameServer : INetworkServer
    {
        public bool AllowPhysicalClients { get; private set; }
        public int MaxClients { get; private set; }
        public string PublicIpAdress { get; private set; }
        public int? TcpPort { get; private set; }
        public int? UdpPort { get; private set; }
        public Guid? ServerId { get; private set; }
        public ServerType _ServerType { get; private set; }
        public string ServerName { get; private set; }
        public bool IsRunning { get; private set; }
        public TcpListener? _TcpListener;
        public UdpClient? _UdpListener;
        private readonly object _lock = new object();

        //public event EventHandler<PeerConnectedEventArgs> ClientConnected;
        //public event EventHandler<PeerDisconnectedEventArgs> ClientDisconnected;
        //public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public PacketHandlerManager _PacketHandlerManager { get; private set; }
        public ConnectionCollection _ConnectionCollection { get; private set; }

        public GameServer()
        {
            AllowPhysicalClients = true;
            MaxClients = 100;
            PublicIpAdress = "127.0.0.1";
            TcpPort = 8050;
            UdpPort = 8051;
            ServerId = Guid.NewGuid();
            ServerName = "Default Server";
            _ServerType = ServerType.default_server;
            _PacketHandlerManager = new PacketHandlerManager();
            _ConnectionCollection = new ConnectionCollection();
        }

        public GameServer(bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType, int? tcpPort = null, int? udpPort = null)
        {
            AllowPhysicalClients = allowPhysicalClients;
            MaxClients = maxClients;
            PublicIpAdress = publicIpAdress;
            TcpPort = tcpPort;
            UdpPort = udpPort;
            ServerId = Guid.NewGuid();
            ServerName = serverName;
            _ServerType = serverType;
            _PacketHandlerManager = new PacketHandlerManager();
            _ConnectionCollection = new ConnectionCollection();
        }

        public void InitFromFile(string configFileName)
        {
            if (IsRunning == false)
            {

            }
            else throw new Exception("Cannot init server from file, while it is running. ");
        }

        public void Start()
        {
            if (IsRunning)
                throw new Exception("Cannot start server, while it is running.");
            
            IsRunning = true;

            if (TcpPort.HasValue)
            {
                _TcpListener = new TcpListener(IPAddress.Any, TcpPort.Value);
                _TcpListener.Start();
                _TcpListener.BeginAcceptTcpClient(HandleTcpClientConnection, null);
            }

            if (UdpPort.HasValue)
            {
                _UdpListener = new UdpClient(UdpPort.Value);
                //_UdpListener.BeginReceive(HandleUdpClientData, null);
            }

            Console.WriteLine("Server started succesfully.");
            Console.WriteLine($"GUID: {ServerId}, SERVER NAME: {ServerName}, TCP port: {TcpPort}, UDP port: {UdpPort}," +
                $"PUBLIC IP: {PublicIpAdress}, AllowPhysicalClients: {AllowPhysicalClients}, MAX clients: {MaxClients}," +
                $"ServerType: {_ServerType}");

            while (IsRunning)
            {
                Console.WriteLine("Nasłuchiwanie..."); 
                Thread.Sleep(4000);
            }
        }

        public void Stop()
        {
            if (!IsRunning)
                throw new Exception("Cannot stop server, while it is's not running.");

            IsRunning = false;

            if (_TcpListener != null)
            {
                _TcpListener.Stop();
                _TcpListener = null;
            }

            if (_UdpListener != null)
            {
                _UdpListener.Close();
                _UdpListener = null;
            }
        }

        public void RegisterHandlers(Dictionary<PacketType, PacketHandler> packetHandlers)
        {
            _PacketHandlerManager.InitHandlers(packetHandlers);
        }

        private void HandleTcpClientConnection(IAsyncResult result)
        {
            if (!IsRunning)
                return;

            if(_TcpListener != null)
            {
                TcpClient tcpClient = _TcpListener.EndAcceptTcpClient(result);
                _TcpListener.BeginAcceptTcpClient(HandleTcpClientConnection, null);

                lock(_lock) // mamy locka w TcpPear.Connect(), więc tutaj albo tam nie będzie potrzeby dodawania nowego locka
                {
                    Console.WriteLine("Received new connection from ...");
                    TcpPeer tcpPeer = new TcpPeer(tcpClient, _PacketHandlerManager, );
                    _ConnectionCollection.Add(tcpPeer);
                    
                }
            }       
        }
    }
}
*/