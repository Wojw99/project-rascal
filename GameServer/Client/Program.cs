using Client;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkData;

namespace Client
{
    class Client
    {
        static bool BrakeThatHellLoop = false;
        static async Task Main(string[] args)
        {     
            SimpleClient client = new SimpleClient(50, 50, TimeSpan.FromMilliseconds(50));
            client.IsRunning = true;
            await client.RunPacketProcessingInBackground();

            // PRZYKŁAD ŁĄCZENIA Z SERWEREM AUTORYZACJI

            /*string authToken = string.Empty;

            TcpPeer AuthServer = await client.CreateTcpServerConnection("127.0.0.1", 8050);

            AuthServer.Connect();
            AuthServer.StartRead();

            string login = "login";
            string password = "password";

            AuthServer.SendPacket(new ClientLoginRequestPacket(login, password));

            try
            {
                PacketBase packet = await client.WaitForResponsePacket(TimeSpan.FromMilliseconds(100), 
                    TimeSpan.FromSeconds(20), PacketType.LOGIN_RESPONSE); // Parametry: 1.intervał, 2.limit czasu, 3.typ pakietu
                // Ustawiamy interwał aby nie obciążać procesora, gdyż przez podany limit
                // czasu będziemy próbować wyciągać pakiet z Dictionary odebranych pakietów z flagą response.

                if (packet is ClientLoginResponsePacket loginResponse)
                {
                    authToken = loginResponse.AuthToken;
                }

                // Pomyślne logowanie - wchodzimy np. do okna wyboru postaci, których też będzie trzeba załadować.
            }

            catch(TimeoutException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);

                // dodatkowo będzie trzeba wysłać pakiet z błędem podczas logowania.

                AuthServer.Disconnect();
            }*/

            try
            {
                string authToken = "gracz";

                // PRZYKŁAD ŁĄCZENIA Z SERWEREM GRY

                TcpPeer GameServer = await client.CreateTcpServerConnection("127.0.0.1", 8051);

                GameServer.Connect();
                GameServer.StartRead();
                
                // wczytujemy naszą postać. W przyszłości, jak byśmy chcieli mieć np. więcej slotów postaci,
                // to wyślemy dodatkowo wybrany slot. (będziemy też musieli sprawdzać czy ten slot jest pusty, itd...)
                GameServer.SendPacket(new CharacterLoadRequestPacket(authToken));

                try
                {
                    PacketBase packet = await client.WaitForResponsePacket(TimeSpan.FromMilliseconds(100), 
                        TimeSpan.FromSeconds(20), PacketType.CHARACTER_LOAD_RESPONSE); // Parametry: 1.intervał, 2.limit czasu, 3.typ pakietu

                    if (packet is CharacterLoadResponsePacket characterLoadResponse)
                    {
                        if (characterLoadResponse.Success == true)
                        {
                            Character PlayerCharacter = characterLoadResponse.GetCharacter();
                            
                            // Jeśli uda się wszystko załadować:
                            GameServer.SendPacket(new CharacterLoadSuccesPacket(true));

                            await Console.Out.WriteLineAsync("Character loaded succesfully, your character: ");
                            await PlayerCharacter.Show();
                        }
                        else
                        {
                            GameServer.SendPacket(new ClientDisconnectPacket(authToken));
                            GameServer.Disconnect();
                        }
                    }
                }
                catch(TimeoutException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    GameServer.SendPacket(new ClientDisconnectPacket(authToken));
                    GameServer.Disconnect();
                }

                while(true)
                {
                    await Task.Delay(1000);
                }
                  
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}