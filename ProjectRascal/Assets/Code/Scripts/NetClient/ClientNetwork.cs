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
    public class ClientNetwork : TcpNetworkClient
    {
        public static string publicServerIpAdress = "127.0.0.1";
        public static int AuthServerPort = 8050;
        public static int GameServerPort = 8051;

        string TestAuthToken = "gracz"; // in the future we have to create authorization service 
        // we have to store also other information in Token, like username, and by username we can receive data from Database to that client

        private VisibleCharacterCollection CharactersCollection;
        private Character ClientCharacter;

        public TcpPeer GameServerPeer;
        public TcpPeer AuthServerPeer; 

        // only for test purposes - to not start main thread loop before Player object doesnt loaded from server
        TaskCompletionSource<bool> ClientPlayerObjectSpecified;

        public 

        private static ClientNetwork _Instance;

        public static ClientNetwork Instance
        {
            get
            {
                if (_Instance == null)
                {
                    return new ClientNetwork(100, 100, System.TimeSpan.FromMilliseconds(50));
                }

                return _Instance;
            }
        }

        public void Awake()
        {
            ConnectTcpServer(publicServerIpAdress, AuthServerPort);

        }

        public void Update()
        {
            
        }

        public ClientNetwork(UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval)
            : base(maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval)
        {
            ClientPlayerObjectSpecified = new TaskCompletionSource<bool>();
            CharactersCollection = new VisibleCharacterCollection();
            ClientCharacter = new Character();
        }

        public override async Task OnPacketReceived(IPeer serverPeer, PacketBase packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.TypeId} from peer with Guid: {serverPeer.Id}");
            
            if(serverPeer == GameServerPeer)
            {
                if (packet is CharacterLoadResponsePacket response)
                {

                    if (response.Success == true)
                    {
                        // set over ClientPlayer - Player class object
                        ClientCharacter = response.GetCharacter();
                        ClientPlayerObjectSpecified.SetResult(true);
                        // if all goes okey, then send Succes packet with 'true' parameter
                        await serverPeer.SendPacket(new CharacterLoadSuccesPacket(true));

                    }
                    else
                    {
                        await serverPeer.Disconnect();
                    }
                }
            }
            else if(serverPeer == AuthServerPeer)
            {

            }

            


            // Note that this is packet from server, but with state of other player.

            if (packet is CharacterStatePacket statePacket)
            {
                await CharactersCollection.OnCharacterStateReceived(statePacket);
            }

        }

        public override async Task OnNewConnection(Socket ServerTcpSocket, Guid newConnectionId, Owner ownerType)
        {
            await Console.Out.WriteLineAsync($"[NEW SERVER CONNECTION] received, with info: {ServerTcpSocket.RemoteEndPoint} ");
            
            
            
            ServerPeer = new TcpPeer(this, ServerTcpSocket, newConnectionId, ownerType);

            // therefore, wait for the data
            await ServerPeer.ConnectToServer();

            // send request to client Player object with values from database
            await ServerPeer.SendPacket(new CharacterLoadRequestPacket(TestAuthToken));
        }

        public override async Task OnServerDisconnect(IPeer serverPeer)
        {
            await Console.Out.WriteLineAsync($"[SERVER CONNECTION CLOSED], with info: {serverPeer.PeerSocket.RemoteEndPoint}. ");
        }

        public async Task TestingOperationsTask() // run it in main program in while loop
        {
            if (!ClientPlayerObjectSpecified.Task.IsCompleted)
            {
                return;
            }

            await Console.Out.WriteLineAsync("---------------------------------------------");
            await Console.Out.WriteLineAsync("TWOJA POSTAC: ");
            await ClientCharacter.Show();
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
            await Console.Out.WriteLineAsync($"ZALOGOWANYCH GRACZY = {CharactersCollection.PlayerCount()}");
            await Console.Out.WriteLineAsync("---------------------------------------------");
            await Console.Out.WriteLineAsync($"WYSLANYCH PAKIETOW = {this.OutPacketCounter}");
            await Console.Out.WriteLineAsync($"ODEBRANYCH PAKIETOW = {this.InPacketCounter}");
            await Console.Out.WriteLineAsync("---------------------------------------------");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            int packetChoice = int.Parse(keyInfo.KeyChar.ToString());

            switch (packetChoice)
            {
                case 1:
                    {
                        await Console.Out.WriteLineAsync("Podaj nowe imię: ");
                        string newName = await Task.Run(() => Console.ReadLine());
                        ClientCharacter.Name = newName;
                        await ServerPeer.SendPacket(new CharacterStatePacket(ClientCharacter));
                        break;
                    }
                case 2:
                    {
                        ClientCharacter.Health += 20;
                        await ServerPeer.SendPacket(new CharacterStatePacket(ClientCharacter));
                        break;
                    }
                case 3:
                    {
                        ClientCharacter.Mana += 20;
                        await ServerPeer.SendPacket(new CharacterStatePacket(ClientCharacter));
                        break;
                    }
                case 4:
                    {
                        ClientCharacter.PositionY += 1;
                        await ServerPeer.SendPacket(new CharacterStatePacket(ClientCharacter));
                        break;
                    }
                case 5:
                    {
                        ClientCharacter.PositionX += 1;
                        await ServerPeer.SendPacket(new CharacterStatePacket(ClientCharacter));
                        break;
                    }
                case 6:
                    {
                        ClientCharacter.PositionY -= 1;
                        await ServerPeer.SendPacket(new CharacterStatePacket(ClientCharacter));
                        break;
                    }
                case 7:
                    {
                        ClientCharacter.PositionX -= 1;
                        await ServerPeer.SendPacket(new CharacterStatePacket(ClientCharacter));
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
