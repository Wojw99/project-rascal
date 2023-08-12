using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public static class PacketFunction
{
    private static readonly object Lock = new object();

    public static Dictionary<int, (double, double)> GlobalPlayerPositions = new Dictionary<int, (double, double)>();


    public static void HandleGlobalPlayerPosition(Packet packet)
    {
        //Packet packet = new Packet(PacketType.packet_global_player_position);

        int playerNum = packet.ReadField<int>("playerNum");

        for (int i = 0; i < playerNum; i++)
        {
            int playerId = packet.ReadField<int>("playerId");
            double posX = packet.ReadField<double>("posX");
            double posY = packet.ReadField<double>("posY");

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
            packet.WriteInt("playerId", 1242325);
            packet.WriteInt("targetId", 4355321);

            /* byte[] data = PacketSerializationManager.serializePacket(packet);
                stream.Write(data, 0, data.Length);
                Console.WriteLine($"Send packet: {packet._type}");*/
            return packet;
        }

    }

    public static Packet SendPlayerMovePacket()
    {
        lock (Lock)
        {
            Packet packet = new Packet(PacketType.packet_player_move);
            packet.WriteInt("playerId", 456546);
            packet.WriteDouble("posX", 456.345);
            packet.WriteDouble("posY", 631.21);
            packet.WriteString("test", "jakies dodatkowe info");

            /*byte[] data = PacketSerializationManager.serializePacket(packet);
            stream.Write(data, 0, data.Length);
            Console.WriteLine($"Send packet: {packet._type}");*/
            return packet;
        }

    }

    public static Packet SendTestPacket()
    {
        lock (Lock)
        {
            Packet packet = new Packet(PacketType.packet_test_packet);
            packet.WriteInt("int", 2147483647);
            packet.WriteShort("short", 32767);
            packet.WriteLong("long", 9223372036854775807);
            packet.WriteDouble("double", 1.7976931348623157E+308);
            packet.WriteFloat("float", (float)3.4028235E+38);
            packet.WriteString("string", "testowanie pakietów");

            /* byte[] data = PacketSerializationManager.serializePacket(packet);
                stream.Write(data, 0, data.Length);
                Console.WriteLine($"Send packet: {packet._type}");*/
            return packet;
        }

    }
}

