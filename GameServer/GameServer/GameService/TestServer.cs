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
            string serverName, ServerType serverType, int? tcpPort = null, int? udpPort = null) 
            : base(allowPhysicalClients, maxClients, publicIpAdress, serverName, serverType, tcpPort, udpPort)
        {
            //packetHandlerManager.InitHandlers(packetHandlers);
            //qPeers = new ConcurrentDictionary<Guid, PlayerConnection> ();
            _World = new World(this);
        }

        protected override async Task OnPacketReceived(IPeer peer, Packet packet)
        {
            if(peer is PlayerConnection playerConnection)
            {
                if(packet is PlayerMovePacket playerMovePacket)
                    await PlayerFunction.OnPlayerMove(playerConnection, playerMovePacket);

                else if(packet is PlayerStatePacket playerStatePacket)
                    await PlayerFunction.OnPlayerStateChanged(playerConnection, playerStatePacket);
            }     
        }

        protected override async Task OnNewConnection(Socket clientSocket, Guid connId, Owner ownerType)
        {
            PlayerConnection playerConn = new PlayerConnection(this, clientSocket, connId, ownerType, ConnCounter++);
            await _World.AddNewPlayer(playerConn);
        }

        protected override async Task OnClientDisconnect(IPeer peer)
        {
            if (peer is PlayerConnection playerConnection)
                await _World.RemovePlayer(playerConnection);
        }
    }
}
