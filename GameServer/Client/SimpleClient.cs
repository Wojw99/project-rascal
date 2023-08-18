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
using NetworkCore.NetworkMessage.old;
using NetworkCore.Packets;

// authorization
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Client
{
    public class SimpleClient : NetworkClient
    {
        string TestAuthToken = "gracz"; // in the future we have to create authorization service 
        // we have to store also other information in Token, like username, and by username we can receive data from Database to that client

        private VisiblePlayersCollection PlayersCollection;
        private Player ClientPlayer;
        private TcpPeer? ServerPeer;

        // only for test purposes - to not start main thread loop before Player object doesnt loaded from server
        TaskCompletionSource<bool> ClientPlayerObjectSpecified;
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
            PlayersCollection = new VisiblePlayersCollection();
            ServerPeer = null;
            ClientPlayer = new Player();
            ClientPlayerObjectSpecified = new TaskCompletionSource<bool>();
        }

        public SimpleClient(UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval) 
            : base(maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval) 
        {
            ClientPlayerObjectSpecified = new TaskCompletionSource<bool>();
            PlayersCollection = new VisiblePlayersCollection();
            ServerPeer = null;
            ClientPlayer = new Player();
        }

        public override async Task OnPacketReceived(IPeer serverPeer, Packet packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.PacketType} from peer with Guid: {serverPeer.Id}");

            if(packet.PacketType == typeof(PlayerLoadResponsePacket))
            {
                PlayerLoadResponsePacket response = new PlayerLoadResponsePacket(packet);

                if(response.Succes == true)
                {
                    // set over ClientPlayer - Player class object
                    ClientPlayer = response.PlayerObj;
                    ClientPlayerObjectSpecified.SetResult(true);
                    // if all goes okey, then send Succes packet with 'true' parameter
                    await serverPeer.SendPacket(new PlayerLoadSuccesPacket(true));

                }
                else
                {
                    await serverPeer.Disconnect();
                }
            }

            // Note that this is packet from server, but with state of other player.
            if (packet.PacketType == typeof(PlayerStatePacket))
            {
                await PlayersCollection.OnPlayerStateReceived(new PlayerStatePacket(packet));
            }
        }

        public override async Task OnNewConnection(Socket ServerTcpSocket, Guid newConnectionId, Owner ownerType)
        {
            await Console.Out.WriteLineAsync($"[NEW SERVER CONNECTION] received, with info: {ServerTcpSocket.RemoteEndPoint} ");
            ServerPeer = new TcpPeer(this, ServerTcpSocket, newConnectionId, ownerType);

            // therefore, wait for the data
            await ServerPeer.ConnectToServer();

            // send request to client Player object with values from database
            await ServerPeer.SendPacket(new PlayerLoadRequestPacket(TestAuthToken));
        }

        public override async Task OnServerDisconnect(IPeer serverPeer)
        {
            await Console.Out.WriteLineAsync($"[SERVER CONNECTION CLOSED], with info: {serverPeer.PeerSocket.RemoteEndPoint}. ");
        }

        public async Task TestingOperationsTask() // run it in main program in while loop
        {
            if(!ClientPlayerObjectSpecified.Task.IsCompleted)
            {
                return;
            }

            await Console.Out.WriteLineAsync("---------------------------------------------");
            await Console.Out.WriteLineAsync("TWOJA POSTAC: ");
            await ClientPlayer.Show();
            await Console.Out.WriteLineAsync("---------------------------------------------");
            await Console.Out.WriteLineAsync("ZMIEN STAN SWOJEGO GRACZA");
            await Console.Out.WriteLineAsync("[1] Ustaw imie");
            await Console.Out.WriteLineAsync("[2] dodaj + 20 healtha");
            await Console.Out.WriteLineAsync("[3] dodaj + 20 many");
            await Console.Out.WriteLineAsync("[4] IDŹ DO GÓRY");
            await Console.Out.WriteLineAsync("[5] IDŹ W PRAWO");
            await Console.Out.WriteLineAsync("[6] IDŹ W DÓŁ");
            await Console.Out.WriteLineAsync("[7] IDŹ W LEWO");
            //await Console.Out.WriteLineAsync("[8] Send packet every 100ms");
            await Console.Out.WriteLineAsync("---------------------------------------------");
            await Console.Out.WriteLineAsync($"ZALOGOWANYCH GRACZY = {PlayersCollection.PlayerCount()}");
            await Console.Out.WriteLineAsync("--wybierz: ----------------------------------");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            int packetChoice = int.Parse(keyInfo.KeyChar.ToString());

            switch (packetChoice)
            {
                case 1:
                    {
                        await Console.Out.WriteLineAsync("Podaj nowe imię: ");
                        string newName = await Task.Run(() => Console.ReadLine());
                        ClientPlayer.pName = newName;
                        await ServerPeer.SendPacket(new PlayerStatePacket(ClientPlayer));
                        break;
                    }
                case 2:
                    {
                        ClientPlayer.pHealth += 20;
                        await ServerPeer.SendPacket(new PlayerStatePacket(ClientPlayer));
                        break;
                    }
                case 3:
                    {
                        ClientPlayer.pMana += 20;
                        await ServerPeer.SendPacket(new PlayerStatePacket(ClientPlayer));
                        break;
                    }
                case 4:
                    {
                        ClientPlayer.pPositionY += 1;
                        await ServerPeer.SendPacket(new PlayerStatePacket(ClientPlayer));
                        break;
                    }
                case 5:
                    {
                        ClientPlayer.pPositionX += 1;
                        await ServerPeer.SendPacket(new PlayerStatePacket(ClientPlayer));
                        break;
                    }
                case 6:
                    {
                        ClientPlayer.pPositionY -= 1;
                        await ServerPeer.SendPacket(new PlayerStatePacket(ClientPlayer));
                        break;
                    }
                case 7:
                    {
                        ClientPlayer.pPositionX -= 1;
                        await ServerPeer.SendPacket(new PlayerStatePacket(ClientPlayer));
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Nieznany wybór.");
                        break;
                    }
            }

        }
    }
}
