using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using ServerApplication.GameService;
using NetworkCore.Packets;
using NetworkCore.NetworkUtility;
using NetworkCore.NetworkData;
using System.Linq.Expressions;

namespace ServerApplication.Game
{
    public class GameService
    {
        static async Task Main(string[] args)
        {
            try
            {
                TestServer Server = new TestServer(true, 120, "127.0.0.1", "Game Server", ServerType.world_server, 8051);

                ServerMonitor Monitor = new ServerMonitor(Server);

                Server.StartListen(); 
                Server.StartPacketProcessing(50, 50, TimeSpan.FromMilliseconds(20));
                Server.StartUpdate(TimeSpan.FromMilliseconds(50));

                await Task.Run(async () => await TestingOperationsTask(Server));

                /*while (Server.IsRunning)
                {
                    var begin = current_time();
                    Server.
                    receive_from_clients(); // poll, accept, receive, decode, validate
                    update(); // AI, simulate
                    send_updates_clients();
                    var elapsed = current_time() - begin;
                    if (elapsed < tick)
                    {
                        sleep(tick - elapsed);
                    }
                }*/


                while (true)
                {
                    await Console.Out.WriteLineAsync(".");
                }
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                Thread.Sleep(10000);
            }
        }

        public static async Task TestingOperationsTask(TestServer Server)
        {
            while(true)
            {
                Console.Clear();
                await Server._World.ShowConnectedPlayers();

                await Console.Out.WriteLineAsync("Choose player Id: ");

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                int playerChoice = int.Parse(keyInfo.KeyChar.ToString());

                if(playerChoice >= 0 && playerChoice <= Server._World.PlayerCount)
                {
                    try
                    {
                        await Server._World.ShowConnectedPlayers();
                        PlayerConnection playerRef = Server._World.GetPlayerObj(playerChoice);
                        await Console.Out.WriteLineAsync($"You choose char with id = {playerChoice}:");
                        await playerRef.CharacterObj.Show();

                        await Console.Out.WriteLineAsync("[1] Change Name");
                        await Console.Out.WriteLineAsync("[2] Add + 10 Health");
                        await Console.Out.WriteLineAsync("[3] Add + 10 Mana");

                        ConsoleKeyInfo keyInfo2 = Console.ReadKey();
                        int operationChoice = int.Parse(keyInfo2.KeyChar.ToString());
                        try
                        {
                            switch(operationChoice)
                            {
                                case 1:
                                    await Console.Out.WriteLineAsync("Choose new name: ");
                                    string newName = await Task.Run(() => Console.ReadLine());
                                    playerRef.SetName(newName);
                                    break;
                                case 2:
                                    playerRef.SetMaxHealth(playerRef.CharacterObj.MaxHealth + 10);
                                    break;
                                case 3:
                                    playerRef.SetMaxMana(playerRef.CharacterObj.MaxMana + 10);
                                    break;

                            }
                        }
                        catch(Exception ex)
                        { Console.WriteLine(ex.ToString());}
                    }
                    catch(Exception ex) 
                    {
                        await Console.Out.WriteLineAsync(ex.Message);
                        Thread.Sleep(10000);
                    }
                }
            }
        }
    }
}
