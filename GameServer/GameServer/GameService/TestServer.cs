using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using ServerApplication.GameService.Base;

using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace ServerApplication.GameService
{
    public class TestServer : NetworkServer
    {
        // public World world { get; set; }
        public World _World;
        //public ConcurrentDictionary<Guid, PlayerConnection> qPeers { get; }

        private int ConnCounter = 0;
        private int packetReceiveCount = 0;

       /* Dictionary<PacketType, PacketHandler> packetHandlers =
                new Dictionary<PacketType, PacketHandler>()
            {
                { PacketType.packet_player_move, new PacketHandler(PacketType.packet_player_move,
                PacketFunction.HandlePlayerMovePacket, PacketFunction.HandleGlobalPlayerPosition) },

                { PacketType.packet_enemy_shoot, new PacketHandler(PacketType.packet_enemy_shoot,
                PacketFunction.HandleEnemyShootPacket, PacketFunction.HandleGlobalPlayerPosition) },

                { PacketType.packet_test_packet, new PacketHandler(PacketType.packet_test_packet,
                PacketFunction.HandleTestPacket, PacketFunction.HandleGlobalPlayerPosition) },
            };*/

        //PacketHandlerManager packetHandlerManager = new PacketHandlerManager();

        public TestServer(bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType,
            UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval,
            int? tcpPort = null, int? udpPort = null) 
            : base(allowPhysicalClients, maxClients, publicIpAdress, serverName, serverType, 
                  maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval, tcpPort, udpPort)
        {
            //packetHandlerManager.InitHandlers(packetHandlers);
            //qPeers = new ConcurrentDictionary<Guid, PlayerConnection> ();
            _World = new World();
        }

        public override async Task OnPacketReceived(IPeer peer, Packet packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.PacketType} from peer with Guid: {peer.Id}");
            await Console.Out.WriteLineAsync($"Received packets = {packetReceiveCount++}");
            if (peer is PlayerConnection playerConn)
            {
                if(packet.PacketType == typeof(PlayerLoadRequestPacket))
                {
                    PlayerLoadRequestPacket request = new PlayerLoadRequestPacket(packet);
                    
                    // check is token correct
                    if (request.AuthToken == "gracz")
                    {
                        // load username from token
                        string username = "some_username_from_token";

                        // load player object by username
                        playerConn.LoadPlayerFromDatabase(username);

                        // send response with player object
                        await playerConn.SendPacket(new PlayerLoadResponsePacket(true, playerConn._Player));
                        
                    }
                    else
                    {
                        // send response with succes = false
                        await playerConn.SendPacket(new PlayerLoadResponsePacket(false));

                        //disconnet Connection
                        await playerConn.Disconnect();
                    }  
                }

                // we can add Player to player collection only if client load his player succesfully
                if(packet.PacketType == typeof(PlayerLoadSuccesPacket))
                {
                    PlayerLoadSuccesPacket loadSuccesStatus = new PlayerLoadSuccesPacket(packet);

                    if (loadSuccesStatus.Succes == true)
                    {
                        await _World.AddNewPlayer(playerConn);
                        await _World.SendPlayerState(playerConn);
                    }
                }

                if (packet.PacketType == typeof(PlayerStatePacket) )
                {
                    await PlayerFunction.OnPlayerStateChanged(playerConn, new PlayerStatePacket(packet));

                    /*if (packet is PlayerStatePacket playerStatePacket)
                    {
                        await Console.Out.WriteLineAsync("testawdfasd");
                        await PlayerFunction.OnPlayerStateChanged(playerConnection, playerStatePacket);
                        // Obsługa PlayerStatePacket
                    }*/
                }
            }

        
        }

        TaskCompletionSource<bool> PlayerLoadCompletionSource;

        protected override async Task OnNewConnection(Socket clientSocket, Guid connId, Owner ownerType)
        {
            await Console.Out.WriteLineAsync($"[NEW CLIENT CONNECTION] received, with info: {clientSocket.RemoteEndPoint} ");

            PlayerConnection playerConn = new PlayerConnection(this, clientSocket, connId, ownerType, ConnCounter++);
            await playerConn.ConnectToClient();

        }

        protected override async Task OnClientDisconnect(IPeer peer)
        {
            await Console.Out.WriteLineAsync($"[CLIENT CONNECTION CLOSED], with info: {peer.PeerSocket.RemoteEndPoint}. ");

            if (peer is PlayerConnection playerConnection)  // do przemyslenia
                await _World.RemovePlayer(playerConnection); // do przemyslenia
        }
    }
}
