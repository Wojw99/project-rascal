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
            if (peer is PlayerConnection playerConnection)
            {
                if (packet.PacketType == typeof(PlayerStatePacket) )
                {
                    await PlayerFunction.OnPlayerStateChanged(playerConnection, new PlayerStatePacket(packet));

                    /*if (packet is PlayerStatePacket playerStatePacket)
                    {
                        await Console.Out.WriteLineAsync("testawdfasd");
                        await PlayerFunction.OnPlayerStateChanged(playerConnection, playerStatePacket);
                        // Obsługa PlayerStatePacket
                    }*/
                }
            }

            
                // ... Tutaj możesz kontynuować dla innych typów pakietów
            

            /* packet.PacketType

             if (peer is PlayerConnection playerConnection)
             {
                 //await Console.Out.WriteLineAsync("test214");
                 if (packet is PlayerStatePacket playerStatePacket)
                 {
                     await PlayerFunction.OnPlayerStateChanged(playerConnection, playerStatePacket);
                     // Możesz teraz używać playerStatePacket jako obiektu PlayerStatePacket
                     // i wywołać na nim odpowiednie metody lub operacje.
                 }*/


            /* if (packet.PacketType == typeof(PlayerMovePacket))
             {
                 //PlayerMovePacket playerMovePacket = packet as PlayerMovePacket;
                 await PlayerFunction.OnPlayerMove(playerConnection, packet as PlayerMovePacket);
             }
             else if (packet.PacketType == typeof(PlayerStatePacket))
             {
                 PlayerStatePacket playerStatePacket = packet as PlayerStatePacket;
                 await PlayerFunction.OnPlayerStateChanged(playerConnection, playerStatePacket);
             }*/
        
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
