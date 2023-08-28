using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using NetworkCore.NetworkData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NetClient
{
    public class Client : MonoBehaviour
    {
        public delegate void OnCharacterStateUpdateReceivedDelegate(string packet);
        public event OnCharacterStateUpdateReceivedDelegate OnCharacterStateUpdateReceived;

        public GameCharacter ClientCharacter ;

        private static Client _instance;

        public static Client Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Client>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<Client>();
                        singletonObject.name = "ClientSingleton";
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        private TcpNetworkClient ClientNetwork { get; set; }

        string AuthToken = "gracz"; // in the future we have to create authorization service 
                                    // we have to store also other information in Token, like username, and by username we can receive data from Database to that client
        public TcpPeer AuthServer { get; private set; }
        public TcpPeer GameServer { get; private set; }

        public Client() 
        {
            ClientNetwork = new TcpNetworkClient();
            ClientNetwork.RunPacketProcessingInBackground(50, 50, TimeSpan.FromMilliseconds(20));
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public async Task ConnectToAuthServer()
        {
            AuthServer = await ClientNetwork.CreateTcpServerConnection("127.0.0.1", 8050);
            AuthServer.Connect();
            AuthServer.StartRead();
        }

        public async Task ConnectToGameServer()
        {
            GameServer = await ClientNetwork.CreateTcpServerConnection("192.168.5.5", 8051);
            GameServer.Connect();
            GameServer.StartRead();
        }

        public async Task RegisterNewAccount(string login, string password)
        {
            if(AuthServer.IsConnected)
            {

            }
        }

        public async Task <bool>LoginToServer(string login, string password)
        {
            if (AuthServer.IsConnected)
            {
                await AuthServer.SendPacket(new ClientLoginRequestPacket(login, password));

                try
                {
                    PacketBase packet = await ClientNetwork.WaitForResponsePacket(TimeSpan.FromMilliseconds(100),
                        TimeSpan.FromSeconds(20), PacketType.LOGIN_RESPONSE); 

                    if (packet is ClientLoginResponsePacket loginResponse)
                    {
                        AuthToken = loginResponse.AuthToken;
                        return true;
                    }
                }

                catch (TimeoutException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    AuthServer.Disconnect();
                }
            }
            return false;
        }

        public async Task LoadCharacter(int slotNum)
        {
            if(GameServer.IsConnected)
            {

                await GameServer.SendPacket(new CharacterLoadRequestPacket(AuthToken));

                try
                {
                    PacketBase packet = await ClientNetwork.WaitForResponsePacket(TimeSpan.FromMilliseconds(20),
                        TimeSpan.FromSeconds(20), PacketType.CHARACTER_LOAD_RESPONSE); // Parametry: 1.intervał, 2.limit czasu, 3.typ pakietu

                    if (packet is CharacterLoadResponsePacket characterLoadResponse)
                    {
                        if (characterLoadResponse.Success == true)
                        {
                            //ClientNetwork.ClientPlayer = characterLoadResponse.GetCharacter();
                            Character ClientChar = characterLoadResponse.GetCharacter();


                            // Jeśli uda się wszystko załadować:
                            await GameServer.SendPacket(new CharacterLoadSuccesPacket(true));

                            await Console.Out.WriteLineAsync("Character loaded succesfully.");
                            
                            
                        }
                        else
                        {
                            await GameServer.SendPacket(new ClientDisconnectPacket(AuthToken));
                            GameServer.Disconnect();
                        }
                    }
                }
                catch (TimeoutException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    await GameServer.SendPacket(new ClientDisconnectPacket(AuthToken));
                    GameServer.Disconnect();
                }
            }
        }
    }
}
