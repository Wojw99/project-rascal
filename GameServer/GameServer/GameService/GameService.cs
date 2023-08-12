using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkMessage;
using NetworkCore.NetworkCommunication;
using ServerApplication.GameService;

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

            TestServer server = new TestServer(true, 120, "127.0.0.1",
            "Game Server", ServerType.world_server, 8051, null);

            await server.Start();

            while(true)
            {
                Thread.Sleep(15000);
                await Console.Out.WriteLineAsync("Serwer uruchomiony.");
                //await server.Update(20);
            }


        }
    }
}
