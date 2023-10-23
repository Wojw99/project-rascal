using NetworkCore.NetworkCommunication;
using ServerApplication.GameService.Base;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections.Concurrent;
using NetworkCore.NetworkUtility;
using ServerApplication.GameService.Player;

namespace ServerApplication.GameService
{
    public class TestServer : TcpNetworkServer
    {
        private World _World;

        public int VidCounter { get; private set; } = 0;

        public TestServer(bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType, int tcpPort) 
            : base(allowPhysicalClients, maxClients, publicIpAdress, serverName, serverType, tcpPort)
        {
            _World = new World();
            //OnPacketSent += showSentPacketInfo;
            //OnPacketReceived += ExaminePacket;
        }

        public void showSentPacketInfo(string packetInfo)
        {
            Console.WriteLine("[SEND] " + packetInfo);
        }

        protected override async Task Update()
        {
            await _World.Update();
        }

        protected override async Task OnNewConnection(Socket clientSocket, Guid connId, Owner ownerType)
        {
            await Console.Out.WriteLineAsync($"[NEW CLIENT CONNECTION] received, with info: {clientSocket.RemoteEndPoint} ");

            PlayerPeer playerConn = new PlayerPeer(_PacketHandler, _World, clientSocket, connId, ownerType);
            playerConn.Connect();
            playerConn.StartRead();

        }

        // now we are receiving ClientDisconnectPacket, so I think we dont need that.
        protected override async Task OnClientDisconnect(IPeer peer)
        {
            //await Console.Out.WriteLineAsync($"[CLIENT CONNECTION CLOSED], with info: {peer.PeerSocket.RemoteEndPoint}. ");

            if (peer is PlayerPeer playerConnection)  // do przemyslenia
                _World.RemovePlayer(playerConnection); // do przemyslenia
        }
    }
}
