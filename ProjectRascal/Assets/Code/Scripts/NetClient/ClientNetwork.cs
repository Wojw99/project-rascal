using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.NetClient
{
    public class ClientNetwork : NetworkClient
    {
        //private static readonly object instanceLock = new object();

        string TestAuthToken = "gracz"; // in the future we have to create authorization service 
        // we have to store also other information in Token, like username, and by username we can receive data from Database to that client

        public VisibleCharacterCollection PlayersCollection;
        public Player ClientPlayer;
        public TcpPeer ServerPeer;

    /*    private static ClientNetwork instance;
        public static ClientNetwork Instance
        {
            get
            {
                lock(instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ClientNetwork("192.168.5.2", 8051, 100, 100, TimeSpan.FromSeconds(50));
                    
                    }
                    return instance;

                }
            }
        }*/

        public ClientNetwork(UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval)
            : base(maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval)
        {
            PlayersCollection = new VisibleCharacterCollection();
            ServerPeer = null;
            ClientPlayer = new Player();
            

            //Task task = Start(serverIpAdress, serverPort);
        }

/*        public async Task Start(string serverIpAdress, int serverPort)
        {
            await ConnectTcpServer(serverIpAdress, serverPort);
        }*/

        public override async Task OnPacketReceived(IPeer serverPeer, Packet packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.PacketType} from peer with Guid: {serverPeer.Id}");

            if (packet.PacketType == typeof(PlayerLoadResponsePacket))
            {
                PlayerLoadResponsePacket response = new PlayerLoadResponsePacket(packet);

                if (response.Succes == true)
                {
                    // set over ClientPlayer - Player class object
                    ClientPlayer = response.PlayerObj;
                    // ClientPlayerObjectSpecified.SetResult(true);

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
    }
}
