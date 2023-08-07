/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using ServerApplication.Game;

namespace ServerApplication
{
    public class Server //: INetworkServer
    {
        public delegate void PacketHandler(Packet packet);

        private Dictionary<PacketType, PacketHandler> packetHandlers;

        //public event Action<int> OnClientConnected;
        //public event Action<int> OnClientDisconnected;
        //public event Action<Connection, byte[]> OnDataReceived;

        public List<Connection> clients => throw new NotImplementedException();

        public bool IsRunning => throw new NotImplementedException();

        public Server()
        {
            packetHandlers = new Dictionary<PacketType, PacketHandler>()
            {
                { PacketType.packet_player_move, PacketFunction.HandlePlayerMovePacket },
                { PacketType.packet_enemy_shoot, PacketFunction.HandleEnemyShootPacket },
                { PacketType.packet_test_packet, PacketFunction.HandleTestPacket },
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



        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void SendDataToClient(Connection client, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void SendDataToAllClients(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
*/