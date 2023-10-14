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
        public World _World;

        private int ConnCounter = 0;

        public int VidCounter { get; private set; } = 0; // by now is the way to create unique identifiers
        // But note that in the future we must load that Vid's from database!

        public TestServer(bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType, int tcpPort) 
            : base(allowPhysicalClients, maxClients, publicIpAdress, serverName, serverType, tcpPort)
        {
            _World = new World();
            OnPacketSent += showSentPacketInfo;
        }

        public void showSentPacketInfo(string packetInfo)
        {
            Console.WriteLine("[SEND] " + packetInfo);
        }

        protected override async Task Update()
        {
            await _World.Update();
        }

        public override async Task OnPacketReceived(IPeer peer, PacketBase packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.TypeId} from peer with Guid: {peer.Id}");

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

                        // send response with player object
                        await playerConn.SendPacket(new CharacterLoadResponsePacket(playerConn.CharacterObj));
                        
                    }
                    else
                    {
                        // CharacterLoadResponsePacket Succes is false by default.
                        await playerConn.SendPacket(new CharacterLoadResponsePacket());

                        //disconnet Connection
                        playerConn.Disconnect();
                    }  
                }

                // we can add Player to player collection only if client load his player succesfully
                else if (packet is CharacterLoadSuccesPacket loadSuccesStatus)
                {
                    if (loadSuccesStatus.Succes == true)
                    {
                        // In that method we also run OnCharacterLoad(), which simply sends to new connected
                        // player current states of all players.
                        await _World.AddNewPlayer(playerConn);

                    }
                }

                else if(packet is TransformPacket movePacket)
                {
                    playerConn.SetPosition(movePacket);
                }

                else if (packet is ClientDisconnectPacket clientDisconnect)
                {
                    await Console.Out.WriteLineAsync($"[CLIENT CONNECTION CLOSED], with info: {playerConn.PeerSocket.RemoteEndPoint}. ");

                    
                    if(clientDisconnect.AuthToken == "gracz") // we need that to check?
                    {
                        // save player state into database
                    }

                    await _World.RemovePlayer(playerConn); // delete from world
                    playerConn.Disconnect(); // disconnect from server

                    
                }

                else if(packet is PingRequestPacket pingRequest)
                {
                    await playerConn.SendPacket(new PingResponsePacket());
                }
            }

        
        }

        TaskCompletionSource<bool> PlayerLoadCompletionSource;

        protected override async Task OnNewConnection(Socket clientSocket, Guid connId, Owner ownerType)
        {
            await Console.Out.WriteLineAsync($"[NEW CLIENT CONNECTION] received, with info: {clientSocket.RemoteEndPoint} ");

            PlayerConnection playerConn = new PlayerConnection(this, clientSocket, connId, ownerType, ConnCounter++);
            playerConn.Connect();
            playerConn.StartRead();

        }

        // now we are receiving ClientDisconnectPacket, so I think we dont need that.
        protected override async Task OnClientDisconnect(IPeer peer)
        {
            //await Console.Out.WriteLineAsync($"[CLIENT CONNECTION CLOSED], with info: {peer.PeerSocket.RemoteEndPoint}. ");

            if (peer is PlayerConnection playerConnection)  // do przemyslenia
                _World.RemovePlayer(playerConnection); // do przemyslenia
        }
    }
}
