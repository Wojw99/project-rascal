using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientBase
    {
        public TcpClient client;
        public NetworkStream stream;

        public ClientBase()
        {
            client = new TcpClient();
        }

        public void Start(string ip, int port)
        {
            
            client.Connect(ip, port);
            stream = client.GetStream();
            Console.WriteLine("Connected to server.");

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
            }

            stream.Close();
            client.Close();
        }
    }
}
