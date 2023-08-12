using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;

namespace Client
{
    public class SimpleClient : NetworkClient
    {
        Dictionary<PacketType, PacketHandler> packetHandlers =
                new Dictionary<PacketType, PacketHandler>()
            {
                { PacketType.packet_player_move, new PacketHandler(PacketType.packet_player_move,
                 PacketFunction.HandleGlobalPlayerPosition, PacketFunction.SendPlayerMovePacket) },

                { PacketType.packet_enemy_shoot, new PacketHandler(PacketType.packet_enemy_shoot,
                PacketFunction.HandleGlobalPlayerPosition, PacketFunction.SendEnemyShootPacket) },

                { PacketType.packet_test_packet, new PacketHandler(PacketType.packet_test_packet,
                PacketFunction.HandleGlobalPlayerPosition, PacketFunction.SendTestPacket) },
            };

        PacketHandlerManager packetHandlerManager = new PacketHandlerManager();

        public SimpleClient() : base() 
        {
            packetHandlerManager.InitHandlers(packetHandlers);
        }

        public override async Task OnPacketReceived(IPeer serverPeer, Packet packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet._type} from peer with Guid: {serverPeer.Id}");

            PacketHandler pHandler = packetHandlerManager.GetHandler(packet._type);

            pHandler.HandleRequest(packet);

            Packet resPacket = pHandler.HandleResponse();

            await serverPeer.SendPacket(resPacket);
        }

        public override async Task<bool> OnServerConnect(IPeer serverPeer)
        {
            // TESTOWE
            await Console.Out.WriteLineAsync("Test funkcji");

            return true; // gdy połączenie się uda
        }

        public override async Task OnServerDisconnect(IPeer serverPeer)
        {

        }
    }
}
