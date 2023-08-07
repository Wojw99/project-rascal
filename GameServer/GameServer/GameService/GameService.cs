using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkMessage;
using NetworkCore.NetworkCommunication;

namespace ServerApplication.Game
{
    public class GameService
    {
        static void Main(string[] args)
        {
            GameServer gameServer = new GameServer(true, 120, "127.0.0.1",
            "Game Server", ServerType.world_server, 8050, null);

            Dictionary<PacketType, PacketHandlerManager.PacketHandler> packetHandlers = 
                new Dictionary<PacketType, PacketHandlerManager.PacketHandler>()
            {
                { PacketType.packet_player_move, PacketFunction.HandlePlayerMovePacket },
                { PacketType.packet_enemy_shoot, PacketFunction.HandleEnemyShootPacket },
                { PacketType.packet_test_packet, PacketFunction.HandleTestPacket },
            };
            //gameServer.InitFromFile("GameServerConfig.ini");
            gameServer.RegisterHandlers(packetHandlers);
            gameServer.Start();

        }
    }
}
