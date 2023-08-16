using Client;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using NetworkCore.Packets.Attributes;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace Client
{
    class Client
    {
        static bool BrakeThatHellLoop = false;
        static async Task Main(string[] args)
        {     
            SimpleClient client = new SimpleClient();

            while (true)
            {
                try
                {
                    await client.ConnectTcpServer("127.0.0.1", 8051);
                    await client.Start();
                    await client.Update(100, 100, TimeSpan.FromMilliseconds(50));





                    /*Packet packet = new Packet(typeof(Packet));

                    packet.Write("ID", 12412);
                    packet.Write("NAME", "Lucian");
                    packet.Write("SURNAME", "Zawadzki");
                    packet.Write("PASSWORD", "lucek123k3f2azd5dfgd343dsf");
                    packet.Write("MESSAGE", "Czesc jestem lucek i testuje sobie dzialanie wielu funkcji w programie, nie wiem co tutaj napisać, ale mam" +
                        "nadzieje ze serwer poprawnie odczyta moją wiadomość pozdro! Fajnie jakby odczytał też polskie znaki");*/
                    //Task inputTask = Task.Run(async () => { await HandleInputKey(); });

                    PlayerStatePacket packet = new PlayerStatePacket();

                    int id = 0;
                    string name = "Jan";
                    int health = 0;
                    int packetSendCount = 0;

                    /*string userInput = string.Empty;

                    while (userInput != string.Empty && userInput.Length < 5)
                    {
                        await Console.Out.WriteLineAsync("ustal imie: ");
                        userInput = await Console.In.ReadLineAsync();
                    }*/

                    while (true)
                    {
                        await Console.Out.WriteLineAsync("[1] Id");
                        await Console.Out.WriteLineAsync("[2] Name");
                        await Console.Out.WriteLineAsync("[3] Health");
                        await Console.Out.WriteLineAsync("[4] Send packet every 100ms");

                        ConsoleKeyInfo keyInfo = Console.ReadKey();

                        if (char.IsDigit(keyInfo.KeyChar))
                        {
                            int packetChoice = int.Parse(keyInfo.KeyChar.ToString());
                            //packet.Clear();

                            switch (packetChoice)
                            {
                            case 1:
                                id += 1;
                                packet.Clear();
                                packet.SetId(id);
                                break;

                            case 2:
                                name += "*";
                                packet.Clear();
                                packet.SetName(name);
                                break;
                            case 3:
                                health += 20;
                                packet.Clear();
                                packet.SetHealth(health);
                                break;
                            case 4:

                                BrakeThatHellLoop = false;
                                PlayerStatePacket extremePacket = new PlayerStatePacket();
                                extremePacket.SetPositionX((float)12.51);
                                extremePacket.Init(1, "Lucian", 200, 100, (float)12153.7125, (float)16543.312, (float)123.152, (float)45613.1122);

                                while (!BrakeThatHellLoop)
                                {
                                    await client.ServerPeer.SendPacket(packet);
                                    packetSendCount++;
                                    await Task.Delay(100);
                                }
                                await Console.Out.WriteLineAsync($"Sended Packets = {packetSendCount}");
                                break;
                            }

                            if(packetChoice != 4)
                            {
                                await client.ServerPeer.SendPacket(packet);
                                await Console.Out.WriteLineAsync($"Sended Packets = {packetSendCount++}");
                            }

                        }
                    }
                }
                catch(Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            }
        }

        static async Task HandleInputKey()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.KeyChar == 'q')
                    {

                        BrakeThatHellLoop = true;
                    }
                }
            }
        }
    }
}