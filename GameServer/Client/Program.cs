using Client;
using NetworkCore.NetworkMessage;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            try
            {
                ClientBase client = new ClientBase();
                client.Start("localhost", 8050);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Thread.Sleep(10000);
            }
        }
    }
}