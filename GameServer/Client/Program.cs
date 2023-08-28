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
    class NetworkClient
    {
        static bool BrakeThatHellLoop = false;
        static async Task Main(string[] args)
        {     
            TcpNetworkClient client = new TcpNetworkClient();
            client.IsRunning = true;
            client.RunPacketProcessingInBackground(50, 50, TimeSpan.FromMilliseconds(20);

            // PRZYKŁAD ŁĄCZENIA Z SERWEREM AUTORYZACJI

            *//*string authToken = string.Empty;

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
            }*//*

            try
            {
                string authToken = "gracz";

                // PRZYKŁAD ŁĄCZENIA Z SERWEREM GRY

                TcpPeer GameServer = await client.CreateTcpServerConnection("192.168.5.5", 8051);

                GameServer.Connect();
                GameServer.StartRead();

                // wczytujemy naszą postać. W przyszłości, jak byśmy chcieli mieć np. więcej slotów postaci,
                // to wyślemy dodatkowo wybrany slot. (będziemy też musieli sprawdzać czy ten slot jest pusty, itd...)

                //Character PlayerCharacter = new Character();
                await GameServer.SendPacket(new CharacterLoadRequestPacket(authToken));

                try
                {
                    PacketBase packet = await client.WaitForResponsePacket(TimeSpan.FromMilliseconds(20), 
                        TimeSpan.FromSeconds(20), PacketType.CHARACTER_LOAD_RESPONSE); // Parametry: 1.intervał, 2.limit czasu, 3.typ pakietu

                    if (packet is CharacterLoadResponsePacket characterLoadResponse)
                    {
                        if (characterLoadResponse.Success == true)
                        {
                            client.ClientPlayer = characterLoadResponse.GetCharacter();
                            
                            // Jeśli uda się wszystko załadować:
                            await GameServer.SendPacket(new CharacterLoadSuccesPacket(true));

                            await Console.Out.WriteLineAsync("Character loaded succesfully.");
                        }
                        else
                        {
                            await GameServer.SendPacket(new ClientDisconnectPacket(authToken));
                            GameServer.Disconnect();
                        }
                    }
                }
                catch(TimeoutException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    await GameServer.SendPacket(new ClientDisconnectPacket(authToken));
                    GameServer.Disconnect();
                }

                while(true)
                {
                    await TestingOperationsTask(GameServer, client);
                    *//*Stopwatch watch = new Stopwatch();

                    watch.Start();
                    await GameServer.SendPacket(new PingRequestPacket());

                    PacketBase packet = await client.WaitForResponsePacket(TimeSpan.FromMilliseconds(10), 
                        TimeSpan.FromSeconds(20), PacketType.PING_RESPONSE);

                    watch.Stop();
                    await Console.Out.WriteLineAsync($"Ping time: {watch.ElapsedMilliseconds}");
                    watch.Reset();*//*
                    //Thread.Sleep(1000);

                }
                  
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        public static async Task TestingOperationsTask(IPeer serverPeer, NetworkClient client) // run it in main program in while loop
        {
            Console.Clear();
            await Console.Out.WriteLineAsync("---------------------------------------------");
            await Console.Out.WriteLineAsync("[1] IDŹ DO GÓRY");
            await Console.Out.WriteLineAsync("[2] IDŹ W PRAWO");
            await Console.Out.WriteLineAsync("[3] IDŹ W DÓŁ");
            await Console.Out.WriteLineAsync("[4] IDŹ W LEWO");
            await Console.Out.WriteLineAsync("[5] Pokaż graczy");
            //await Console.Out.WriteLineAsync("[8] Send packet every 100ms");
            await Console.Out.WriteLineAsync("---------------------------------------------");
            //await Console.Out.WriteLineAsync($"ZALOGOWANYCH GRACZY = {PlayersCollection.PlayerCount()}");
            await Console.Out.WriteLineAsync("---------------------------------------------");
            //await Console.Out.WriteLineAsync($"WYSLANYCH PAKIETOW = {this.OutPacketCounter}");
            //await Console.Out.WriteLineAsync($"ODEBRANYCH PAKIETOW = {this.InPacketCounter}");
            await Console.Out.WriteLineAsync("---------------------------------------------");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            int packetChoice = int.Parse(keyInfo.KeyChar.ToString());

            switch (packetChoice)
            {
                case 1:
                    {
                        client.ClientPlayer.PositionY += 1;
                        await serverPeer.SendPacket(new CharacterMovePacket(client.ClientPlayer));
                        break;
                    }
                case 2:
                    {
                        client.ClientPlayer.PositionX += 1;
                        await serverPeer.SendPacket(new CharacterMovePacket(client.ClientPlayer));
                        break;
                    }
                case 3:
                    {
                        client.ClientPlayer.PositionY -= 1;
                        await serverPeer.SendPacket(new CharacterMovePacket(client.ClientPlayer));
                        break;
                    }
                case 4:
                    {
                        client.ClientPlayer.PositionX -= 1;
                        await serverPeer.SendPacket(new CharacterMovePacket(client.ClientPlayer));
                        break;
                    }
                case 5:
                    {
                        await Console.Out.WriteLineAsync("---------------------------------------------");
                        await Console.Out.WriteLineAsync("TWOJA POSTAC: ");
                        await client.ClientPlayer.Show();
                        await Console.Out.WriteLineAsync("---------------------------------------------");
                        await client.CharactersCollection.ShowCharacters();
                        await Console.Out.WriteLineAsync("Wcisnij dowolny klawisz");
                        ConsoleKeyInfo waitKeyInfo = Console.ReadKey();
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