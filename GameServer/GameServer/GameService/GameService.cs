using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using ServerApplication.GameService;
using NetworkCore.Packets;
using NetworkCore.NetworkUtility;

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


            TestServer Server = new TestServer(true, 120, "127.0.0.1",
            "Game Server", ServerType.world_server, 50, 50, TimeSpan.FromMilliseconds(50), 8051);

            ServerMonitor Monitor = new ServerMonitor(Server);

            await Server.StartListen(); 
            await Server.RunPacketProcessingInBackground();

            /*while (Server.IsRunning)
            {
                var begin = current_time();
                Server.
                receive_from_clients(); // poll, accept, receive, decode, validate
                update(); // AI, simulate
                send_updates_clients();
                var elapsed = current_time() - begin;
                if (elapsed < tick)
                {
                    sleep(tick - elapsed);
                }
            }*/


            while (true)
            {
                await Task.Delay(2000);
            }
        }
    }
}
