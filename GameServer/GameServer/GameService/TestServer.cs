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
            _PacketSender.OnPacketSent += ShowSentPacketInfo;
            _PacketHandler.OnPacketReceived += ShowReceivedPacketInfo;
        }

        private void ShowSentPacketInfo(string packetInfo)
        {
            Console.WriteLine("[SEND] " + packetInfo);
        }

        private void ShowReceivedPacketInfo(string packetInfo)
        {
            Console.WriteLine("[RECEIVED] " + packetInfo);
        }
        private async Task UpdateWorldAsync()
        {
            while (!CancellationSource.Token.IsCancellationRequested)
            {
                _World.Update();
                await Task.Delay(1);
            }
        }

        protected override async Task OnServerStarted()
        {
            await UpdateWorldAsync();
        }

        protected override async Task OnServerTickUpdate()
        {
            
        }


        protected override async Task OnClientConnect(Socket clientSocket, Guid connId, Owner ownerType)
        {
            await Console.Out.WriteLineAsync($"[NEW CLIENT CONNECTION] received, with info: {clientSocket.RemoteEndPoint} ");

            PlayerPeer playerConn = new PlayerPeer(_PacketHandler, _PacketSender, _World, clientSocket, connId, ownerType);
            playerConn.Connect();
            playerConn.StartRead();

        }

        // now we are receiving ClientDisconnectPacket, so I think we dont need that.
        protected override async Task OnClientDisconnect(IPeer peer)
        {
            await Console.Out.WriteLineAsync($"[CLIENT CONNECTION CLOSED], with info: {peer.PeerSocket.RemoteEndPoint}. ");

            if (peer is PlayerPeer playerConnection)  // do przemyslenia
                _World.RemovePlayer(playerConnection); // do przemyslenia
        }
    }
}
