using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using static System.Net.Mime.MediaTypeNames;

namespace ServerApplication.Game
{
    public class PacketFunction
    {

        public static void HandleEnemyShootPacket(Packet packet)
        {
            int playerId = packet.ReadField<int>("playerId");
            int targetId = packet.ReadField<int>("targetId");

            Console.WriteLine($"Packet received: [playerId: {playerId}, targetId: {targetId}]");
            // wykonaj działania / wywołaj funkcje z innego namespace/folderu

        }

        public static void HandlePlayerMovePacket(Packet packet)
        {
            int playerId = packet.ReadField<int>("playerId");
            double posX = packet.ReadField<double>("posX");
            double posY = packet.ReadField<double>("posY");
            string test = packet.ReadField<string>("test");

            Console.WriteLine($"Packet received: [playerId: {playerId}, posX: {posX}, posY: {posY}, test: {test},]");
            // wykonaj działania / wywołaj funkcje z innego namespace/folderu
        }

        public static void HandleTestPacket(Packet packet)
        {
            int n1 = packet.ReadField<int>("int");
            short n2 = packet.ReadField<short>("short");
            long n3 = packet.ReadField<long>("long");
            double n4 = packet.ReadField<double>("double");
            float n5 = packet.ReadField<float>("float");
            string n6 = packet.ReadField<string>("string");

            Console.WriteLine($"Packet received: [int: {n1}, short: {n2}, long: {n3}, double: {n4}, float: {n5}, string: {n6}]");
            // wykonaj działania / wywołaj funkcje z innego namespace/folderu       
        }
    }
}
