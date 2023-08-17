using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using NetworkCore.NetworkMessage.old;
using NetworkCore.Packets;

namespace Client
{
    public class SimpleClient : NetworkClient
    {
        private TcpPeer? ServerPeer ;
        public TcpPeer? GetServerPeer
        {
            get
            {
                if (ServerPeer == null)
                {
                    throw new InvalidOperationException("ServerPeer must be initialized before accessing it.");
                }
                return ServerPeer;
            }
        }

        //ConcurrentDictionary<Guid, TcpPeer> servers = new ConcurrentDictionary<Guid, TcpPeer>();

        /*Dictionary<PacketType, PacketHandler> packetHandlers =
                new Dictionary<PacketType, PacketHandler>()
            {
                { PacketType.packet_player_move, new PacketHandler(PacketType.packet_player_move,
                 PacketFunction.HandleGlobalPlayerPosition, PacketFunction.SendPlayerMovePacket) },

                { PacketType.packet_enemy_shoot, new PacketHandler(PacketType.packet_enemy_shoot,
                PacketFunction.HandleGlobalPlayerPosition, PacketFunction.SendEnemyShootPacket) },

                { PacketType.packet_test_packet, new PacketHandler(PacketType.packet_test_packet,
                PacketFunction.HandleGlobalPlayerPosition, PacketFunction.SendTestPacket) },
            };

        PacketHandlerManager packetHandlerManager = new PacketHandlerManager();*/

        public SimpleClient() : base()
        {
            ServerPeer = null;
        }

        public SimpleClient(UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval) 
            : base(maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval) 
        {
            ServerPeer = null;
        }

        public override async Task OnPacketReceived(IPeer serverPeer, Packet packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.PacketType} from peer with Guid: {serverPeer.Id}");
        }

        public override async Task OnNewConnection(Socket ServerTcpSocket, Guid newConnectionId, Owner ownerType)
        {
            ServerPeer = new TcpPeer(this, ServerTcpSocket, newConnectionId, ownerType);
            await ServerPeer.ConnectToServer();
        }

        public override async Task OnServerDisconnect(IPeer serverPeer)
        {

        }
    }
}
