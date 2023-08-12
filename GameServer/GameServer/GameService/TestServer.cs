using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication.GameService
{
    public class TestServer : NetworkServer
    {
        Dictionary<PacketType, PacketHandler> packetHandlers =
                new Dictionary<PacketType, PacketHandler>()
            {
                { PacketType.packet_player_move, new PacketHandler(PacketType.packet_player_move,
                PacketFunction.HandlePlayerMovePacket, PacketFunction.HandleGlobalPlayerPosition) },

                { PacketType.packet_enemy_shoot, new PacketHandler(PacketType.packet_enemy_shoot,
                PacketFunction.HandleEnemyShootPacket, PacketFunction.HandleGlobalPlayerPosition) },

                { PacketType.packet_test_packet, new PacketHandler(PacketType.packet_test_packet,
                PacketFunction.HandleTestPacket, PacketFunction.HandleGlobalPlayerPosition) },
            };

        PacketHandlerManager packetHandlerManager = new PacketHandlerManager();

        public TestServer(bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType, int? tcpPort = null, int? udpPort = null) 
            : base(allowPhysicalClients, maxClients, publicIpAdress, serverName, serverType, tcpPort, udpPort)
        {
            packetHandlerManager.InitHandlers(packetHandlers);
        }

        public override async Task OnPacketReceived(IPeer peer, Packet packet)
        {
            //LOGGER: await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet._type} from peer with Guid: {peer.Id}");

            PacketHandler pHandler = packetHandlerManager.GetHandler(packet._type);

            pHandler.HandleRequest(packet);

            Packet resPacket = pHandler.HandleResponse();

            await peer.SendPacket(resPacket);
        }

        public override async Task<bool> OnClientConnect(IPeer peer)
        {
            // 1. loading world data for player.
            await Console.Out.WriteLineAsync( "Test funkcji");
            return true; //
        }

        public override async Task OnClientDisconnect(IPeer peer)
        {

        }
    }
}
