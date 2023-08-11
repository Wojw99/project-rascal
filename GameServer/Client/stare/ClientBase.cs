/*using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientBase
    {
        //public TcpClient client;
        //public NetworkStream stream;
        
        TcpPeer _TcpPeer { get; set; }
        PacketHandlerManager _PacketHandlerManager { get; set; }
        public ClientBase(string ip, int port)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(ip, port);
            _PacketHandlerManager = new PacketHandlerManager();

            Dictionary<PacketType, PacketHandler> packetHandlers =
                new Dictionary<PacketType, PacketHandler>()
            {
                { PacketType.packet_player_move, new PacketHandler(PacketType.packet_player_move,
                 PacketFunction.HandleGlobalPlayerPosition, PacketFunction.SendPlayerMovePacket) },

                { PacketType.packet_enemy_shoot, new PacketHandler(PacketType.packet_enemy_shoot,
                PacketFunction.HandleGlobalPlayerPosition, PacketFunction.SendEnemyShootPacket) },

                { PacketType.packet_test_packet, new PacketHandler(PacketType.packet_test_packet,
                PacketFunction.HandleGlobalPlayerPosition, PacketFunction.SendTestPacket) },
            };

            _PacketHandlerManager.InitHandlers(packetHandlers);


            _TcpPeer = new TcpPeer(tcpClient, _PacketHandlerManager);
            _TcpPeer.StartReceive();
        }

        public void Start(string ip, int port)
        {
           

            while(_TcpPeer.IsConnected)
            {

                _TcpPeer.SendPacket()
                Thread.Sleep(1000);
                Console.WriteLine("Nasłuchuje...");
            }

            *//*ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

            while (!client.Connected)
            {
                Console.WriteLine("Trying to connect...");

                try
                {
                    client.Connect(ip, port);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Unable to connect to the server");
                    Console.WriteLine("Press random key to retry, or ESC to quit.");

                    if (Console.KeyAvailable)
                    {
                        keyInfo = Console.ReadKey();
                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            break; // Wyjście z pętli po wciśnięciu ESC
                        }
                    }
                }
                //Thread.Yield();



            }
            
            if(client.Connected)
            {
                Console.WriteLine("Connected to server succesfully.");

                stream = client.GetStream();

                while (true)
                {
                    var random = new Random();
                    int randomInt = random.Next(3);

                    Thread.Sleep(100);

                    switch (randomInt)
                    {
                        case 0:
                            PacketFunction.SendEnemyShootPacket(stream);
                            break;
                        case 1:
                            PacketFunction.SendPlayerMovePacket(stream);
                            break;
                        case 2:
                            PacketFunction.SendTestPacket(stream);
                            break;
                    }

                    byte[] buffer = new byte[1024]; // Zakładam maksymalny rozmiar pakietu
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        // Tutaj możesz przetwarzać odczytane dane z bufora
                    }

                    *//*packet.WriteInt("playerNum", GlobalPlayerPositions.Count);
                    foreach (var playerPos in GlobalPlayerPositions)
                    {
                        packet.WriteInt("playerId", playerPos.Key);
                        packet.WriteDouble("posX", playerPos.Value.Item1);
                        packet.WriteDouble("posY", playerPos.Key);
                    }*//*
                }
                stream.Close();
            }
            
                client.Close();
            *//*
        }
    }
}
*/