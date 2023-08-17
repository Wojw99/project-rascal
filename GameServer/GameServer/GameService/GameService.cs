using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using ServerApplication.GameService;
using NetworkCore.Packets;
using NetworkCore.Packets.Attributes;

namespace ServerApplication.Game
{
    public class GameService
    {
        static async Task Main(string[] args)
        {
            /*GameServer gameServer = new GameServer(true, 120, "127.0.0.1",
            "Game Server", ServerType.world_server, 8050, null);

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

            gameServer.RegisterHandlers(packetHandlers);
            gameServer.Start();*/


            TestServer server = new TestServer(true, 120, "192.168.5.2",
            "Game Server", ServerType.world_server, 50, 50, TimeSpan.FromMilliseconds(50), 8051, null);

            await server.Start();
            await server.RunPacketProcessingInBackground();

            while(true)
            {
                Thread.Sleep(200);

            }


        }
    }
}
