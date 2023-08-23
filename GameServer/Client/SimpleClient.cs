using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;

// authorization
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Client
{
    public class SimpleClient : TcpNetworkClient
    {
        string TestAuthToken = "gracz"; // in the future we have to create authorization service 
        // we have to store also other information in Token, like username, and by username we can receive data from Database to that client

        private VisibleCharactersCollection CharactersCollection;
        private Character ClientPlayer;
        //private TcpPeer? ServerPeer;

        //TaskCompletionSource<bool> ResponseReceived;


        // only for test purposes - to not start main thread loop before Player object doesnt loaded from server

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
            CharactersCollection = new VisibleCharactersCollection();
            ClientPlayer = new Character();
            //ClientPlayerObjectSpecified = new TaskCompletionSource<bool>();
        }

        public SimpleClient(UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval) 
            : base(maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval) 
        {
            //ClientPlayerObjectSpecified = new TaskCompletionSource<bool>();
            CharactersCollection = new VisibleCharactersCollection();
            ClientPlayer = new Character();
        }

        public override async Task OnPacketReceived(IPeer serverPeer, PacketBase packet)
        {
            //await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.TypeId} from peer with Guid: {serverPeer.Id}");
         
            if(packet is CharacterStatesUpdatePacket characterStatesPacket)
            {
                
                foreach(var statePacket in characterStatesPacket.PacketCollection)
                {
                    
                    await CharactersCollection.OnCharacterStateReceived(statePacket);
                    // await Console.Out.WriteLineAsync("otrzymano");
                }
                //foreach (var Packet in characterStatesPacket.PacketCollection)
                    //await PlayersCollection.OnCharacterStateReceived(Packet);
            }

            // Note that this is packet from server, but with state of other player.

            /*if (packet is CharacterStatePacket statePacket)
            {
                await PlayersCollection.OnCharacterStateReceived(statePacket);
            }*/
            
        }

        /*public override async Task OnNewConnection(Socket ServerTcpSocket, Guid newConnectionId, Owner ownerType)
        {
            await Console.Out.WriteLineAsync($"[NEW SERVER CONNECTION] received, with info: {ServerTcpSocket.RemoteEndPoint} ");
            ServerPeer = new TcpPeer(this, ServerTcpSocket, newConnectionId, ownerType);

            // therefore, wait for the data
            await ServerPeer.ConnectToServer();

            // send request to client Player object with values from database
            await ServerPeer.SendPacket(new CharacterLoadRequestPacket(TestAuthToken));
        }*/

        /*public override async Task OnServerDisconnect(IPeer serverPeer)
        {
            await Console.Out.WriteLineAsync($"[SERVER CONNECTION CLOSED], with info: {serverPeer.PeerSocket.RemoteEndPoint}. ");
        }*/

        
    }
}
