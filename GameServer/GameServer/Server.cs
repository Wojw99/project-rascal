using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkMessage;
using NetworkCore.NetworkCommunication;

namespace GameServer
{
    public class Server
    {
        public delegate void PacketHandler(Packet packet);

        private Dictionary<PacketType, PacketHandler> packetHandlers;

        public Server()
        {
            packetHandlers = new Dictionary<PacketType, PacketHandler>()
            {
                { PacketType.packet_player_move, ServerHandle.HandlePlayerMovePacket },
                { PacketType.packet_enemy_shoot, ServerHandle.HandleEnemyShootPacket },
                { PacketType.packet_test_packet, ServerHandle.HandleTestPacket },
            };
        }

        public void HandlePacket(Packet packet)
        {
            if (packetHandlers.TryGetValue(packet._type, out PacketHandler handler))
            {
                handler(packet);
            }
            else
            {
                // Obsługa nieznanego typu pakietu
            }
        }
    }
}
