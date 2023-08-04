using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkMessage;
using NetworkCore.NetworkCommunication;

namespace GameServer
{
    public class ServerHandle
    {

        public static void HandleEnemyShootPacket(Packet packet)
        {
            int playerId = packet.ReadField<int>("playerId");
            int targetId = packet.ReadField<int>("targetId");


            // wykonaj działania

        }

        public static void HandlePlayerMovePacket(Packet packet)
        {
            int playerId = packet.ReadField<int>("playerId");
            double posX = packet.ReadField<double>("posX");
            double posY = packet.ReadField<double>("posY");
            string test = packet.ReadField<string>("test");

            packet.WriteString("test", "xd");

            Console.WriteLine(" playerId = {0}, posX = {1}, posY = {2}, test = {3} ", playerId, posX, posY, test);
            // wykonaj działania
        }

        public static void HandleTestPacket(Packet packet)
        {
            Console.WriteLine(packet.ReadField<int> ("int"));
            Console.WriteLine(packet.ReadField<short>("short"));
            Console.WriteLine(packet.ReadField<long>("long"));
            Console.WriteLine(packet.ReadField<double>("double"));
            Console.WriteLine(packet.ReadField<float>("float"));
            Console.WriteLine(packet.ReadField<string>("string"));
        }
    }
}
