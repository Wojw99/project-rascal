using NetworkCore.NetworkCommunication;
using ServerApplication.GameService.Base;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace ServerApplication.GameService
{
    public class TestServer : TcpNetworkServer
    {
        // public World world { get; set; }
        public World _World;
        //public ConcurrentDictionary<Guid, PlayerConnection> qPeers { get; }

        private int ConnCounter = 0;
        private int packetReceiveCount = 0;

        public int VidCounter { get; private set; } = 0; // by now is the way to create unique identifiers
        // But note that in the future we must load that Vid's from database!

        /* Dictionary<PacketType, PacketHandler> packetHandlers =
                 new Dictionary<PacketType, PacketHandler>()
             {
                 { PacketType.packet_player_move, new PacketHandler(PacketType.packet_player_move,
                 PacketFunction.HandlePlayerMovePacket, PacketFunction.HandleGlobalPlayerPosition) },

                 { PacketType.packet_enemy_shoot, new PacketHandler(PacketType.packet_enemy_shoot,
                 PacketFunction.HandleEnemyShootPacket, PacketFunction.HandleGlobalPlayerPosition) },

                 { PacketType.packet_test_packet, new PacketHandler(PacketType.packet_test_packet,
                 PacketFunction.HandleTestPacket, PacketFunction.HandleGlobalPlayerPosition) },
             };*/

        //PacketHandlerManager packetHandlerManager = new PacketHandlerManager();

        public TestServer(bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType,
            UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval,
            int tcpPort) 
            : base(allowPhysicalClients, maxClients, publicIpAdress, serverName, serverType, 
                  maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval, tcpPort)
        {
            //packetHandlerManager.InitHandlers(packetHandlers);
            //qPeers = new ConcurrentDictionary<Guid, PlayerConnection> ();
            _World = new World();
        }

        public override async Task OnPacketReceived(IPeer peer, PacketBase packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.TypeId} from peer with Guid: {peer.Id}");
            await Console.Out.WriteLineAsync($"Received packets = {packetReceiveCount++}");

            // Check is received connection registered into PlayerConnection
            // note that we are registering all incoming connections into that Object
            // by now (in OnNewConnection method). So by now we dont need that especially.
            if (peer is PlayerConnection playerConn)
            {
                if (packet is CharacterLoadRequestPacket request)
                {
                    // check is token correct
                    if (request.AuthToken == "gracz")
                    {
                        // load username from token
                        string username = "some_username_from_token";

                        // load player object by username
                        playerConn.LoadCharacterFromDatabase(username, VidCounter++); // by now overloaded with unique identifiers from server app.

                        await playerConn.CharacterObj.Show();

                        // send response with player object
                          await playerConn.SendPacket(new CharacterLoadResponsePacket(playerConn.CharacterObj));
                        
                    }
                    else
                    {
                        // CharacterLoadResponsePacket Succes is false by default.
                        await playerConn.SendPacket(new CharacterLoadResponsePacket());

                        //disconnet Connection
                        await playerConn.Disconnect();
                    }  
                }

                // we can add Player to player collection only if client load his player succesfully
                else if (packet is CharacterLoadSuccesPacket loadSuccesStatus)
                {
                    if (loadSuccesStatus.Succes == true)
                    {
                        await _World.AddNewPlayer(playerConn);
                        await _World.SendPlayerStateToConnectedPlayers(playerConn);
                    }
                }

                else if (packet is CharacterStatePacket statePacket)
                {
                    // We run static method for that. We can load other packets in the same way.
                    // But by now I will write most of packets in OnPacketReceived
                    await PlayerFunction.OnCharacterStateChanged(playerConn, statePacket);
                }

                else if (packet is ClientDisconnectPacket clientDisconnect)
                {
                    await Console.Out.WriteLineAsync($"[CLIENT CONNECTION CLOSED], with info: {playerConn.PeerSocket.RemoteEndPoint}. ");

                    
                    if(clientDisconnect.AuthToken == "gracz") // we need that to check?
                    {
                        // save player state into database
                    }

                    await _World.RemovePlayer(playerConn); // delete from world
                    await playerConn.Disconnect(); // disconnect from server

                    
                }
            }

        
        }

        TaskCompletionSource<bool> PlayerLoadCompletionSource;

        protected override async Task OnNewConnection(Socket clientSocket, Guid connId, Owner ownerType)
        {
            await Console.Out.WriteLineAsync($"[NEW CLIENT CONNECTION] received, with info: {clientSocket.RemoteEndPoint} ");

            PlayerConnection playerConn = new PlayerConnection(this, clientSocket, connId, ownerType, ConnCounter++);
            await playerConn.ConnectToClient();

        }

        // now we are receiving ClientDisconnectPacket, so I think we dont need that.
        protected override async Task OnClientDisconnect(IPeer peer)
        {
            //await Console.Out.WriteLineAsync($"[CLIENT CONNECTION CLOSED], with info: {peer.PeerSocket.RemoteEndPoint}. ");

            if (peer is PlayerConnection playerConnection)  // do przemyslenia
                await _World.RemovePlayer(playerConnection); // do przemyslenia
        }
    }
}
