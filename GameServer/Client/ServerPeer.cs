using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ServerPeer : TcpPeer
    {
        private IPEndPoint ServerIpEndpoint;

        public ServerPeer(PacketHandler packetHandler, PacketSender packetSender, string serverIpAddress, int serverTcpPort)
            : base(packetHandler, packetSender, new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp),
                  Guid.NewGuid(), Owner.client)
        {
            ServerIpEndpoint = new IPEndPoint(IPAddress.Parse(serverIpAddress), serverTcpPort);
        }

        public void ConnectToServer()
        {
            Console.WriteLine("Trying to connect to server...");
            PeerSocket.Connect(ServerIpEndpoint);

            if (PeerSocket.Connected)
            {
                Console.WriteLine($"Succesfully connected with server, on port = " +
                    $"{ServerIpEndpoint.Port}, adress = {ServerIpEndpoint.Address}.");
            }
        }
    }
}
