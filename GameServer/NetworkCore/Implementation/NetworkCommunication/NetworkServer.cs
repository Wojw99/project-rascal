using NetworkCore.NetworkConfig;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;

/*namespace NetworkCore.NetworkCommunication
{

    public class NetworkServer : INetworkServer
    {
        private TcpListener tcpListener;
        private UdpClient udpListener;

        public ServerConfiguration Configuration { get; private set; }
        public bool IsRunning { get; private set; }

        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public NetworkServer(ServerConfiguration config)
        {
            Configuration = config;
        }

        public void Start()
        {
            if (IsRunning)
                return;

            IsRunning = true;

            if (Configuration.TcpPort.HasValue)
            {
                tcpListener = new TcpListener(IPAddress.Any, Configuration.TcpPort.Value);
                tcpListener.Start();
                tcpListener.BeginAcceptTcpClient(HandleTcpClientConnection, null);
            }

            if (Configuration.UdpPort.HasValue)
            {
                udpListener = new UdpClient(Configuration.UdpPort.Value);
                udpListener.BeginReceive(HandleUdpClientData, null);
            }
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            IsRunning = false;

            if (tcpListener != null)
            {
                tcpListener.Stop();
                tcpListener = null;
            }

            if (udpListener != null)
            {
                udpListener.Close();
                udpListener = null;
            }
        }

        private void HandleTcpClientConnection(IAsyncResult result)
        {
            if (!IsRunning)
                return;

            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(HandleTcpClientConnection, null);

            // Obsługa nowego połączenia klienta - np. dodanie do listy klientów, obsługa zdarzeń itp.

            // Powiadomienie o nowym połączeniu
            OnClientConnected(tcpClient);
        }

        private void HandleUdpClientData(IAsyncResult result)
        {
            if (!IsRunning)
                return;

            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, Configuration.UdpPort.Value);
            byte[] data = udpListener.EndReceive(result, ref remoteEndPoint);
            udpListener.BeginReceive(HandleUdpClientData, null);

            // Obsługa odebranych danych UDP - np. przetwarzanie danych itp.

            // Powiadomienie o odebranych danych
            OnMessageReceived(remoteEndPoint, data);
        }

        private void OnClientConnected(TcpClient tcpClient)
        {
            ClientConnected?.Invoke(this, new ClientConnectedEventArgs { TcpClient = tcpClient });
        }

        private void OnMessageReceived(IPEndPoint remoteEndPoint, byte[] data)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs
            {
                ClientId = Guid.NewGuid(), // W przykładzie nadajemy losowy identyfikator klienta
                Data = data
            });
        }
    }
    
}
*/