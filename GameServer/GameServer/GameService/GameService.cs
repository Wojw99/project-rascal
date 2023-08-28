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

namespace ServerApplication.Game
{
    public class GameService
    {
        static async Task Main(string[] args)
        {
            TestServer Server = new TestServer(true, 120, "192.168.5.5", "Game Server", ServerType.world_server, 8051);

            ServerMonitor Monitor = new ServerMonitor(Server);

            Server.StartListen(); 
            Server.RunPacketProcessingInBackground(50, 50, TimeSpan.FromMilliseconds(20));
            Server.StartUpdate(TimeSpan.FromMilliseconds(50));

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
                await TestingOperationsTask(Server);
            }
        }

        public static async Task TestingOperationsTask(TestServer Server)
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
                    PlayerConnection playerRef = Server._World.GetPlayerObj(playerChoice);
                    await Console.Out.WriteLineAsync($"You choose char with id = {playerChoice}:");
                    await playerRef.CharacterObj.Show();

                    await Console.Out.WriteLineAsync("[1] Change Name");
                    await Console.Out.WriteLineAsync("[2] Add + 10 Health");
                    await Console.Out.WriteLineAsync("[3] Add + 10 Mana");

                    ConsoleKeyInfo keyInfo2 = Console.ReadKey();
                    int operationChoice = int.Parse(keyInfo2.KeyChar.ToString());

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
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    Thread.Sleep(10000);
                }
            }
        }
    }
}
