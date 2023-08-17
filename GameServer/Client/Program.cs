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
            SimpleClient client = new SimpleClient(50, 50, TimeSpan.FromMilliseconds(50));

            while (true)
            {
                try
                {
                    await client.ConnectTcpServer("192.168.5.2", 8051);


                    while (true)
                    {
                        await client.TestingOperationsTask();
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