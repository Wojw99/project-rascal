/*using NetworkCore.NetworkMessage;
using NetworkCore.NetworkMessage.old;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class PacketFunction
    {
        private static readonly object Lock = new object();

        public static Dictionary<int, (double, double)> GlobalPlayerPositions = new Dictionary<int, (double, double)>();


        public static void HandleGlobalPlayerPosition(Packet packet)
        {
            //Packet packet = new Packet(PacketType.packet_global_player_position);

            int playerNum = packet.Read<int>("playerNum");

            for (int i = 0; i < playerNum; i++)
            {
                int playerId = packet.Read<int>("playerId");
                double posX = packet.Read<double>("posX");
                double posY = packet.Read<double>("posY");

                GlobalPlayerPositions.Add(playerId, (posX, posY));
            }

            Console.WriteLine("Player positions = ");
            foreach (var playerPos in GlobalPlayerPositions)
            {
                Console.WriteLine($"playerId: {playerPos.Key}, posX: {playerPos.Value.Item1}, posY: {playerPos.Value.Item2}");
            }
        }
        public static Packet SendEnemyShootPacket()
        {
            Packet packet = new Packet(PacketType.packet_enemy_shoot);
            lock (Lock)
            {
                packet.Write("playerId", 1242325);
                packet.Write("targetId", 4355321);

                *//* byte[] data = PacketSerializationManager.serializePacket(packet);
                 stream.Write(data, 0, data.Length);
                 Console.WriteLine($"Send packet: {packet._type}");*//*
                return packet;
            }

        }

        public static Packet SendPlayerMovePacket()
        {
            lock (Lock)
            {
                Packet packet = new Packet(PacketType.packet_player_move);
                packet.Write("playerId", 456546);
                packet.Write("posX", 456.345);
                packet.Write("posY", 631.21);
                packet.Write("test", "jakies dodatkowe info");

                *//*byte[] data = PacketSerializationManager.serializePacket(packet);
                stream.Write(data, 0, data.Length);
                Console.WriteLine($"Send packet: {packet._type}");*//*
                return packet;
            }

        }

        public static Packet SendTestPacket()
        {
            lock (Lock)
            {
                Packet packet = new Packet(PacketType.packet_test_packet);
                packet.Write("int", 2147483647);
                packet.Write("short", 32767);
                packet.Write("long", 9223372036854775807);
                packet.Write("double", 1.7976931348623157E+308);
                packet.Write("float", (float)3.4028235E+38);
                packet.Write("string", "testowanie pakietów");

                *//* byte[] data = PacketSerializationManager.serializePacket(packet);
                 stream.Write(data, 0, data.Length);
                 Console.WriteLine($"Send packet: {packet._type}");*//*
                return packet;
            }

        }
    }
}
*/