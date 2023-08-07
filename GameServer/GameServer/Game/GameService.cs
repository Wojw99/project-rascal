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
            GameServer gameServer = new GameServer(true, 120, "127.0.01",
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

/*            Packet testPacket = new Packet(PacketType.packet_test_packet);
            // initializing with maximal values of each type
            testPacket.WriteInt("int", 2147483647);
            testPacket.WriteShort("short", 32767);
            testPacket.WriteLong("long", 9223372036854775807);
            testPacket.WriteDouble("double", 1.7976931348623157E+308);
            testPacket.WriteFloat("float", (float)3.4028235E+38);
            testPacket.WriteString("string", "testowanie pakietów");

            byte[] data = PacketSerializationManager.serializePacket(testPacket);
            Packet newPacket = PacketSerializationManager.DeserializeByteData(data);
            gameServer._PacketHandlerManager.HandlePacket(ref newPacket);*/
        }
    }
}
