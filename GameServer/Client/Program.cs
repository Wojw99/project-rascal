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
    public class NetworkClient
    {
        static bool BrakeThatHellLoop = false;
        static AttributesPacket attributesPacket = new AttributesPacket(-1);
        static TransformPacket transformPacket = new TransformPacket(-1);
        static async Task Main(string[] args)
        {
            if(await CommitSendCharacterLoadRequest("gracz"))
            {
                while (true)
                {
                    await TestingOperationsTask();
                }
            }
        }

        public static async Task<bool> CommitSendCharacterLoadRequest(string authToken)
        {
            ClientSingleton Client = await ClientSingleton.GetInstanceAsync();

            await Client.GameServer.SendPacket(new CharacterLoadRequestPacket(authToken));

            PacketBase packet = await Client.WaitForResponsePacket(TimeSpan.FromMilliseconds(20),
                TimeSpan.FromSeconds(50), PacketType.CHARACTER_LOAD_RESPONSE);

            if (packet is CharacterLoadResponsePacket res)
            {
                CommitSendCharacterLoadSucces(true);
                attributesPacket = res.AttributesPacket;
                transformPacket = res.TransformPacket;
                return true;
            }
            else
            {
                CommitSendCharacterLoadSucces(false);
                return false;
            }
        }

        public static async void CommitSendCharacterLoadSucces(bool loadSucces)
        {
            ClientSingleton client = await ClientSingleton.GetInstanceAsync();
            await client.GameServer.SendPacket(new CharacterLoadSuccesPacket(loadSucces));
        }

        public static async void CommitSendPlayerCharacterTransfer()
        {
            ClientSingleton client = await ClientSingleton.GetInstanceAsync();

            await client.GameServer.SendPacket(transformPacket);

        }

        public static async Task TestingOperationsTask() // run it in main program in while loop
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
                        transformPacket.PosY += 1;
                        break;
                    }
                case 2:
                    {
                        transformPacket.PosX += 1;
                        break;
                    }
                case 3:
                    {
                        transformPacket.PosY -= 1;
                        break;
                    }
                case 4:
                    {
                        transformPacket.PosX -= 1;
                        break;
                    }
                case 5:
                    {
                        await Console.Out.WriteLineAsync("---------------------------------------------");
                        await Console.Out.WriteLineAsync("TWOJA POSTAC: ");
                       // await client.ClientPlayer.Show();
                        await Console.Out.WriteLineAsync("---------------------------------------------");
                        //await client.CharactersCollection.ShowCharacters();
                        await Console.Out.WriteLineAsync("Wcisnij dowolny klawisz");
                        ConsoleKeyInfo waitKeyInfo = Console.ReadKey();
                        break;
                    }
                default:
                    {
                        await Console.Out.WriteLineAsync("Nieznany wybór.");
                        break;
                    }
            }
            await Console.Out.WriteLineAsync("Wysylam pakiet z pozycja.");
            CommitSendPlayerCharacterTransfer();

        }
    }
}