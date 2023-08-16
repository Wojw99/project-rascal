using Client;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        static async Task Main(string[] args)
        {

            /*ClientBase client = new ClientBase("localhost", 8050);
            client.Start("localhost", 8050);*/
            
            SimpleClient client = new SimpleClient();

            while(true)
            {
                try
                {
                    await client.ConnectTcpServer("127.0.0.1", 8051);
                    await client.Start();

                    while (true)
                    {
                        Thread.Sleep(200);
                        //await Console.Out.WriteLineAsync("Client is running...");
                        //Packet packet = PacketFunction.SendTestPacket();
                        PlayerMovePacket packet = new PlayerMovePacket((float)45645.234, float.MaxValue, (float)23523.2);

                        client.SendPacketToAllServers(packet);
                    }
                }
                catch(Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            }
        }
    }
}